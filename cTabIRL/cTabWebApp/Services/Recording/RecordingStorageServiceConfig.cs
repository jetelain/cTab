using System;

#nullable enable

namespace cTabWebApp.Services.Recording
{
    public class RecordingStorageServiceConfig
    {
        /// <summary>Duration to keep stored recordings.</summary>
        public TimeSpan RetentionDuration { get; set; } = TimeSpan.FromDays(5);

        /// <summary>Directory to store recording files. Defaults to a temp sub-directory.</summary>
        public string? StorageLocation { get; set; }

        /// <summary>Maximum number of stored recordings per Steam user.</summary>
        public int MaxSessionRecordingCount { get; set; } = 20;

        /// <summary>Maximum total number of stored recordings across all users.</summary>
        public int MaxTotalRecordingCount { get; set; } = 2000;
    }
}
