using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


#nullable enable

namespace cTabWebApp.Services.Recording
{
    public interface IRecordingStorageService
    {
        Task<StoredRecording?> SaveAsync(string steamId, SessionRecording recording);
        Task<IReadOnlyList<StoredRecording>> GetByUserAsync(string steamId);
        Stream? OpenRecording(StoredRecording stored);
        Task CleanUpAsync();
    }
}
