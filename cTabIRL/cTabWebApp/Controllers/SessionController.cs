using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using cTabWebApp.Models;
using cTabWebApp.Services.Recording;
using Microsoft.AspNetCore.Mvc;

namespace cTabWebApp.Controllers
{
    public class SessionController : Controller
    {
        private readonly IPlayerStateService _service;
        private readonly IRecordingStorageService _recordingService;
        private readonly IRecordingSessionService _recordingSessionService;

        private static readonly JsonSerializerOptions _downloadOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public SessionController(IPlayerStateService service, IRecordingStorageService recordingService, IRecordingSessionService recordingSessionService)
        {
            _service = service;
            _recordingService = recordingService;
            _recordingSessionService = recordingSessionService;
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
        [ValidateAntiForgeryToken]
        public IActionResult Start(string t)
        {
            var state = _service.GetStateByToken(t);
            if (state == null)
            {
                return NotFound();
            }
            _recordingSessionService.StartRecording(state);
            return Ok(new { isRecording = state.CurrentRecording != null });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Stop(string t)
        {
            var state = _service.GetStateByToken(t);
            if (state == null)
            {
                return NotFound();
            }
            await _recordingSessionService.StopRecordingAsync(state);
            return Ok(new { isRecording = false, hasRecording = state.LastRecording != null });
        }

        [HttpGet]
        public async Task<IActionResult> Download(string t)
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

            var filename = $"ctab-session-{recording.RecordingStart:yyyyMMdd-HHmmss}.json";
            Response.ContentType = "application/json";
            Response.Headers.ContentEncoding = "gzip";
            Response.Headers.ContentDisposition = $"attachment; filename=\"{filename}\"";
            await using var gzip = new GZipStream(Response.Body, CompressionLevel.Optimal, leaveOpen: true);
            await JsonSerializer.SerializeAsync(gzip, recording, _downloadOptions);
            return new EmptyResult();
        }

        [HttpGet]
        public async Task<IActionResult> History(string t)
        {
            var state = _service.GetStateByToken(t);
            if (state == null || string.IsNullOrEmpty(state.SteamId))
            {
                return Ok(Array.Empty<object>());
            }
            var recordings = await _recordingService.GetByUserAsync(state.SteamId);
            return Ok(recordings.Select(r => new
            {
                id = r.Token,
                worldName = r.WorldName,
                recordingStart = r.RecordingStart,
                recordingEnd = r.RecordingEnd
            }));
        }

        [HttpGet]
        public async Task<IActionResult> Replay()
        {
            var steamId = SteamHelper.GetSteamId(User);
            var recordings = string.IsNullOrEmpty(steamId)
                ? (IReadOnlyList<StoredRecording>)[]
                : (await _recordingService.GetByUserAsync(steamId));
            return View(new ReplayVM { Recordings = recordings });
        }

        [HttpGet]
        public async Task<IActionResult> DownloadStored(string id)
        {
            var steamId = SteamHelper.GetSteamId(User);
            if (string.IsNullOrEmpty(steamId))
            {
                return NotFound();
            }
            var stored = (await _recordingService.GetByUserAsync(steamId)).FirstOrDefault(r => r.Token == id);
            if (stored == null)
            {
                return NotFound();
            }
            var stream = _recordingService.OpenRawRecording(stored);
            if (stream == null)
            {
                return NotFound();
            }
            Response.Headers.ContentEncoding = "gzip";
            var filename = $"ctab-session-{stored.RecordingStart:yyyyMMdd-HHmmss}.json";
            return File(stream, "application/json", filename);
        }
    }
}
