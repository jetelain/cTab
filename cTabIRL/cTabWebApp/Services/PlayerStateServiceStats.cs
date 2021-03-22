using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public int TotalSessions { get; internal set; }
    }
}
