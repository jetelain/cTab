using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#nullable enable

namespace cTabWebApp.Recording
{
    internal sealed class RecordingCleanupService : BackgroundService
    {
        private static readonly TimeSpan Interval = TimeSpan.FromHours(1);

        private readonly IRecordingService _recordingService;
        private readonly ILogger<RecordingCleanupService> _logger;

        public RecordingCleanupService(IRecordingService recordingService, ILogger<RecordingCleanupService> logger)
        {
            _recordingService = recordingService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(Interval);
            do
            {
                try
                {
                    await _recordingService.CleanUpAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during recording cleanup.");
                }
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));
        }
    }
}
