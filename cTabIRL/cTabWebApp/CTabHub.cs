using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace cTabWebApp
{
    public class CTabHub : Hub
    {
        private static MissionMessage lastMission = null;
        private static SetPositionMessage lastSetPosition = null;
        private static UpdateMarkersMessage lastUpdateMarkers = null;

        public async Task WebHello(WebHelloMessage message)
        {
            Console.WriteLine("WebHello");
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
            Console.WriteLine("ArmaHello");
            await Groups.AddToGroupAsync(Context.ConnectionId, "Arma");
        }

        public async Task ArmaStartMission(ArmaMessage message)
        {
            Console.WriteLine("ArmaStartMission " + string.Join(", ", message.Args));
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
            Console.WriteLine("ArmaUpdatePosition " + string.Join(", ",message.Args));

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

        public async void ArmaUpdateMarkers(ArmaMessage message)
        {
            Console.WriteLine("ArmaUpdateMarkers " + string.Join(", ", message.Args));

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

        private string GetMilSymbol(string iconA, string iconB)
        {
            var size = GetUnitSize(iconB);
            switch (iconA)
            {
                case "\\A3\\ui_f\\data\\map\\markers\\nato\\b_armor.paa":
                    return $"10031000{size}1205000000";

                case "\\cTab\\img\\b_mech_inf_wheeled.paa":
                    return $"10031000{size}1211020051";

                case "\\A3\\ui_f\\data\\map\\markers\\nato\\b_air.paa":
                    switch(iconB)
                    {
                        case "\\cTab\\img\\icon_air_contact_ca.paa":
                            return $"10031000{size}1206000000";
                    }
                    break;

                case "\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa":
                    return $"10031000{size}1211000000";
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
    }
}
