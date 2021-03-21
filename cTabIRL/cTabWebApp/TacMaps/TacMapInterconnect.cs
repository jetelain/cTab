using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arma3TacMapLibrary.Hubs;
using Arma3TacMapLibrary.Maps;
using Arma3TacMapLibrary.TacMaps;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace cTabWebApp.TacMaps
{
    public class TacMapInterconnect
    {
        private readonly HubConnection tacMapHub;
        private readonly IHubContext<CTabHub> armaHub;
        private readonly MapId mapId;
        private readonly string armaChannel;

        public TacMapInterconnect(MapId mapId, string tacMapHubUri, IHubContext<CTabHub> armaHub, string armaChannel)
        {
            this.armaHub = armaHub;
            this.mapId = mapId;
            this.armaChannel = armaChannel;

            tacMapHub = new HubConnectionBuilder()
                .WithUrl(tacMapHubUri)
                .WithAutomaticReconnect()
                .Build();

            Init();
        }

        private void Init()
        {
            tacMapHub.On<Marker<MapId>,bool>("AddOrUpdateMarker", (msg, _) =>
                armaHub.Clients.Group(armaChannel).SendAsync("Callback", "AddTacMapMarker", MapExporter.GetMarkerData(msg.id, msg.data)));

            tacMapHub.On<Marker<MapId>,bool>("RemoveMarker", (msg, _) =>
                armaHub.Clients.Group(armaChannel).SendAsync("Callback", "RemoveTacMapMarker", MapExporter.GetMarkerData(msg.id, msg.data)));

            tacMapHub.Reconnected += async info => { await SayHello(); };
        }

        private async Task SayHello()
        {
            await armaHub.Clients.Group(armaChannel).SendAsync("Callback", "ClearTacMapMarkers", "[]");
            await tacMapHub.InvokeAsync("Hello", mapId);
        }

        public async Task Connect()
        {
            await tacMapHub.StartAsync();
            await SayHello();
        }

        public async Task Close()
        {
            await armaHub.Clients.Group(armaChannel).SendAsync("Callback", "ClearTacMapMarkers", "[]");
            await tacMapHub.StopAsync();
        }

        public MapId MapId => mapId;
    }
}
