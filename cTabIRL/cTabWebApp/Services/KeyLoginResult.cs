namespace cTabWebApp
{
    public class KeyLoginResult
    {
        public KeyLoginResult(KeyLoginState state, string token)
        {
            this.State = state;
            this.Token = token;
        }

        public KeyLoginState State { get; }

        public string Token { get; }
    }
}