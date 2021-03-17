using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace cTabWebApp.TacMaps
{
    public class TacMapService
    {
        private readonly IHubContext<CTabHub> _armaHub;

        public TacMapService(IHubContext<CTabHub> armaHub, IConfiguration configuration)
        {
            _armaHub = armaHub;
            TacMapEndpoint = configuration.GetValue<string>("TacMapEndpoint");
        }

        public string TacMapEndpoint { get; }

        public async Task UpdateTacMapInterconnect(PlayerState state)
        {
            if (string.IsNullOrEmpty(TacMapEndpoint))
            {
                return;
            }

            if (state.Interconnect != null && (state.SyncedTacMapId == null || state.SyncedTacMapId.TacMapID != state.Interconnect.MapId.TacMapID || state.ActiveArmaConnections == 0))
            {
                var oldInterconnect = state.Interconnect;
                state.Interconnect = null;
                await oldInterconnect.Close();
            }

            if (state.SyncedTacMapId != null && state.ActiveArmaConnections > 0)
            {
                var interconnect = new TacMapInterconnect(state.SyncedTacMapId, TacMapEndpoint + "/MapHub", _armaHub, state.ArmaChannelName);
                await interconnect.Connect();
            }
        }
    }
}
