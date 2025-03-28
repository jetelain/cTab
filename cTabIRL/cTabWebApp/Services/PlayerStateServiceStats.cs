namespace cTabWebApp
{
    public class PlayerStateServiceStats
    {
        public int ActiveArmaConnections { get; internal set; }
        public int ActiveWebConnections { get; internal set; }
        public int ActiveSessions { get; internal set; }
        public int ActiveSessionsWithSteam { get; internal set; }
        public int ActiveTacMapConnections { get; internal set; }
        public int ActiveSessionsWithTacMap { get; internal set; }
        public int ActiveSessionsWithIntel { get; internal set; }
        public int ActiveSessionsWithPhotos { get; internal set; }
        public int TotalSessions { get; internal set; }
    }
}
