using System.Threading.Tasks;
using Arma3TacMapLibrary.Maps;
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
            tacMapHub.On<TacMapMarker, bool>("AddOrUpdateMarker", async (msg, _) =>
            {
                foreach(var marker in MapExporter.GetMarkerData(msg.id, msg.data))
                {
                    await armaHub.Clients.Group(armaChannel).SendAsync("Callback", "AddTacMapMarker", marker);
                }
            });

            tacMapHub.On<TacMapMarker, bool>("RemoveMarker", async (msg, _) =>
            {
                foreach (var marker in MapExporter.GetMarkerData(msg.id, msg.data))
                {
                    await armaHub.Clients.Group(armaChannel).SendAsync("Callback", "RemoveTacMapMarker", marker);
                }
            });

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
