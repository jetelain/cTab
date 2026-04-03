using System.Threading.Tasks;

#nullable enable

namespace cTabWebApp.Services.Recording
{
    public interface IRecordingSessionService
    {
        void StartRecording(PlayerState state);

        Task StopRecordingAsync(PlayerState state);
    }
}
