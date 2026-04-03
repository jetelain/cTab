using System;
using System.Linq;
using System.Threading.Tasks;

#nullable enable

namespace cTabWebApp.Services.Recording
{
    internal sealed class RecordingSessionService : IRecordingSessionService
    {
        private readonly IRecordingStorageService _recordingStorage;

        public RecordingSessionService(IRecordingStorageService recordingStorage)
        {
            _recordingStorage = recordingStorage;
        }

        public void StartRecording(PlayerState state)
        {
            if (state.CurrentRecording != null)
            {
                return;
            }

            var recording = new ActiveRecording();

            if (state.LastMission != null)
            {
                recording.Append("Mission", state.LastMission);
            }
            if (state.LastUpdateMarkers != null)
            {
                recording.Append("UpdateMarkers", state.LastUpdateMarkers);
            }
            if (state.LastUpdateMapMarkers != null)
            {
                recording.Append("UpdateMapMarkers", state.LastUpdateMapMarkers);
            }
            if (state.LastSetPosition != null)
            {
                recording.Append("SetPosition", state.LastSetPosition);
            }

            state.CurrentRecording = recording;
        }

        public async Task StopRecordingAsync(PlayerState state)
        {
            var active = state.CurrentRecording;
            if (active == null)
            {
                return;
            }

            var worldName = state.LastMission?.WorldName ?? "unknown";
            var recording = new SessionRecording
            {
                WorldName = worldName,
                RecordingStart = active.StartedAt,
                RecordingEnd = DateTime.UtcNow,
                Events = active.TakeSnapshot()
            };

            state.LastRecording = recording;
            state.CurrentRecording = null;

            if (!string.IsNullOrEmpty(state.SteamId) && state.IsAuthenticated)
            {
                await _recordingStorage.SaveAsync(state.SteamId, recording);
            }
        }
    }
}
