using cTabWebApp;
using cTabWebApp.Services.Recording;

namespace cTabWebAppTest.Recording
{
    public class RecordingSessionServiceTest
    {
        private static RecordingSessionService CreateService(IRecordingStorageService? storage = null)
            => new RecordingSessionService(storage ?? new NoRecordingStorageService());

        // ── StartRecording ────────────────────────────────────────────────────

        [Fact]
        public void StartRecording_CreatesNewRecording_WhenNoneActive()
        {
            var service = CreateService();
            var state = new PlayerState();

            service.StartRecording(state);

            Assert.NotNull(state.CurrentRecording);
        }

        [Fact]
        public void StartRecording_IsNoOp_WhenAlreadyRecording()
        {
            var service = CreateService();
            var state = new PlayerState();
            service.StartRecording(state);
            var original = state.CurrentRecording;

            service.StartRecording(state);

            Assert.Same(original, state.CurrentRecording);
        }

        [Fact]
        public void StartRecording_NoEvents_WhenStateHasNoHistory()
        {
            var service = CreateService();
            var state = new PlayerState();

            service.StartRecording(state);

            Assert.Empty(state.CurrentRecording!.Events);
        }

        [Fact]
        public void StartRecording_AppendsMissionEvent_WhenLastMissionSet()
        {
            var service = CreateService();
            var state = new PlayerState { LastMission = new MissionMessage { WorldName = "altis", Size = 20480 } };

            service.StartRecording(state);

            Assert.Contains(state.CurrentRecording!.Events, e => e.Type == "Mission");
        }

        [Fact]
        public void StartRecording_AppendsAllAvailableStateEvents()
        {
            var service = CreateService();
            var state = new PlayerState
            {
                LastMission = new MissionMessage { WorldName = "stratis" },
                LastUpdateMarkers = new UpdateMarkersMessage(),
                LastUpdateMapMarkers = new UpdateMapMarkersMessage(),
                LastSetPosition = new SetPositionMessage { X = 1.0, Y = 2.0 }
            };

            service.StartRecording(state);

            var types = state.CurrentRecording!.Events.Select(e => e.Type).ToList();
            Assert.Contains("Mission", types);
            Assert.Contains("UpdateMarkers", types);
            Assert.Contains("UpdateMapMarkers", types);
            Assert.Contains("SetPosition", types);
        }

        // ── StopRecordingAsync ────────────────────────────────────────────────

        [Fact]
        public async Task StopRecordingAsync_IsNoOp_WhenNoCurrentRecording()
        {
            var storage = new CapturingStorageService();
            var service = CreateService(storage);
            var state = new PlayerState();

            await service.StopRecordingAsync(state);

            Assert.Null(state.LastRecording);
            Assert.Empty(storage.Saved);
        }

        [Fact]
        public async Task StopRecordingAsync_SetsLastRecordingAndClearsCurrentRecording()
        {
            var service = CreateService();
            var state = new PlayerState();
            service.StartRecording(state);

            await service.StopRecordingAsync(state);

            Assert.Null(state.CurrentRecording);
            Assert.NotNull(state.LastRecording);
        }

        [Fact]
        public async Task StopRecordingAsync_UsesWorldName_FromMission()
        {
            var service = CreateService();
            var state = new PlayerState { LastMission = new MissionMessage { WorldName = "malden" } };
            service.StartRecording(state);

            await service.StopRecordingAsync(state);

            Assert.Equal("malden", state.LastRecording!.WorldName);
        }

        [Fact]
        public async Task StopRecordingAsync_UsesUnknown_WhenNoMission()
        {
            var service = CreateService();
            var state = new PlayerState();
            service.StartRecording(state);

            await service.StopRecordingAsync(state);

            Assert.Equal("unknown", state.LastRecording!.WorldName);
        }

        [Fact]
        public async Task StopRecordingAsync_SavesRecording_WhenAuthenticated()
        {
            var storage = new CapturingStorageService();
            var service = CreateService(storage);
            var state = new PlayerState { SteamId = "76561234567890000", IsAuthenticated = true };
            service.StartRecording(state);

            await service.StopRecordingAsync(state);

            Assert.Single(storage.Saved);
            Assert.Equal("76561234567890000", storage.Saved[0].steamId);
        }

        [Fact]
        public async Task StopRecordingAsync_DoesNotSave_WhenNotAuthenticated()
        {
            var storage = new CapturingStorageService();
            var service = CreateService(storage);
            var state = new PlayerState { SteamId = "76561234567890000", IsAuthenticated = false };
            service.StartRecording(state);

            await service.StopRecordingAsync(state);

            Assert.Empty(storage.Saved);
        }

        [Fact]
        public async Task StopRecordingAsync_DoesNotSave_WhenSteamIdIsNull()
        {
            var storage = new CapturingStorageService();
            var service = CreateService(storage);
            var state = new PlayerState { IsAuthenticated = true };
            service.StartRecording(state);

            await service.StopRecordingAsync(state);

            Assert.Empty(storage.Saved);
        }

        [Fact]
        public async Task StopRecordingAsync_LastRecording_ContainsEventsFromActiveRecording()
        {
            var service = CreateService();
            var state = new PlayerState { LastMission = new MissionMessage { WorldName = "altis" } };
            service.StartRecording(state);
            state.CurrentRecording!.Append("SetPosition", new object());

            await service.StopRecordingAsync(state);

            Assert.True(state.LastRecording!.Events.Count >= 2);
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private sealed class CapturingStorageService : IRecordingStorageService
        {
            public List<(string steamId, SessionRecording recording)> Saved { get; } = new();

            public Task<StoredRecording?> SaveAsync(string steamId, SessionRecording recording)
            {
                Saved.Add((steamId, recording));
                return Task.FromResult<StoredRecording?>(null);
            }

            public IReadOnlyList<StoredRecording> GetByUser(string steamId) => [];

            public Stream? OpenRecording(StoredRecording stored) => null;

            public Task CleanUpAsync() => Task.CompletedTask;
        }
    }
}
