using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Arma3TacMapLibrary.Arma3;
using Arma3TacMapLibrary.TacMaps;
using cTabWebApp.Services;
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

        public CTabHub(PublicUriService publicUri, IPlayerStateService service, ILogger<CTabHub> logger)
        {
            _publicUri = publicUri;
            _service = service;
            _logger = logger;
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
            var ua = Context.GetHttpContext().Request.Headers["User-Agent"];
            if (!ua.Any(u => u.Contains("cTabExtension/1.")))
            {
                _logger.LogWarning($"ArmaHello was not sent by Extension, but by '{string.Join(", ", ua)}'");
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
                Console.WriteLine($"No state for ArmaStartMission");
                return;
            }

            var worldName = ArmaSerializer.ParseString(message.Args[0]);
            var size = ArmaSerializer.ParseDouble(message.Args[1]) ?? 0d;
            var date = ArmaSerializer.ParseIntegerArray(message.Args[2]);

            state.LastMission = new MissionMessage()
            {
                WorldName = worldName,
                Size = size,
                Date = ToDateTime(date),
                Timestamp = message.Timestamp
            };

            await Clients.Group(state.WebChannelName).SendAsync("Mission", state.LastMission);
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

            state.LastSetPosition = new SetPositionMessage()
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

                var kind = (string)data[0];
                var id = (string)data[1];
                var iconA = (string)data[2];
                var iconB = (string)data[3];
                var text = (string)data[4];
                var textDetail = (string)data[5];
                var pos = ((object[])data[6]).Cast<double?>().ToArray();
                var dir = (double)data[7];
                var vehicle = data.Length > 8 ? (string)data[8] : null;

                msg.Makers.Add(new Marker()
                {
                    Kind = kind,
                    Id = id,
                    X = pos[0] ?? 0,
                    Y = pos[1] ?? 0,
                    Heading = dir,
                    Symbol = GetMilSymbol(iconA, iconB),
                    Name = text,
                    Vehicle = vehicle
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

        private string GetUnitSize(string iconB)
        {
            switch (iconB)
            {
                case "\\A3\\ui_f\\data\\map\\markers\\nato\\group_0.paa":
                    return "11";
                case "\\A3\\ui_f\\data\\map\\markers\\nato\\group_1.paa":
                    return "12";
                case "\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa":
                    return "13";
                case "\\A3\\ui_f\\data\\map\\markers\\nato\\group_3.paa":
                    return "14";
            }
            return "00";
        }
        // https://spatialillusions.com/unitgenerator/
        private static Dictionary<string, string> icons = new Dictionary<string, string>()
        {
            // BLUE
            { "\\cTab\\img\\b_mech_inf_wheeled.paa"                  , "10031000001211020051" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_support.paa"  , "10031000001634000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_motor_inf.paa", "10031000001211040000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_uav.paa"      , "10031000001219000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_air.paa"      , "10031000001206000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_plane.paa"    , "10031000001208000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_mech_inf.paa" , "10031000001211020000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_art.paa"      , "10031000001303000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_armor.paa"    , "10031000001205000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_mortar.paa"   , "10031000001308000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_hq.paa"       , "10031002000000000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\military\\end_CA.paa" , "img:mil_end.png" }, // 10032500001508000000 in theory
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa"      , "10031000001211000000" },

            // GREEN
            { "\\A3\\ui_f\\data\\map\\markers\\military\\join_CA.paa"   , "img:mil_join.png" },
            { "\\A3\\ui_f\\data\\map\\markers\\military\\circle_CA.paa" , "img:mil_circle.png" }, // 10032500003205000000 in theory
            { "\\A3\\ui_f\\data\\map\\mapcontrol\\Hospital_CA.paa"      , "10032000001122020000" },
            { "\\A3\\ui_f\\data\\map\\markers\\military\\warning_CA.paa", "img:mil_warning.png" },

            // RED 
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\o_inf.paa"      , "10061000001211000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\o_mech_inf.paa" , "10061000001211020000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\o_motor_inf.paa", "10061000001211040000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\o_armor.paa"    , "10061000001205000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\o_air.paa"      , "10061000001206000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\o_plane.paa"    , "10061000001208000000" },
            { "\\A3\\ui_f\\data\\map\\markers\\nato\\o_unknown.paa"  , "10061000000000000000" },
            { "\\cTab\\img\\o_inf_rifle.paa"                         , "10061500001100000000" }, 
            { "\\cTab\\img\\o_inf_mg.paa"                            , "10062700001103010000" },
            { "\\cTab\\img\\o_inf_at.paa"                            , "10062700001103160000" },
            { "\\cTab\\img\\o_inf_mmg.paa"                           , "10062700001103030000" },
            { "\\cTab\\img\\o_inf_mat.paa"                           , "10062700001103070000" },
            { "\\cTab\\img\\o_inf_mmortar.paa"                       , "10062700001103140000" },
            { "\\cTab\\img\\o_inf_aa.paa"                            , "10061500001111000000" }
        };

        private string GetMilSymbol(string iconA, string iconB)
        {
            var size = GetUnitSize(iconB);
            string symbol;
            if (icons.TryGetValue(iconA, out symbol))
            {
                if (size == "00")
                {
                    return symbol;
                }
                return $"{symbol.Substring(0,8)}{size}{symbol.Substring(10)}";
            }
            return $"10031000{size}0000000000";
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
            Console.WriteLine("ArmaEndMission " + string.Join(", ", message.Args));
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
            Console.WriteLine("ArmaDevices " + string.Join(", ", message.Args));
            var deviceLevel = int.Parse(message.Args[0], CultureInfo.InvariantCulture);
            var useMils = bool.Parse(message.Args[1]);

            state.LastDevices = new DevicesMessage()
            {
                Level = deviceLevel,
                UseMils = useMils
            };

            await Clients.Group(state.WebChannelName).SendAsync("Devices", state.LastDevices);
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
            await Clients.Group(state.WebChannelName).SendAsync("WebSyncTacMap", new SyncTacMapMessage() { MapId = state.SyncedTacMapId });
        }


        public override Task OnDisconnectedAsync(Exception exception)
        {
            var state = GetState();
            if (state != null)
            {
                var actual = (ConnectionKind)Context.Items[nameof(ConnectionKind)];
                if (actual == ConnectionKind.Arma)
                {
                    Interlocked.Decrement(ref state.ActiveArmaConnections);
                }
                else if (actual == ConnectionKind.Web)
                {
                    Interlocked.Decrement(ref state.ActiveWebConnections);
                }
            }
            return base.OnDisconnectedAsync(exception);
        }
    }
}
