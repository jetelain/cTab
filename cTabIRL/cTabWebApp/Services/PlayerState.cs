using System;
using System.Collections.Generic;
using Arma3TacMapLibrary.Maps;
using cTabWebApp.Messages;
using cTabWebApp.Services;
using cTabWebApp.TacMaps;

namespace cTabWebApp
{
    public class PlayerState
    {
        public int Id { get; internal set; }
        internal MissionMessage LastMission { get; set; }
        internal SetPositionMessage LastSetPosition { get; set; }
        internal UpdateMarkersMessage LastUpdateMarkers { get; set; }
        internal DevicesMessage LastDevices { get; set; }
        public string Token { get; internal set; }
        public string HashedKey { get; internal set; }
        public string KeyHostname { get; internal set; }
        public string SteamId { get; internal set; }
        public string ArmaChannelName { get { return "Arma-" + Id; } }
        public string WebChannelName { get { return "WebUI-" + Id; } }
        public DateTime LastActivityUtc { get; internal set; }

        public MapId SyncedTacMapId { get; internal set; }

        public TacMapInterconnect Interconnect { get; internal set; }

        internal int ActiveArmaConnections;
        internal int ActiveWebConnections;

        public int ActiveConnections 
        { 
            get { return ActiveArmaConnections + ActiveWebConnections; } 
        }

        internal UpdateMessagesMessage LastUpdateMessages { get; set; }
        public bool IsAuthenticated { get; internal set; }
        public string SpectatorToken { get; internal set; }
        public UpdateMapMarkersMessage LastUpdateMapMarkers { get; internal set; }
        public UpdateMessageTemplatesMessage LastUpdateMessagesTemplates { get; set; }
        public string UploadToken { get; internal set; }
        public List<PlayerTakenImage> Images { get; set; } = new List<PlayerTakenImage>();
        public UpdateSideFeedMessage LastUpdateSideFeedMessage { get; internal set; }
    }
}
