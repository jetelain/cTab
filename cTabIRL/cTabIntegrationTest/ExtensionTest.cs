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
                Console.WriteLine($"{name}:{function}:{data}");
                return 0;
            });

            // Act
            ExtensionDispatch.RvExtensionArgs("Warmup", new string[] { });
            ExtensionDispatch.RvExtensionArgs("Debug", new string[] { "true" });
            ExtensionDispatch.RvExtensionArgs("Connect", new string[] { _webapp.BaseUrl + "/hub", "76561234567890124", "cTabIntegrationTest", "123456" });
            ExtensionDispatch.RvExtensionArgs("StartMission", new string[] { "\"malden\"", "12800", "[2035,6,24,12,0]", "3.0", "3.0" });
            await Task.Delay(500);
            ExtensionDispatch.RvExtensionArgs("UpdateMarkers", new[] { "[\"g\",\"o11\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\b_inf.paa\",\"\\A3\\ui_f\\data\\map\\markers\\nato\\group_2.paa\",\"SP04\",\"\",[6177.97,9262.54,169.726],0,\"\"]"});
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

    }
}