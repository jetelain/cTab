using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arma3TacMapLibrary.Arma3;
using Arma3TacMapLibrary.Maps;
using cTabWebApp.Services;
using cTabWebApp.TacMaps;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using QRCoder;

namespace cTabWebApp
{
    public class CTabHub : Hub
    {
        private readonly PublicUriService _publicUri;
        private readonly IPlayerStateService _service;
        private readonly ILogger<CTabHub> _logger;
        private readonly TacMapService _tacMapService;

        public CTabHub(PublicUriService publicUri, IPlayerStateService service, ILogger<CTabHub> logger, TacMapService tacMapService)
        {
            _publicUri = publicUri;
            _service = service;
            _logger = logger;
            _tacMapService = tacMapService;
        }

        public async Task WebHello(WebHelloMessage message)
        {
            var state = _service.GetStateByToken(message.Token);
            if (state == null)
            {
                _logger.LogWarning($"No state for token '{message.Token}'");
                return;
            }

            state.LastActivityUtc = DateTime.UtcNow;

            Context.Items[nameof(PlayerState)] = state;
            Context.Items[nameof(ConnectionKind)] = ConnectionKind.Web;

            Interlocked.Increment(ref state.ActiveWebConnections);

            try
            {
                await WebJoin(state);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "WebJoin failed");
            }
        }

        private async Task WebJoin(PlayerState state)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, state.WebChannelName);

            if (state.LastMission != null)
            {
                await Clients.Caller.SendAsync("Mission", state.LastMission);
            }
            if (state.LastDevices != null)
            {
                await Clients.Caller.SendAsync("Devices", state.LastDevices);
            }
            if (state.LastUpdateMarkers != null)
            {
                await Clients.Caller.SendAsync("UpdateMarkers", state.LastUpdateMarkers);
            }
            if (state.LastSetPosition != null)
            {
                await Clients.Caller.SendAsync("SetPosition", state.LastSetPosition);
            }
            if (state.LastUpdateMessages != null)
            {
                await Clients.Caller.SendAsync("UpdateMessages", state.LastUpdateMessages);
            }
            if (state.LastUpdateMapMarkers != null)
            {
                await Clients.Caller.SendAsync("UpdateMapMarkers", state.LastUpdateMapMarkers);
            }
            if (state.LastUpdateAcoustic != null)
            {
                await Clients.Caller.SendAsync("UpdateAcoustic", state.LastUpdateAcoustic);
            }
            if (state.SyncedTacMapId != null)
            {
                await Clients.Caller.SendAsync("SyncTacMap", new SyncTacMapMessage() { MapId = state.SyncedTacMapId });
            }
        }

        public async Task SpectatorHello(SpectatorHelloMessage message)
        {
            var state = _service.GetStateBySpectatorToken(message.SpectatorToken);
            if (state == null)
            {
                _logger.LogWarning($"No state for token '{message.SpectatorToken}'");
                return;
            }

            Context.Items[nameof(PlayerState)] = state;
            Context.Items[nameof(ConnectionKind)] = ConnectionKind.Spectator;

            try
            {
                await WebJoin(state);
            }
            catch(Exception e)
            {
                _logger.LogWarning(e, "WebJoin failed");
            }
        }

        public async Task ArmaHello(ArmaHelloMessage message)
        {
            var ext = Context.GetHttpContext().Request.Headers["Extension"];
            var ua = Context.GetHttpContext().Request.Headers["User-Agent"];
            Console.WriteLine($"Extension => '{ext}' User-Agent => '{ua}'");
            if (!ua.Any(u => u.Contains("cTabExtension/1.")) && !ext.Any(u => u.Contains("cTabExtension/1.")))
            {
                _logger.LogWarning($"ArmaHello was not sent by Extension, but by '{ua}' / '{ext}'");
                return;
            }

            var state = _service.GetOrCreateStateBySteamIdAndKey(message.SteamId, message.Key, new Uri(Context.GetHttpContext().Request.GetEncodedUrl()).DnsSafeHost);

            Context.Items[nameof(PlayerState)] = state;
            Context.Items[nameof(ConnectionKind)] = ConnectionKind.Arma;

            Interlocked.Increment(ref state.ActiveArmaConnections);

            await Groups.AddToGroupAsync(Context.ConnectionId, state.ArmaChannelName);

            var uri = _publicUri.GetPublicAdress(Context.GetHttpContext());

            var generator = new PayloadGenerator.Url(new Uri(new Uri(uri), "/?t="+ state.Token).AbsoluteUri);
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(generator.ToString(), QRCodeGenerator.ECCLevel.Q);
            var data = $"[['{new QrFontCode(qrCodeData).GetString().Replace("\n", "','")}'],'{uri}']";

            await Clients.Caller.SendAsync("Callback", "Connected", data);

            await _tacMapService.UpdateTacMapInterconnect(state);
        }

        private PlayerState GetState()
        {
            object objState;
            if (Context.Items.TryGetValue(nameof(PlayerState), out objState))
            {
                var state = (PlayerState)objState;
                state.LastActivityUtc = DateTime.UtcNow;
                return state;
            }
            return null;
        }

        private PlayerState GetState(ConnectionKind asked)
        {
            var state = GetState();
            if (state != null)
            {
                var actual = (ConnectionKind)Context.Items[nameof(ConnectionKind)];
                if (actual != asked)
                {
                    _logger.LogWarning($"Bad connection type Expected:{asked}, Actual:{actual}");
                    // TODO: once carefully tested, always return null !
                    if (actual == ConnectionKind.Spectator)
                    {
                        return null;
                    }
                }
            }
            return state;
        }

        public async Task ArmaStartMission(ArmaMessage message)
        {
            var state = GetState(ConnectionKind.Arma);
            if (state == null)
            {
                return;
            }

            var worldName = ArmaSerializer.ParseString(message.Args[0]);
            var size = ArmaSerializer.ParseDouble(message.Args[1]) ?? 0d;
            var date = ArmaSerializer.ParseIntegerArray(message.Args[2]);
            var versionCtab = message.Args.Length > 3 ? ToVersion(ArmaSerializer.ParseString(message.Args[3])) : null;
            var versionIrl = message.Args.Length > 4 ? ToVersion(ArmaSerializer.ParseString(message.Args[4])) : null;

            state.LastMission = new MissionMessage()
            {
                WorldName = worldName,
                Size = size,
                Date = ToDateTime(date),
                Timestamp = message.Timestamp,
                CtabFeatureLevel = GetCtabFeatureLevel(versionCtab),
                IrlFeatureLevel = GetIrlFeatureLevel(versionIrl),
            };

            await Clients.Group(state.WebChannelName).SendAsync("Mission", state.LastMission);
        }

        private static readonly Version CtabLevel1 = new Version(2, 7);

        private static int GetCtabFeatureLevel(Version versionCtab)
        {
            if (versionCtab == null || versionCtab < CtabLevel1)
            {
                return 0;
            }
            return 1;
        }

        private static int GetIrlFeatureLevel(Version versionIrl)
        {
            if (versionIrl == null)
            {
                return 0;
            }
            return 1;
        }

        private Version ToVersion(string v)
        {
            if (!string.IsNullOrEmpty(v) && Version.TryParse(v, out var version))
            {
                return version;
            }
            return null;
        }

        private DateTime ToDateTime(int[] date)
        {
            return new DateTime(date[0], date[1], date[2], date[3], date[4], 0, DateTimeKind.Utc);
        }

        public async Task ArmaUpdatePosition(ArmaMessage message)
        {
            if (message.Timestamp < DateTime.UtcNow.AddMinutes(-1))
            {
                _logger.LogTrace("Too old, skip");
                return; // Too old !
            }

            var state = GetState(ConnectionKind.Arma);
            if (state == null)
            {
                _logger.LogWarning($"No state for ArmaUpdatePosition");
                return;
            }

            var x = ArmaSerializer.ParseDouble(message.Args[0]) ?? 0d;
            var y = ArmaSerializer.ParseDouble(message.Args[1]) ?? 0d;
            var z = ArmaSerializer.ParseDouble(message.Args[2]) ?? 0d;
            var dir = ArmaSerializer.ParseDouble(message.Args[3]) ?? 0d;
            var date = ArmaSerializer.ParseIntegerArray(message.Args[4]);
            var grp = ArmaSerializer.ParseString(message.Args[5]);
            var vehicle = message.Args.Length > 6 ? ArmaSerializer.ParseString(message.Args[6]) : null; 

            var pos = state.LastSetPosition = new SetPositionMessage()
            {
                X = x,
                Y = y,
                Altitude = z,
                Heading = dir,
                Date = ToDateTime(date),
                Timestamp = message.Timestamp,
                Group = grp,
                Vehicle = vehicle
            };

            if (message.Args.Length > 11)
            {
                pos.VhlDir = ArmaSerializer.ParseDoubleArray(message.Args[7]);
                pos.VhlUp = ArmaSerializer.ParseDoubleArray(message.Args[8]);
                pos.VhlVel = ArmaSerializer.ParseDoubleArray(message.Args[9]);
                pos.VhlPos = ArmaSerializer.ParseDoubleArray(message.Args[10]);
                pos.Wind = ArmaSerializer.ParseDoubleArray(message.Args[11]);
            }


            await Clients.Group(state.WebChannelName).SendAsync("SetPosition", state.LastSetPosition);
        }

        public async Task ArmaUpdateMapMarkers(ArmaMessage message)
        {
            var state = GetState(ConnectionKind.Arma);
            if (state == null)
            {
                _logger.LogWarning($"No state for ArmaUpdateMapMarkers");
                return;
            }
            var msg =new UpdateMapMarkersMessage()
            {
                Simples = new List<SimpleMapMarker>(),
                Icons = new List<IconMapMarker>(),
                Polylines = new List<PolylineMapMarker>()
            }; 

            var simple = ArmaSerializer.ParseMixedArray(message.Args[0]);
            foreach(object[] simpleMarker in simple)
            {
                var shape = (string)simpleMarker[3];
                if (string.Equals(shape, "ICON", StringComparison.OrdinalIgnoreCase))
                {
                    Arma3MarkerType type;
                    if (Enum.TryParse((string)simpleMarker[2], true, out type))
                    {
                        msg.Icons.Add(new IconMapMarker()
                        {
                            Name = ShorterName((string)simpleMarker[0]),
                            Pos = ((object[])simpleMarker[1]).Cast<double>().Take(2).Select(p => Math.Round(p, 1)).ToArray(),
                            Icon = ToIcon(type, (string)simpleMarker[7]),
                            Size = ((object[])simpleMarker[4]).Cast<double>().ToArray(),
                            Dir = (double)simpleMarker[5],
                            Label = (string)simpleMarker[8],
                            Alpha = (double)simpleMarker[9]
                        });
                    }
                }
                else
                {
                    msg.Simples.Add(new SimpleMapMarker()
                    {
                        Name = ShorterName((string)simpleMarker[0]),
                        Pos = ((object[])simpleMarker[1]).Cast<double>().Take(2).Select(p => Math.Round(p, 1)).ToArray(),
                        Shape = shape.ToLowerInvariant(),
                        Size = ((object[])simpleMarker[4]).Cast<double>().ToArray(),
                        Dir = (double)simpleMarker[5],
                        Brush = (string)simpleMarker[6],
                        Color = ToHtmlColor((string)simpleMarker[7]),
                        Alpha = (double)simpleMarker[9]
                    });
                }
            }

            var poly = ArmaSerializer.ParseMixedArray(message.Args[1]);
            foreach (object[] polyMarker in poly)
            {
                msg.Polylines.Add(new PolylineMapMarker()
                {
                    Name = ShorterName((string)polyMarker[0]),
                    Points = ((object[])polyMarker[1]).Cast<double>().Select(p => Math.Round(p, 1)).ToArray(),
                    Brush = (string)polyMarker[2],
                    Color = ToHtmlColor((string)polyMarker[3]),
                    Alpha = (double)polyMarker[4]
                });
            }

            state.LastUpdateMapMarkers = msg;
            try
            {
                await Clients.Group(state.WebChannelName).SendAsync("UpdateMapMarkers", state.LastUpdateMapMarkers);
            }
            catch(Exception e)
            {
                _logger.LogWarning(e, "UpdateMapMarkers failed");
            }
        }

        private static string ShorterName(string v)
        {
            // Make names shorter to avoid going beyond SignalR limits
            return v?.Replace("_USER_DEFINED ", "§U");
        }

        private static string ToIcon(Arma3MarkerType marker, string color)
        {
            if (color == "ColorWhite" || marker >= Arma3MarkerType.flag_aaf)
            {
                return $"{marker}.png";
            }
            return $"{color}/{marker}.png";
        }

        private static string ToHtmlColor(string strColor)
        {
            Arma3MarkerColor color;
            if (Enum.TryParse(strColor, true, out color))
            {
                return color.ToHexa();
            }
            return "000";
        }

        public async Task ArmaUpdateMarkers(ArmaMessage message)
        {
            //Console.WriteLine("ArmaUpdateMarkers " + string.Join(", ", message.Args));

            var state = GetState(ConnectionKind.Arma);
            if (state == null)
            {
                _logger.LogWarning($"No state for ArmaUpdateMarkers");
                return;
            }

            var msg = new UpdateMarkersMessage()
            {
                Timestamp = message.Timestamp,
                Makers = new List<Marker>()
            };

            foreach (var entry in message.Args)
            {
                var data = ArmaSerializer.ParseMixedArray(entry);

                if ( data.Length < 8 )
                {
                    _logger.LogWarning($"Bad Marker: '{string.Join("', '", data)}'");
                    continue;
                }

                var kind = (string)data[0];
                var id = (string)data[1];
                var iconA = (string)data[2];
                var iconB = (string)data[3];
                var text = (string)data[4];
                var textDetail = (string)data[5];
                var pos = ((object[])data[6]).Cast<double?>().ToArray();
                var dir = (double)data[7];
                var vehicleOrGroup = data.Length > 8 ? (string)data[8] : null;

                msg.Makers.Add(new Marker()
                {
                    Kind = kind,
                    Id = id,
                    X = pos[0] ?? 0,
                    Y = pos[1] ?? 0,
                    Heading = dir,
                    Symbol = CTabMarkers.GetMilSymbol(iconA, iconB),
                    Name = text,
                    Vehicle = kind != "u" ? vehicleOrGroup : null,
                    Group = kind == "u" ? vehicleOrGroup : null
                });
            }

            msg.Makers.Sort((a, b) => a.Name.CompareTo(b.Name));

            state.LastUpdateMarkers = msg;
            try
            {
                await Clients.Group(state.WebChannelName).SendAsync("UpdateMarkers", state.LastUpdateMarkers);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "UpdateMarkers failed");
            }
        }

        public async Task ArmaUpdateAcoustic(ArmaMessage message)
        {
            //Console.WriteLine("ArmaUpdateMarkers " + string.Join(", ", message.Args));

            var state = GetState(ConnectionKind.Arma);
            if (state == null)
            {
                _logger.LogWarning($"No state for ArmaUpdateAcoustic");
                return;
            }

            var gameTime = ArmaSerializer.ParseDouble(message.Args[0]);
            var shots = ArmaSerializer.ParseMixedArray(message.Args[1]);

            var msg = new UpdateAcousticMessage()
            {
                Timestamp = message.Timestamp,
                GameTime = gameTime ?? 0,
                Shots = new List<DetectedShot>(shots.Length)
            };

            foreach (object[] data in shots)
            {
                var time = (double)data[0];
                var shotid = (int)((double)data[1]);
                var pos = ((object[])data[2]).Cast<double?>().ToArray();
                var radius = (double)data[3];
                var caliber = (double)data[4];
                msg.Shots.Add(new DetectedShot()
                {
                    Time = time,
                    Id = shotid,
                    X = pos[0] ?? 0,
                    Y = pos[1] ?? 0,
                    Radius = radius,
                    Caliber = caliber
                });
            }
            state.LastUpdateAcoustic = msg;
            try
            {
                await Clients.Group(state.WebChannelName).SendAsync("UpdateAcoustic", state.LastUpdateAcoustic);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "UpdateAcoustic failed");
            }
        }

        public async Task ArmaUpdateMarkersPosition(ArmaMessage message)
        {
            var state = GetState(ConnectionKind.Arma);
            if (state == null)
            {
                _logger.LogWarning($"No state for ArmaUpdateMarkersPosition");
                return;
            }

            var msg = new UpdateMarkersMessagePosition()
            {
                Timestamp = message.Timestamp,
                Makers = new List<MarkerPosition>()
            };

            foreach (var entry in message.Args)
            {
                try
                {
                    var data = ArmaSerializer.ParseMixedArray(entry);

                    var id = (string)data[0];
                    var pos = ((object[])data[1]).Cast<double?>().ToArray();
                    var dir = (double)data[2];

                    msg.Makers.Add(new MarkerPosition()
                    {
                        Id = id,
                        X = pos[0] ?? 0,
                        Y = pos[1] ?? 0,
                        Heading = dir
                    });
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, $"Bad MarkersPosition '{entry}'");
                }
            }

            var lastUpdate = state.LastUpdateMarkers;
            if (lastUpdate != null)
            {
                foreach(var marker in lastUpdate.Makers)
                {
                    var updated = msg.Makers.FirstOrDefault(m => m.Id == marker.Id);
                    if (updated != null)
                    {
                        marker.X = updated.X;
                        marker.Y = updated.Y;
                        marker.Heading = updated.Heading;
                    }
                }
            }

            try 
            { 
                await Clients.Group(state.WebChannelName).SendAsync("UpdateMarkersPosition", msg);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "UpdateMarkersPosition failed");
            }
        }


        public async Task ArmaUpdateMessages(ArmaMessage message)
        {
            //Console.WriteLine("ArmaUpdateMarkers " + string.Join(", ", message.Args));

            var state = GetState(ConnectionKind.Arma);
            if (state == null)
            {
                _logger.LogWarning($"No state for ArmaUpdateMarkers");
                return;
            }

            var msg = new UpdateMessagesMessage()
            {
                Timestamp = message.Timestamp,
                Messages = new List<Message>()
            };

            foreach (var entry in message.Args)
            {
                var data = ArmaSerializer.ParseMixedArray(entry);

                var title = (string)data[0];
                var body = (string)data[1];
                var msgState = (int)((double?)data[2]);
                var id = (string)data[3];

                msg.Messages.Add(new Message()
                {
                    Id = id,
                    Title = title,
                    State = msgState,
                    Body = body
                });
            }
            state.LastUpdateMessages = msg;
            try 
            { 
                await Clients.Group(state.WebChannelName).SendAsync("UpdateMessages", state.LastUpdateMessages);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "UpdateMessages failed");
            }
        }

        public void ArmaEndMission(ArmaMessage message)
        {
            var state = GetState(ConnectionKind.Arma);
            if (state == null)
            {
                _logger.LogWarning($"No state for ArmaEndMission");
                return;
            }
            state.LastSetPosition = null;
            state.LastMission = null;
        }

        public async Task ArmaDevices(ArmaMessage message)
        {
            var state = GetState(ConnectionKind.Arma);
            if (state == null)
            {
                _logger.LogWarning($"No state for ArmaDevices");
                return;
            }
            var deviceLevel = int.Parse(message.Args[0], CultureInfo.InvariantCulture);
            var useMils = bool.Parse(message.Args[1]);
            var vehicleMode = message.Args.Length > 2 ? int.Parse(message.Args[2], CultureInfo.InvariantCulture) : 0;
            state.LastDevices = new DevicesMessage()
            {
                Level = deviceLevel,
                UseMils = useMils,
                VehicleMode = vehicleMode
            };
            await Clients.Group(state.WebChannelName).SendAsync("Devices", state.LastDevices);
        }

        public async Task ArmaActionRangeFinder(ArmaMessage message)
        {
            var state = GetState(ConnectionKind.Arma);
            if (state == null)
            {
                _logger.LogWarning($"No state for ArmaActionTelemeter");
                return;
            }
            if (message.Args.Length < 4)
            {
                _logger.LogWarning($"Bad ActionRangeFinder: '{string.Join("', '", message.Args)}'");
                return;
            }
            var telemeter = new RangeFinderMessage()
            {
                X = ArmaSerializer.ParseDouble(message.Args[0]) ?? 0d,
                Y = ArmaSerializer.ParseDouble(message.Args[1]) ?? 0d,
                Z = ArmaSerializer.ParseDouble(message.Args[2]) ?? 0d,
                Distance = ArmaSerializer.ParseDouble(message.Args[3]) ?? 0d
            };
            await Clients.Group(state.WebChannelName).SendAsync("ActionRangeFinder", telemeter);
        }

        public async Task WebAddUserMarker(WebAddUserMarkerMessage message)
        {
            var state = GetState(ConnectionKind.Web);
            if (state == null)
            {
                _logger.LogWarning($"No state for WebAddUserMarker");
                return;
            }
            string data = FormattableString.Invariant($"[[{message.X},{message.Y}],{message.Data[0]},{message.Data[1]},{message.Data[2]}]");
            await Clients.Group(state.ArmaChannelName).SendAsync("Callback", "AddUserMarker", data);
        }
        private static string ToData(IdMessage message)
        {
            return FormattableString.Invariant($"[\"{ArmaSerializer.Escape(message.Id)}\"]");
        }

        public async Task WebSendMessage(WebSendMessageMessage message)
        {
            var state = GetState(ConnectionKind.Web);
            if (state == null)
            {
                _logger.LogWarning($"No state for WebSendMessage");
                return;
            }
            if (string.IsNullOrEmpty(message.To) || string.IsNullOrEmpty(message.Body) || message.Body.Length > 5000 || message.To.Length > 32)
            {
                return;
            }
            string data = FormattableString.Invariant($"[\"{ArmaSerializer.Escape(message.To)}\",\"{ArmaSerializer.Escape(message.Body)}\"]");
            await Clients.Group(state.ArmaChannelName).SendAsync("Callback", "SendMessage", data);
        }

        public async Task WebMessageRead(IdMessage message)
        {
            var state = GetState(ConnectionKind.Web);
            if (state == null)
            {
                _logger.LogWarning($"No state for WebMessageRead");
                return;
            }
            await Clients.Group(state.ArmaChannelName).SendAsync("Callback", "MessageRead", ToData(message));
        }

        public async Task WebDeleteMessage(IdMessage message)
        {
            var state = GetState(ConnectionKind.Web);
            if (state == null)
            {
                _logger.LogWarning($"No state for WebDeleteMessage");
                return;
            }
            await Clients.Group(state.ArmaChannelName).SendAsync("Callback", "DeleteMessage", ToData(message));
        }

        public async Task WebDeleteUserMarker(IdMessage message)
        {
            var state = GetState(ConnectionKind.Web);
            if (state == null)
            {
                _logger.LogWarning($"No state for DeleteUserMarker");
                return;
            }
            await Clients.Group(state.ArmaChannelName).SendAsync("Callback", "DeleteUserMarker", ToData(message));
        }

        //DeleteUserMarker
        public async Task WebSyncTacMap(SyncTacMapMessage message)
        {
            var state = GetState(ConnectionKind.Web);
            if (state == null)
            {
                _logger.LogWarning($"No state for WebSyncTacMap");
                return;
            }
            if (message.MapId == null || message.MapId.TacMapID == 0 || string.IsNullOrEmpty(message.MapId.ReadToken))
            {
                state.SyncedTacMapId = null;
            }
            else
            {
                state.SyncedTacMapId = new MapId() 
                { 
                    IsReadOnly = true, 
                    ReadToken = message.MapId.ReadToken, 
                    TacMapID = message.MapId.TacMapID 
                };
            }
            await Clients.Group(state.WebChannelName).SendAsync("SyncTacMap", new SyncTacMapMessage() { MapId = state.SyncedTacMapId });

            await _tacMapService.UpdateTacMapInterconnect(state);
        }

        public async Task WebTicAlert(TicAlertMessage message)
        {
            var state = GetState(ConnectionKind.Web);
            if (state == null)
            {
                _logger.LogWarning($"No state for WebTicAlert");
                return;
            }
            await Clients.Group(state.ArmaChannelName).SendAsync("Callback", "TicAlert", message.State ? "[true]" : "[false]");
        }

        public async override Task OnDisconnectedAsync(Exception exception)
        {
            var state = GetState();
            if (state != null)
            {
                var actual = (ConnectionKind)Context.Items[nameof(ConnectionKind)];
                if (actual == ConnectionKind.Arma)
                {
                    Interlocked.Decrement(ref state.ActiveArmaConnections);
                    await _tacMapService.UpdateTacMapInterconnect(state);
                }
                else if (actual == ConnectionKind.Web)
                {
                    Interlocked.Decrement(ref state.ActiveWebConnections);
                }
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
