using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using cTabWebApp.Services.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace cTabWebApp.Controllers
{
    public class IntelController : Controller
    {
        private const int MaxArchiveSize = 50 * 1024 * 1024;

        private readonly IPlayerStateService _service;
        private readonly IImageService _images;
        private readonly ImageServiceConfig _imageConfig;
        private readonly IHubContext<CTabHub> _hubContext;
        private readonly IImageArchiveService _archiveService;

        public IntelController(IPlayerStateService service, IImageService images, ImageServiceConfig imageConfig, IImageArchiveService archiveService, IHubContext<CTabHub> hubContext)
        {
            _service = service;
            _images = images;
            _imageConfig = imageConfig;
            _hubContext = hubContext;
            _archiveService = archiveService;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(MaxArchiveSize)]
        [RequestFormLimits(MultipartBodyLengthLimit = MaxArchiveSize, ValueLengthLimit = MaxArchiveSize)]
        public async Task<IActionResult> FeedArchive([FromQuery]string t, IFormFile archive)
        {
            if (archive.Length > MaxArchiveSize)
            {
                return BadRequest("Archive exceeds size limit.");
            }

            var state = _service.GetStateByToken(t);
            if (state == null || state.LastUpdateSideFeedMessage == null)
            {
                return NotFound();
            }

            using (var file = archive.OpenReadStream())
            {
                var result = await _archiveService.ImportArchiveAsync(state, file, HttpContext.Connection.RemoteIpAddress);
                if (!string.IsNullOrEmpty(result.Error))
                {
                    return BadRequest(result.Error);
                }
                foreach(var stored in result.Images)
                {
                    var uri = new Uri(new Uri(HttpContext.Request.GetEncodedUrl()), $"/Image/{stored.Token}");
                    await _hubContext.Clients.Group(state.ArmaChannelName).SendAsync("Callback", "AddPhoto", $"['{uri.AbsoluteUri}',{stored.Data}]");
                }
            }

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> FeedArchive(string t)
        {
            var state = _service.GetStateByToken(t);
            if (state == null || state.LastUpdateSideFeedMessage == null)
            {
                return NotFound();
            }

            var memoryStream = new MemoryStream();

            await _archiveService.GenerateArchiveAsync(memoryStream, GetImagesFromFeed(state));

            memoryStream.Position = 0;

            var worldName = state.LastMission?.WorldName ?? "unknown";
            return File(memoryStream, "application/zip", $"ctab-feed-{worldName}-{DateTime.UtcNow:yyyy-MM-dd}.zip");
        }

        private List<PlayerTakenImage> GetImagesFromFeed(PlayerState state)
        {
            var images = new List<PlayerTakenImage>();
            foreach (var img in state.LastUpdateSideFeedMessage.Entries)
            {
                var token = Path.GetFileName(img.ImageUri);
                if (!string.IsNullOrEmpty(token))
                {
                    var stored = _images.GetImage(token);
                    if (stored != null)
                    {
                        images.Add(stored);
                    }
                }
            }
            return images;
        }

        [HttpGet]
        public async Task<IActionResult> PlayerArchive(string t)
        {
            var state = _service.GetStateByToken(t);
            if (state == null)
            {
                return NotFound();
            }

            var memoryStream = new MemoryStream();
            await _archiveService.GenerateArchiveAsync(memoryStream, state.Images);
            memoryStream.Position = 0;

            var worldName = state.LastMission?.WorldName ?? "unknown";
            return File(memoryStream, "application/zip", $"ctab-player-{worldName}-{DateTime.UtcNow:yyyy-MM-dd}.zip");
        }
    }
}
