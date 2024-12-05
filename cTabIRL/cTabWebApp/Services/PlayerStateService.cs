using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Prometheus;

namespace cTabWebApp
{
    internal class PlayerStateService : IPlayerStateService
    {
        private readonly Gauge ActiveArmaConnections;
        private readonly Gauge ActiveWebConnections;
        private readonly Gauge ActiveTacMapConnections;
        private readonly Gauge ActiveSessions;
        private readonly Gauge ActiveSessionsWithSteam;
        private readonly Gauge ActiveSessionsWithTacMap;
        private readonly Counter TotalSessions;

        private static readonly List<PlayerState> players = new List<PlayerState>();
        private static readonly string[] notASteamId = new[] { "_SP_PLAYER_", "_SP_AI_", "" };
        private static int nextId = 1;

        public PlayerStateService(CollectorRegistry registry)
        {
            var factory = Metrics.WithCustomRegistry(registry);
            ActiveArmaConnections = factory.CreateGauge("arma_connected", "Active Arma Connections");
            ActiveWebConnections = factory.CreateGauge("web_connected", "Active Web Connections");
            ActiveTacMapConnections = factory.CreateGauge("tacmap_connected", "Active connections to TacMap");
            ActiveSessions = factory.CreateGauge("sessions_active", "Active sessions");
            ActiveSessionsWithSteam = factory.CreateGauge("sessions_active_steam", "Sessions connected to Steam");
            ActiveSessionsWithTacMap = factory.CreateGauge("sessions_active_tacmap", "Sessions connected to TacMap");
            TotalSessions = factory.CreateCounter("sessions_total", "Total sessions created");
            registry.AddBeforeCollectCallback(UpdateStats);
        }

        public PlayerState GetStateByToken(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return null;
            }
            lock (players)
            {
                return players.FirstOrDefault(p => p.Token == token);
            }
        }

        public KeyLoginResult GetTokenBySteamIdAndKey(string steamId, string key)
        {
            List<PlayerState> candidates;
            lock (players)
            {
                candidates = players.Where(p => p.SteamId == steamId).ToList();
            }
            if (candidates.Count == 0)
            {
                return new KeyLoginResult(KeyLoginState.UnknownPlayer, null);
            }
            var state = candidates.FirstOrDefault(p => p.HashedKey == HashKeyForHost(key, p.KeyHostname));
            if (state == null)
            {
                return new KeyLoginResult(KeyLoginState.BadKey, null);
            }
            return new KeyLoginResult(KeyLoginState.Ok, state.Token);
        }

        public PlayerState GetOrCreateStateBySteamIdAndKey(string steamId, string hashedKey, string keyHostname)
        {
            PlayerState state = null;
            if (!notASteamId.Contains(steamId))
            {
                lock (players)
                {
                    state = players.FirstOrDefault(p => p.SteamId == steamId && p.HashedKey == hashedKey && p.KeyHostname == keyHostname);
                }
            }
            if (state == null)
            {
                TotalSessions.Inc();

                var id = Interlocked.Increment(ref nextId);
                state = new PlayerState()
                {
                    Id = id,
                    HashedKey = hashedKey,
                    SteamId = steamId,
                    KeyHostname = keyHostname,
                    Token = GenerateToken(id),
                    SpectatorToken = GenerateToken(id),
                    LastActivityUtc = DateTime.UtcNow
                };
                lock (players)
                {
                    players.RemoveAll(p => p.ActiveConnections == 0 && p.LastActivityUtc < DateTime.UtcNow.AddDays(-1));
                    players.Add(state);
                }
            }
            return state;
        }

        private static string HashKeyForHost(string key, string hostname)
        {
            if (string.IsNullOrEmpty(key))
            {
                return string.Empty;
            }
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: key,
                            salt: Encoding.UTF8.GetBytes(hostname),
                            prf: KeyDerivationPrf.HMACSHA256,
                            iterationCount: 10000,
                            numBytesRequested: 256 / 8));
        }

        private string GenerateToken(int id)
        {
            var random = RandomNumberGenerator.GetBytes(32);
            // Includes id to avoid collision
            return id.ToString("X") + "x" + Convert.ToBase64String(random).Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }

        public IEnumerable<PlayerState> GetUserAuthenticatedStates(string steamId)
        {
            List<PlayerState> candidates;
            lock (players)
            {
                candidates = players.Where(p => p.SteamId == steamId && p.IsAuthenticated).ToList();
            }
            return candidates;
        }

        public PlayerState GetStateBySpectatorToken(string spectatorToken)
        {
            if (string.IsNullOrEmpty(spectatorToken))
            {
                return null;
            }
            lock (players)
            {
                return players.FirstOrDefault(p => p.SpectatorToken == spectatorToken);
            }
        }
        public PlayerStateServiceStats GetStats(bool isAdmin)
        {
            lock (players)
            {
                return new PlayerStateServiceStats()
                {
                    ActiveArmaConnections = players.Sum(p => p.ActiveArmaConnections),
                    ActiveWebConnections = players.Sum(p => p.ActiveWebConnections),
                    ActiveTacMapConnections = players.Count(p => p.Interconnect != null),
                    ActiveSessions = players.Count(p => p.ActiveConnections > 0),
                    ActiveSessionsWithSteam = players.Count(p => p.ActiveConnections > 0 && p.IsAuthenticated),
                    ActiveSessionsWithTacMap = players.Count(p => p.ActiveConnections > 0 && p.SyncedTacMapId != null),
                    TotalSessions = nextId - 1
                };
            }
        }

        private void UpdateStats()
        {
            lock (players)
            {
                ActiveArmaConnections.Set(players.Sum(p => p.ActiveArmaConnections));
                ActiveWebConnections.Set(players.Sum(p => p.ActiveWebConnections));
                ActiveTacMapConnections.Set(players.Count(p => p.Interconnect != null));
                ActiveSessions.Set(players.Count(p => p.ActiveConnections > 0));
                ActiveSessionsWithSteam.Set(players.Count(p => p.ActiveConnections > 0 && p.IsAuthenticated));
                ActiveSessionsWithTacMap.Set(players.Count(p => p.ActiveConnections > 0 && p.SyncedTacMapId != null));
            }
        }
    }
}
