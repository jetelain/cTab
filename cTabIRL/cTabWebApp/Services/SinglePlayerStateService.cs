using System.Collections.Generic;

namespace cTabWebApp
{
    public class SinglePlayerStateService : IPlayerStateService
    {
        private readonly PlayerState state = new PlayerState()
        {
            Id = 1,
            Token = "SINGLE"
        };

        public PlayerState GetOrCreateStateBySteamIdAndKey(string steamId, string hashedKey, string keyHostname)
        {
            return state;
        }

        public KeyLoginResult GetTokenBySteamIdAndKey(string steamId, string key)
        {
            return new KeyLoginResult(KeyLoginState.Ok, state.Token);
        }

        public PlayerState GetStateByToken(string token)
        {
            return state;
        }

        public IEnumerable<PlayerState> GetUserAuthenticatedStates(string steamId)
        {
            return new[] { state };
        }

        public PlayerState GetStateBySpectatorToken(string spectatorToken)
        {
            return null;
        }
    }
}
