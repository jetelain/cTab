using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

#nullable enable

namespace cTabWebApp.Recording
{
    public interface IRecordingService
    {
        Task<StoredRecording?> SaveAsync(string steamId, SessionRecording recording);
        IReadOnlyList<StoredRecording> GetByUser(string steamId);
        Stream? OpenRecording(StoredRecording stored);
        Task CleanUpAsync();
    }
}
