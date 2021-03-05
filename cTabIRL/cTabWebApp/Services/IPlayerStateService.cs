using System.Collections.Generic;

namespace cTabWebApp
{
    public interface IPlayerStateService
    {
        PlayerState GetStateByToken(string token);

        PlayerState GetOrCreateStateBySteamIdAndKey(string steamId, string hashedKey, string keyHostname);

        KeyLoginResult GetTokenBySteamIdAndKey(string steamId, string key);

        IEnumerable<PlayerState> GetUserAuthenticatedStates(string steamId);

        PlayerState GetStateBySpectatorToken(string spectatorToken);
    }
}