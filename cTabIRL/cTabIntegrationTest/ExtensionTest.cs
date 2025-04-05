using cTabExtension;
using cTabWebApp;

namespace cTabIntegrationTest
{
    [Collection("cTabWebApp")]
    public class ExtensionTest
    {
        private readonly WebApp _webapp;

        public ExtensionTest(WebApp webApp)
        {
            _webapp = webApp;
        }

        [Fact]
        public async Task StartMission()
        {
            // Arrange
            var callbacks = new List<string>();
            ExtensionDispatch.RVExtensionRegisterCallback((name, function, data) =>
            {
                callbacks.Add($"{name}:{function}:{data}");
                Console.WriteLine($"{name}:{function}:{data}");
                return 0;
            });

            // Act
            ExtensionDispatch.RvExtensionArgs("Warmup", new string[] { });
            ExtensionDispatch.RvExtensionArgs("Debug", new string[] { "true" });
            ExtensionDispatch.RvExtensionArgs("Connect", new string[] { _webapp.BaseUrl + "/hub", "76561234567890123", "cTabIntegrationTest", "123456" });
            ExtensionDispatch.RvExtensionArgs("StartMission", new string[] { "\"malden\"", "12800", "[2035,6,24,12,0]", "3.0", "3.0" });
            await Task.Delay(500);

            // Assert
            // Extension must have callbacked
            var connected = callbacks.FirstOrDefault(c => c.StartsWith("ctab:Connected:"));
            Assert.NotNull(connected);
            Assert.Contains("'http://localhost:5000/'", connected);

            // Session must have been created
            var token = _webapp.Get<IPlayerStateService>().GetTokenBySteamIdAndKey("76561234567890123", "123456");
            Assert.Equal(KeyLoginState.Ok, token.State);

            // Mission information must be set
            var state = _webapp.Get<IPlayerStateService>().GetStateByToken(token.Token);
            Assert.NotNull(state.LastMission);
            Assert.Equal("malden", state.LastMission.WorldName);
            Assert.Equal(12800, state.LastMission.Size);
            Assert.Equal(new DateTime(2035, 6, 24, 12, 0, 0, DateTimeKind.Utc), state.LastMission.Date);
        }

        [Fact]
        public async Task UpdatePosition()
        {
            // Arrange
            var callbacks = new List<string>();
            ExtensionDispatch.RVExtensionRegisterCallback((name, function, data) =>
            {
                callbacks.Add($"{name}:{function}:{data}");
                Console.WriteLine($"{name}:{function}:{data}");
                return 0;
            });

            // Act
            ExtensionDispatch.RvExtensionArgs("Warmup", new string[] { });
            ExtensionDispatch.RvExtensionArgs("Debug", new string[] { "true" });
            ExtensionDispatch.RvExtensionArgs("Connect", new string[] { _webapp.BaseUrl + "/hub", "76561234567890124", "cTabIntegrationTest", "123456" });
            ExtensionDispatch.RvExtensionArgs("StartMission", new string[] { "\"malden\"", "12800", "[2035,6,24,12,0]", "3.0", "3.0" });
            await Task.Delay(500);
            ExtensionDispatch.RvExtensionArgs("UpdateMarkers", new[] { "[\"g\",\"o11\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP04\",\"\",[6177.97,9262.54,169.726],0,\"\"]" });
            ExtensionDispatch.RvExtensionArgs("UpdatePosition", new string[] { "6093", "9256.31", "171.876", "351.846", "[2035,6,24,12,1]", "\"o11\"" });
            await Task.Delay(100);

            // Assert
            // Session must have been created
            var token = _webapp.Get<IPlayerStateService>().GetTokenBySteamIdAndKey("76561234567890124", "123456");
            var state = _webapp.Get<IPlayerStateService>().GetStateByToken(token.Token);
            Assert.NotNull(state.LastSetPosition);
            Assert.Equal(new DateTime(2035, 6, 24, 12, 1, 0, DateTimeKind.Utc), state.LastSetPosition.Date);
            Assert.Equal(6093, state.LastSetPosition.X);
            Assert.Equal(9256.31, state.LastSetPosition.Y);
            Assert.Equal(171.876, state.LastSetPosition.Altitude);
            Assert.Equal(351.846, state.LastSetPosition.Heading);
            Assert.Equal("o11", state.LastSetPosition.Group);
        }

        [Fact]
        public async Task EndMission()
        {
            // Arrange
            var callbacks = new List<string>();
            ExtensionDispatch.RVExtensionRegisterCallback((name, function, data) =>
            {
                callbacks.Add($"{name}:{function}:{data}");
                Console.WriteLine($"{name}:{function}:{data}");
                return 0;
            });

            // Act
            ExtensionDispatch.RvExtensionArgs("Warmup", new string[] { });
            ExtensionDispatch.RvExtensionArgs("Debug", new string[] { "true" });
            ExtensionDispatch.RvExtensionArgs("Connect", new string[] { _webapp.BaseUrl + "/hub", "76561234567890125", "cTabIntegrationTest", "123456" });
            ExtensionDispatch.RvExtensionArgs("StartMission", new string[] { "\"malden\"", "12800", "[2035,6,24,12,0]", "3.0", "3.0" });
            await Task.Delay(500);
            ExtensionDispatch.RvExtensionArgs("EndMission", new string[] { });
            await Task.Delay(100);

            // Assert
            // Session must have been created
            var token = _webapp.Get<IPlayerStateService>().GetTokenBySteamIdAndKey("76561234567890125", "123456");
            var state = _webapp.Get<IPlayerStateService>().GetStateByToken(token.Token);
            Assert.Null(state.LastMission);
        }

        [Fact]
        public async Task UpdateMarkers()
        {
            // Arrange
            var callbacks = new List<string>();
            ExtensionDispatch.RVExtensionRegisterCallback((name, function, data) =>
            {
                callbacks.Add($"{name}:{function}:{data}");
                Console.WriteLine($"{name}:{function}:{data}");
                return 0;
            });

            // Act
            ExtensionDispatch.RvExtensionArgs("Warmup", new string[] { });
            ExtensionDispatch.RvExtensionArgs("Debug", new string[] { "true" });
            ExtensionDispatch.RvExtensionArgs("Connect", new string[] { _webapp.BaseUrl + "/hub", "76561234567890126", "cTabIntegrationTest", "123456" });
            ExtensionDispatch.RvExtensionArgs("StartMission", new string[] { "\"malden\"", "12800", "[2035,6,24,12,0]", "3.0", "3.0" });
            await Task.Delay(500);
            ExtensionDispatch.RvExtensionArgs("UpdateMarkers", new[] { "[\"g\",\"o11\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP04\",\"\",[6177.97,9262.54,169.726],0,\"\"]" });
            await Task.Delay(100);

            // Assert
            // Session must have been created
            var token = _webapp.Get<IPlayerStateService>().GetTokenBySteamIdAndKey("76561234567890126", "123456");
            var state = _webapp.Get<IPlayerStateService>().GetStateByToken(token.Token);
            Assert.NotNull(state.LastUpdateMarkers);
            var marker = Assert.Single(state.LastUpdateMarkers.Makers);
            Assert.Equal("o11", marker.Id);
            Assert.Equal("SP04", marker.Name);
            Assert.Equal(6177.97, marker.X);
            Assert.Equal(9262.54, marker.Y);
            Assert.Equal(0, marker.Heading);
            Assert.Equal("10031000131211000000", marker.Symbol);
        }


        [Fact]
        public async Task UpdateMessages()
        {
            // Arrange
            var callbacks = new List<string>();
            ExtensionDispatch.RVExtensionRegisterCallback((name, function, data) =>
            {
                callbacks.Add($"{name}:{function}:{data}");
                Console.WriteLine($"{name}:{function}:{data}");
                return 0;
            });

            // Act
            ExtensionDispatch.RvExtensionArgs("Warmup", new string[] { });
            ExtensionDispatch.RvExtensionArgs("Debug", new string[] { "true" });
            ExtensionDispatch.RvExtensionArgs("Connect", new string[] { _webapp.BaseUrl + "/hub", "76561234567890127", "cTabIntegrationTest", "123456" });
            ExtensionDispatch.RvExtensionArgs("StartMission", new string[] { "\"malden\"", "12800", "[2035,6,24,12,0]", "3.0", "3.0" });
            await Task.Delay(500);
            ExtensionDispatch.RvExtensionArgs("UpdateMessages", new[] { "[\"title\",\"content\",0,\"m1\"]" });
            await Task.Delay(100);

            // Assert
            // Session must have been created
            var token = _webapp.Get<IPlayerStateService>().GetTokenBySteamIdAndKey("76561234567890127", "123456");
            var state = _webapp.Get<IPlayerStateService>().GetStateByToken(token.Token);
            Assert.NotNull(state.LastUpdateMessages);
            var msg = Assert.Single(state.LastUpdateMessages.Messages);
            Assert.Equal("title", msg.Title);
            Assert.Equal("content", msg.Body);
            Assert.Equal("m1", msg.Id);
        }

        [Fact]
        public async Task UpdateMarkersPosition()
        {
            // Arrange
            var callbacks = new List<string>();
            ExtensionDispatch.RVExtensionRegisterCallback((name, function, data) =>
            {
                callbacks.Add($"{name}:{function}:{data}");
                Console.WriteLine($"{name}:{function}:{data}");
                return 0;
            });

            // Act
            ExtensionDispatch.RvExtensionArgs("Warmup", new string[] { });
            ExtensionDispatch.RvExtensionArgs("Debug", new string[] { "true" });
            ExtensionDispatch.RvExtensionArgs("Connect", new string[] { _webapp.BaseUrl + "/hub", "76561234567890128", "cTabIntegrationTest", "123456" });
            ExtensionDispatch.RvExtensionArgs("StartMission", new string[] { "\"malden\"", "12800", "[2035,6,24,12,0]", "3.0", "3.0" });
            await Task.Delay(500);
            ExtensionDispatch.RvExtensionArgs("UpdateMarkers", new[] { "[\"g\",\"o11\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP04\",\"\",[6177.97,9262.54,169.726],0,\"\"]" });
            await Task.Delay(50);
            ExtensionDispatch.RvExtensionArgs("UpdateMarkersPosition", new[] { "[\"o11\",[6170,9260],120]" });
            await Task.Delay(100);

            // Assert
            // Session must have been created
            var token = _webapp.Get<IPlayerStateService>().GetTokenBySteamIdAndKey("76561234567890128", "123456");
            var state = _webapp.Get<IPlayerStateService>().GetStateByToken(token.Token);
            Assert.NotNull(state.LastUpdateMarkers);
            var marker = Assert.Single(state.LastUpdateMarkers.Makers);
            Assert.Equal("o11", marker.Id);
            Assert.Equal(6170, marker.X);
            Assert.Equal(9260, marker.Y);
            Assert.Equal(120, marker.Heading);
        }

        [Fact]
        public async Task Devices()
        {
            // Arrange
            var callbacks = new List<string>();
            ExtensionDispatch.RVExtensionRegisterCallback((name, function, data) =>
            {
                callbacks.Add($"{name}:{function}:{data}");
                Console.WriteLine($"{name}:{function}:{data}");
                return 0;
            });

            // Act
            ExtensionDispatch.RvExtensionArgs("Warmup", new string[] { });
            ExtensionDispatch.RvExtensionArgs("Debug", new string[] { "true" });
            ExtensionDispatch.RvExtensionArgs("Connect", new string[] { _webapp.BaseUrl + "/hub", "76561234567890129", "cTabIntegrationTest", "123456" });
            ExtensionDispatch.RvExtensionArgs("StartMission", new string[] { "\"malden\"", "12800", "[2035,6,24,12,0]", "3.0", "3.0" });
            await Task.Delay(500);
            ExtensionDispatch.RvExtensionArgs("Devices", new[] { "1", "true", "0" });
            await Task.Delay(100);

            // Assert
            // Session must have been created
            var token = _webapp.Get<IPlayerStateService>().GetTokenBySteamIdAndKey("76561234567890129", "123456");
            var state = _webapp.Get<IPlayerStateService>().GetStateByToken(token.Token);
            Assert.NotNull(state.LastDevices);
            Assert.Equal(1, state.LastDevices.Level);
            Assert.True(state.LastDevices.UseMils);
            Assert.Equal(0, state.LastDevices.VehicleMode);
        }

        [Fact]
        public async Task UpdateSideFeed()
        {
            // Arrange
            var callbacks = new List<string>();
            ExtensionDispatch.RVExtensionRegisterCallback((name, function, data) =>
            {
                callbacks.Add($"{name}:{function}:{data}");
                Console.WriteLine($"{name}:{function}:{data}");
                return 0;
            });

            // Act
            ExtensionDispatch.RvExtensionArgs("Warmup", new string[] { });
            ExtensionDispatch.RvExtensionArgs("Debug", new string[] { "true" });
            ExtensionDispatch.RvExtensionArgs("Connect", new string[] { _webapp.BaseUrl + "/hub", "76561234567890131", "cTabIntegrationTest", "123456" });
            ExtensionDispatch.RvExtensionArgs("StartMission", new string[] { "\"malden\"", "12800", "[2035,6,24,12,0]", "3.0", "3.0" });
            await Task.Delay(500);
            ExtensionDispatch.RvExtensionArgs("UpdateSideFeed", new[] { "[1,1,[6313.14,705.207,5.73917],[2035,5,28,12,4],[\"http://localhost:5000/Image/0123456789x0123456789ABCDEF\",false,296.729,[22.2426,3990.51],[[2330.75,-47.6208,5],[4546.83,4352.99,5],[6318.69,705.095,5],[6316.54,700.817,5]],[6320.12,701.694,6.70369]]]" });
            await Task.Delay(100);

            // Assert
            // Session must have been created
            var token = _webapp.Get<IPlayerStateService>().GetTokenBySteamIdAndKey("76561234567890131", "123456");
            var state = _webapp.Get<IPlayerStateService>().GetStateByToken(token.Token);
            Assert.NotNull(state.LastUpdateSideFeedMessage);
            var entry = Assert.Single(state.LastUpdateSideFeedMessage.Entries);
            Assert.Equal("1", entry.Id);
            Assert.Equal([6313.14, 705.207, 5.73917], entry.Location);
            Assert.Equal(new DateTime(2035, 5, 28, 12, 4, 0, DateTimeKind.Utc), entry.DateTime);
            Assert.Equal("http://localhost:5000/Image/0123456789x0123456789ABCDEF", entry.ImageUri);
            Assert.False(entry.ImageProject);
        }

        [Fact]
        public async Task UpdateMessageTemplates()
        {
            // Arrange
            var callbacks = new List<string>();
            ExtensionDispatch.RVExtensionRegisterCallback((name, function, data) =>
            {
                callbacks.Add($"{name}:{function}:{data}");
                Console.WriteLine($"{name}:{function}:{data}");
                return 0;
            });

            // Act
            ExtensionDispatch.RvExtensionArgs("Warmup", new string[] { });
            ExtensionDispatch.RvExtensionArgs("Debug", new string[] { "true" });
            ExtensionDispatch.RvExtensionArgs("Connect", new string[] { _webapp.BaseUrl + "/hub", "76561234567890132", "cTabIntegrationTest", "123456" });
            ExtensionDispatch.RvExtensionArgs("StartMission", new string[] { "\"malden\"", "12800", "[2035,6,24,12,0]", "3.0", "3.0" });
            await Task.Delay(500);
            ExtensionDispatch.RvExtensionArgs("UpdateMessageTemplates", new[] { "[\"builtin#2\",1,\"MEDEVAC\",\"MEDEVAC\",\"\",[[\"MEDEVAC\",\"\",[]],[\"Line 1\",\"LOCATION\",[[\"\",\"Grid of pickup zone\",5]]],[\"Line 2\",\"CALL SIGN & FREQ\",[[\"\",\"Call sign\",3],[\"\",\"Frequency\",4]]],[\"Line 3\",\"NUMBER OF PATIENTS/PRECEDENCE\",[[\"A\",\"URGENT Hospital under 90 min\",1],[\"B\",\"PRIORITY Hospital under 4 hours\",1],[\"C\",\"ROUTINE Hospital within 24 hours\",1]]],[\"Line 4\",\"SPECIAL EQUIPMENT REQUIRED\",[[\"A\",\"None\",6],[\"B\",\"Hoist (Winch)\",6],[\"C\",\"Extrication\",6],[\"D\",\"Ventilator\",6],[\"E\",\"Others\",0]]],[\"Line 5\",\"NUMBER TO BE CARRIED LYING/SITTING\",[[\"L\",\"Litter (Stretcher)\",1],[\"A\",\"Ambulatory (Walking)\",1],[\"E\",\"Escorts (e.g. for child patient)\",1]]],[\"Line 6\",\"SECURITY AT PICKUP ZONE (PZ)\",[[\"N\",\"No enemy\",6],[\"P\",\"Possible enemy\",6],[\"E\",\"Enemy in area\",6],[\"X\",\"Hot PZ - Armed escort required\",6]]],[\"Line 7\",\"PICKUP ZONE (PZ) MARKING METHOD\",[[\"A\",\"Panels\",6],[\"B\",\"Pyro\",6],[\"C\",\"Smoke\",6],[\"D\",\"None\",6],[\"E\",\"Other\",0]]],[\"Line 8\",\"NATIONALITY/STATUS\",[[\"A\",\"Military\",1],[\"D\",\"Civilian\",1],[\"E\",\"PW / Detainee\",1],[\"F\",\"Child\",1]]],[\"Line 9\",\"PICKUP ZONE (PZ) TERRAIN/OBSTACLES\",[[\"\",\"Terrain / obstacles\",0]]]]]" });
            await Task.Delay(100);

            // Assert
            // Session must have been created
            var token = _webapp.Get<IPlayerStateService>().GetTokenBySteamIdAndKey("76561234567890132", "123456");
            var state = _webapp.Get<IPlayerStateService>().GetStateByToken(token.Token);
            Assert.NotNull(state.LastUpdateMessagesTemplates);
            var template = Assert.Single(state.LastUpdateMessagesTemplates.Templates);
            Assert.Equal("builtin#2", template.Uid);
        }
    }
}