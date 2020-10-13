using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Primitives;

namespace cTabWebApp
{
    public class CTabHub : Hub
    {
        private static MissionMessage lastMission = null;
        private static SetPositionMessage lastSetPosition = null;
        private static UpdateMarkersMessage lastUpdateMarkers = null;

        public async Task WebHello(WebHelloMessage message)
        {
            //Console.WriteLine("WebHello");
            await Groups.AddToGroupAsync(Context.ConnectionId, "WebUI");
            if (lastMission != null)
            {
                await Clients.Caller.SendAsync("Mission", lastMission);
            }
            if (lastSetPosition != null)
            {
                await Clients.Caller.SendAsync("SetPosition", lastSetPosition);
            }
            if (lastUpdateMarkers != null)
            {
                await Clients.Caller.SendAsync("UpdateMarkers", lastUpdateMarkers);
            }
        }

        public async Task ArmaHello(ArmaHelloMessage message)
        {
            //Console.WriteLine("ArmaHello");
            await Groups.AddToGroupAsync(Context.ConnectionId, "Arma");
        }

        public async Task ArmaStartMission(ArmaMessage message)
        {
            //Console.WriteLine("ArmaStartMission " + string.Join(", ", message.Args));
            var worldName = JsonSerializer.Deserialize<string>(message.Args[0]);
            var size = double.Parse(message.Args[1], CultureInfo.InvariantCulture);
            var date = JsonSerializer.Deserialize<int[]>(message.Args[2]);

            lastMission = new MissionMessage()
            {
                WorldName = worldName,
                Size = size,
                Date = ToDateTime(date),
                Timestamp = message.Timestamp
            };

            await Clients.Group("WebUI").SendAsync("Mission", lastMission);
        }

        private DateTime ToDateTime(int[] date)
        {
            return new DateTime(date[0], date[1], date[2], date[3], date[4], 0, DateTimeKind.Utc);
        }

        public async Task ArmaUpdatePosition(ArmaMessage message)
        {
            //Console.WriteLine("ArmaUpdatePosition " + string.Join(", ",message.Args));

            if (message.Timestamp < DateTime.UtcNow.AddMinutes(-1))
            {
                Console.WriteLine("  Too old, skip");
                return; // Too old !
            }

            var x = double.Parse(message.Args[0], CultureInfo.InvariantCulture);
            var y = double.Parse(message.Args[1], CultureInfo.InvariantCulture);
            var z = double.Parse(message.Args[2], CultureInfo.InvariantCulture);
            var dir = double.Parse(message.Args[3], CultureInfo.InvariantCulture);
            var date = JsonSerializer.Deserialize<int[]>(message.Args[4]);
            var grp = JsonSerializer.Deserialize<string>(message.Args[5]);
            var vehicle = message.Args.Length > 6 ? JsonSerializer.Deserialize<string>(message.Args[6]) : null;

            lastSetPosition = new SetPositionMessage()
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

            await Clients.Group("WebUI").SendAsync("SetPosition", lastSetPosition);
        }

        public async Task ArmaUpdateMarkers(ArmaMessage message)
        {
            //Console.WriteLine("ArmaUpdateMarkers " + string.Join(", ", message.Args));

            var msg = new UpdateMarkersMessage()
            {
                Timestamp = message.Timestamp,
                Makers = new List<Marker>()
            };

            foreach (var entry in message.Args)
            {
                var data = JsonSerializer.Deserialize<JsonElement[]>(entry.Replace("\\","\\\\"));

                var kind = data[0].GetString();
                var id = data[1].GetString();
                var iconA = data[2].GetString();
                var iconB = data[3].GetString();
                var text = data[4].GetString();
                var textDetail = data[5].GetString();
                var pos = data[6].EnumerateArray().Select(a => a.GetDouble()).ToArray();
                var dir = data[7].GetDouble();
                var vehicle = data.Length > 8 ? data[8].GetString() : null;

                msg.Makers.Add(new Marker()
                {
                    Kind = kind,
                    Id = id,
                    X = pos[0],
                    Y = pos[1],
                    Heading = dir,
                    Symbol = GetMilSymbol(iconA, iconB),
                    Name = text,
                    Vehicle = vehicle
                });
            }

            lastUpdateMarkers = msg;
            await Clients.Group("WebUI").SendAsync("UpdateMarkers", lastUpdateMarkers);
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

        public void ArmaEndMission(ArmaMessage message)
        {
            Console.WriteLine("ArmaEndMission " + string.Join(", ", message.Args));
            lastSetPosition = null;
            lastMission = null;
        }

        public void ArmaDevices(ArmaMessage message)
        {
            Console.WriteLine("ArmaDevices " + string.Join(", ", message.Args));
        }

        public async Task WebAddUserMarker(WebAddUserMarkerMessage message)
        {
            string data = FormattableString.Invariant($"[[{message.X},{message.Y}],{message.Data[0]},{message.Data[1]},{message.Data[2]}]");
            await Clients.Group("Arma").SendAsync("Callback", "AddUserMarker", data);
        }
    }
}
