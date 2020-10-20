namespace cTabWebApp
{
    public interface IPlayerStateService
    {
        PlayerState GetStateByToken(string token);

        PlayerState GetOrCreateStateBySteamIdAndKey(string steamId, string hashedKey, string keyHostname);

        KeyLoginResult GetTokenBySteamIdAndKey(string steamId, string key);
    }
}