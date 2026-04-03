using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;


#nullable enable

namespace cTabWebApp.Services.Recording
{
    public class NoRecordingStorageService : IRecordingStorageService
    {
        public Task<StoredRecording?> SaveAsync(string steamId, SessionRecording recording)
            => Task.FromResult<StoredRecording?>(null);

        public Task<IReadOnlyList<StoredRecording>> GetByUserAsync(string steamId) => Task.FromResult<IReadOnlyList<StoredRecording>>([]);

        public Stream? OpenRecording(StoredRecording stored) => null;

        public Task CleanUpAsync() => Task.CompletedTask;
    }
}
