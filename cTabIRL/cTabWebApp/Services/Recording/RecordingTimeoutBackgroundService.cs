using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#nullable enable

namespace cTabWebApp.Services.Recording
{
    internal sealed class RecordingTimeoutBackgroundService : BackgroundService
    {
        private static readonly TimeSpan Interval = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan RecordingTimeout = TimeSpan.FromHours(1);

        private readonly IPlayerStateService _playerStateService;
        private readonly IRecordingSessionService _recordingSessionService;
        private readonly ILogger<RecordingTimeoutBackgroundService> _logger;

        public RecordingTimeoutBackgroundService(
            IPlayerStateService playerStateService,
            IRecordingSessionService recordingSessionService,
            ILogger<RecordingTimeoutBackgroundService> logger)
        {
            _playerStateService = playerStateService;
            _recordingSessionService = recordingSessionService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(Interval);
            do
            {
                try
                {
                    await StopTimedOutRecordingsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during recording timeout check.");
                }
            }
            while (await timer.WaitForNextTickAsync(stoppingToken));
        }

        private async Task StopTimedOutRecordingsAsync()
        {
            var cutoff = DateTime.UtcNow - RecordingTimeout;
            foreach (var state in _playerStateService.GetStatesWithActiveRecording())
            {
                if (state.CurrentRecording?.LastEventAt < cutoff)
                {
                    _logger.LogInformation("Stopping timed-out recording for player {PlayerId}.", state.Id);
                    await _recordingSessionService.StopRecordingAsync(state);
                }
            }
        }
    }
}
