using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using cTabWebApp.Models;
using cTabWebApp.Recording;
using Microsoft.AspNetCore.Mvc;

namespace cTabWebApp.Controllers
{
    [IgnoreAntiforgeryToken]
    public class SessionController : Controller
    {
        private readonly IPlayerStateService _service;
        private readonly IRecordingService _recordingService;

        private static readonly JsonSerializerOptions _downloadOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public SessionController(IPlayerStateService service, IRecordingService recordingService)
        {
            _service = service;
            _recordingService = recordingService;
        }

        [HttpGet]
        public IActionResult Status(string t)
        {
            var state = _service.GetStateByToken(t);
            if (state == null)
            {
                return NotFound();
            }
            return Ok(new
            {
                isRecording = state.CurrentRecording != null,
                hasRecording = state.LastRecording != null
            });
        }

        [HttpPost]
        public IActionResult Start(string t)
        {
            var state = _service.GetStateByToken(t);
            if (state == null)
            {
                return NotFound();
            }
            if (state.CurrentRecording != null)
            {
                return Ok(new { isRecording = true });
            }

            var recording = new ActiveRecording();

            // Bootstrap current state into recording
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
            return Ok(new { isRecording = true });
        }

        [HttpPost]
        public async Task<IActionResult> Stop(string t)
        {
            var state = _service.GetStateByToken(t);
            if (state == null)
            {
                return NotFound();
            }
            var active = state.CurrentRecording;
            if (active == null)
            {
                return Ok(new { isRecording = false, hasRecording = state.LastRecording != null });
            }

            var worldName = state.LastMission?.WorldName ?? "unknown";
            var recording = new SessionRecording
            {
                WorldName = worldName,
                RecordingStart = active.StartedAt,
                RecordingEnd = DateTime.UtcNow,
                Events = active.Events
            };

            state.LastRecording = recording;
            state.CurrentRecording = null;

            if (!string.IsNullOrEmpty(state.SteamId) && state.IsAuthenticated)
            {
                await _recordingService.SaveAsync(state.SteamId, recording);
            }

            return Ok(new { isRecording = false, hasRecording = true });
        }

        [HttpGet]
        public IActionResult Download(string t)
        {
            var state = _service.GetStateByToken(t);
            if (state == null)
            {
                return NotFound();
            }
            var recording = state.LastRecording;
            if (recording == null)
            {
                return NotFound();
            }

            var bytes = JsonSerializer.SerializeToUtf8Bytes(recording, _downloadOptions);
            var filename = $"ctab-session-{recording.RecordingStart:yyyyMMdd-HHmmss}.json";
            return File(bytes, "application/json", filename);
        }

        [HttpGet]
        public IActionResult History(string t)
        {
            var state = _service.GetStateByToken(t);
            if (state == null || string.IsNullOrEmpty(state.SteamId))
            {
                return Ok(Array.Empty<object>());
            }
            var recordings = _recordingService.GetByUser(state.SteamId);
            return Ok(recordings.Select(r => new
            {
                id = r.Token,
                worldName = r.WorldName,
                recordingStart = r.RecordingStart,
                recordingEnd = r.RecordingEnd
            }));
        }

        [HttpGet]
        public IActionResult Replay()
        {
            var steamId = SteamHelper.GetSteamId(User);
            var recordings = string.IsNullOrEmpty(steamId)
                ? (IReadOnlyList<StoredRecording>)[]
                : _recordingService.GetByUser(steamId);
            return View(new ReplayVM { Recordings = recordings });
        }

        [HttpGet]
        public IActionResult DownloadStored(string id)
        {
            var steamId = SteamHelper.GetSteamId(User);
            if (string.IsNullOrEmpty(steamId))
            {
                return NotFound();
            }
            var stored = _recordingService.GetByUser(steamId).FirstOrDefault(r => r.Token == id);
            if (stored == null)
            {
                return NotFound();
            }
            var stream = _recordingService.OpenRecording(stored);
            if (stream == null)
            {
                return NotFound();
            }
            var filename = $"ctab-session-{stored.RecordingStart:yyyyMMdd-HHmmss}.json";
            return File(stream, "application/json", filename);
        }
    }
}
