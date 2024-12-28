using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cTabWebApp.Models
{
    public class MapVM
    {
        public string Token { get; set; }
        public string InitialMap { get; set; } = "altis";

        public bool IsSpectator { get; set; }
        public string SpectatorToken { get; internal set; }

        public string TacMapEndpoint { get; set; }

        public string TacMapDomain { get; set; }

        public string IconBasePath { get; set; } = "https://atlas.plan-ops.fr/data/1/markers/";
    }
}
