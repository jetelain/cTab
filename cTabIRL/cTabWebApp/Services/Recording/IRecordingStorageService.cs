using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


#nullable enable

namespace cTabWebApp.Services.Recording
{
    public interface IRecordingStorageService
    {
        Task<StoredRecording?> SaveAsync(string steamId, SessionRecording recording);
        IReadOnlyList<StoredRecording> GetByUser(string steamId);
        Stream? OpenRecording(StoredRecording stored);
        Task CleanUpAsync();
    }
}
