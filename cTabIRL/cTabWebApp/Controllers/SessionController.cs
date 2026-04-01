using System;
using System.Text.Json;
using cTabWebApp.Recording;
using cTabWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace cTabWebApp.Controllers
{
    [IgnoreAntiforgeryToken]
    public class SessionController : Controller
    {
        private readonly IPlayerStateService _service;

        private static readonly JsonSerializerOptions _downloadOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public SessionController(IPlayerStateService service)
        {
            _service = service;
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
        public IActionResult Stop(string t)
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
            var events = active.Events;
            var recording = new SessionRecording
            {
                WorldName = worldName,
                RecordingStart = active.StartedAt,
                RecordingEnd = DateTime.UtcNow,
                Events = events
            };

            state.LastRecording = recording;
            state.CurrentRecording = null;
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
    }
}
