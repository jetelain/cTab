using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

#nullable enable

namespace cTabWebApp.Recording
{
    public class RecordingService : IRecordingService
    {
        // Per-user list, populated lazily on first access
        private readonly ConcurrentDictionary<string, List<StoredRecording>> _userIndex = new();
        private readonly HashSet<string> _loadedUsers = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        private readonly string _storageDirectory;
        private readonly TimeSpan _retentionDuration;
        private readonly int _maxSessionRecordingCount;

        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public RecordingService(RecordingServiceConfig config)
        {
            _retentionDuration = config.RetentionDuration;
            _maxSessionRecordingCount = config.MaxSessionRecordingCount;
            _storageDirectory = config.StorageLocation
                ?? Path.Combine(Path.GetTempPath(), "cTabWebApp", "SessionRecordings");
            Directory.CreateDirectory(_storageDirectory);
        }

        private bool IsExpired(StoredRecording entry) => entry.ExpiresUtc < DateTime.UtcNow;

        private string UserDirectory(string steamId) => Path.Combine(_storageDirectory, steamId);
        private string MetaPath(string steamId, string token) => Path.Combine(UserDirectory(steamId), token + ".json");
        private string DataPath(string steamId, string token) => Path.Combine(UserDirectory(steamId), token + ".json.gz");

        /// <summary>
        /// Loads the user's recording metadata from disk into the in-memory index.
        /// Must be called while holding <see cref="_semaphore"/>.
        /// </summary>
        private void EnsureUserLoadedLocked(string steamId)
        {
            if (_loadedUsers.Contains(steamId))
            {
                return;
            }
            var recordings = new List<StoredRecording>();
            var userDir = UserDirectory(steamId);
            if (Directory.Exists(userDir))
            {
                foreach (var file in Directory.EnumerateFiles(userDir, "*.json"))
                {
                    var token = Path.GetFileNameWithoutExtension(file);
                    try
                    {
                        var entry = JsonSerializer.Deserialize<StoredRecording>(File.ReadAllText(file), _jsonOptions);
                        if (entry != null && entry.Token == token && entry.SteamId == steamId && !IsExpired(entry))
                        {
                            recordings.Add(entry);
                        }
                        else
                        {
                            DeleteFilesNoLock(steamId, token);
                        }
                    }
                    catch (JsonException)
                    {
                        DeleteFilesNoLock(steamId, token);
                    }
                }
            }
            _userIndex[steamId] = recordings;
            _loadedUsers.Add(steamId);
        }

        public async Task<StoredRecording?> SaveAsync(string steamId, SessionRecording recording)
        {
            StoredRecording entry;
            await _semaphore.WaitAsync();
            try
            {
                EnsureUserLoadedLocked(steamId);
                EnforceUserQuotaLocked(steamId);
                var token = AllocateTokenLocked(steamId);
                entry = new StoredRecording
                {
                    Token = token,
                    SteamId = steamId,
                    WorldName = recording.WorldName,
                    RecordingStart = recording.RecordingStart,
                    RecordingEnd = recording.RecordingEnd,
                    ExpiresUtc = DateTime.UtcNow + _retentionDuration
                };
                _userIndex[steamId].Add(entry);
            }
            finally
            {
                _semaphore.Release();
            }

            Directory.CreateDirectory(UserDirectory(steamId));
            await File.WriteAllTextAsync(MetaPath(steamId, entry.Token), JsonSerializer.Serialize(entry, _jsonOptions));

            await using var fileStream = File.Create(DataPath(steamId, entry.Token));
            await using var gzStream = new GZipStream(fileStream, CompressionLevel.Optimal);
            await JsonSerializer.SerializeAsync(gzStream, recording, _jsonOptions);

            return entry;
        }

        public IReadOnlyList<StoredRecording> GetByUser(string steamId)
        {
            _semaphore.Wait();
            try
            {
                EnsureUserLoadedLocked(steamId);
                return _userIndex[steamId]
                    .Where(r => !IsExpired(r))
                    .OrderByDescending(r => r.RecordingStart)
                    .ToList();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public Stream? OpenRecording(StoredRecording stored)
        {
            var path = DataPath(stored.SteamId, stored.Token);
            if (!File.Exists(path))
            {
                return null;
            }
            return new GZipStream(File.OpenRead(path), CompressionMode.Decompress);
        }

        public async Task CleanUpAsync()
        {
            if (!Directory.Exists(_storageDirectory))
            {
                return;
            }
            var toDelete = new List<(string steamId, string token)>();
            await _semaphore.WaitAsync();
            try
            {
                foreach (var userDir in Directory.EnumerateDirectories(_storageDirectory))
                {
                    var steamId = Path.GetFileName(userDir);
                    EnsureUserLoadedLocked(steamId);
                    var recordings = _userIndex[steamId];
                    var expired = recordings.Where(IsExpired).ToList();
                    foreach (var e in expired)
                    {
                        recordings.Remove(e);
                        toDelete.Add((steamId, e.Token));
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }
            foreach (var (steamId, token) in toDelete)
            {
                DeleteFilesNoLock(steamId, token);
            }
        }

        private void EnforceUserQuotaLocked(string steamId)
        {
            var recordings = _userIndex[steamId];
            if (recordings.Count >= _maxSessionRecordingCount)
            {
                var toRemove = recordings
                    .OrderBy(r => r.RecordingStart)
                    .Take(recordings.Count - _maxSessionRecordingCount + 1)
                    .ToList();
                foreach (var e in toRemove)
                {
                    recordings.Remove(e);
                    DeleteFilesNoLock(steamId, e.Token);
                }
            }
        }

        private string AllocateTokenLocked(string steamId)
        {
            var recordings = _userIndex[steamId];
            string token;
            do
            {
                token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(18))
                    .Replace('+', '-').Replace('/', '_').TrimEnd('=');
            }
            while (recordings.Any(r => r.Token == token));
            return token;
        }

        private void DeleteFilesNoLock(string steamId, string token)
        {
            var meta = MetaPath(steamId, token);
            if (File.Exists(meta)) File.Delete(meta);
            var data = DataPath(steamId, token);
            if (File.Exists(data)) File.Delete(data);
        }
    }
}
