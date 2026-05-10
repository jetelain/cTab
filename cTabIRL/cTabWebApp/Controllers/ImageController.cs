using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using cTabWebApp.Services.Images;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

#nullable enable

namespace cTabWebApp.Controllers
{
    public class ImageController : Controller
    {
        private readonly IPlayerStateService _service;
        private readonly IImageService _images;
        private readonly ImageServiceConfig _imageConfig;

        public ImageController(IPlayerStateService service, IImageService images, ImageServiceConfig imageConfig)
        {
            _service = service;
            _images = images;
            _imageConfig = imageConfig;
        }

        [HttpPost]
        [Route("Image")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string token, [FromForm] string data = "")
        {
            var state = _service.GetStateByUploadToken(token);
            if (state == null)
            {
                return Forbid();
            }
            var ext = HttpContext.Request.Headers["Extension"];
            if (!ext.Any(u => u?.Contains("cTabExtension/1.") ?? false))
            {
                return BadRequest();
            }
            if (file.Length > _imageConfig.MaxImageSizeInBytes)
            {
                return BadRequest();
            }
            if (file.ContentType != "image/jpeg")
            {
                return BadRequest();
            }

            var mem = new MemoryStream();
            await file.CopyToAsync(mem);
            mem.Position = 0;

            var stored = await _images.SaveImageAsync(state, mem.ToArray(), data, HttpContext.Connection.RemoteIpAddress);
            if (stored == null)
            {
                return BadRequest();
            }
            var uri = new Uri(new Uri(HttpContext.Request.GetEncodedUrl()), $"/Image/{stored.Token}");
            return Content(uri.AbsoluteUri);
        }

        [HttpGet]
        [Route("Image/{token}")]
        [Route("Image/{token}.jpeg")]
        public IActionResult GetImage(string token)
        {
            var stored = _images.GetImage(token);
            if (stored == null)
            {
                return NotFound();
            }
            var stream = _images.OpenImage(stored);
            if (stream == null)
            {
                return NotFound();
            }
            return File(stream, "image/jpeg");
        }

        [HttpGet]
        [Route("Image/{token}.html")]
        public IActionResult GetImageHtml(string token, int h = 0)
        {
            var stored = _images.GetImage(token);
            if (stored == null)
            {
                return NotFound();
            }
            if (h <= 0)
            {
                h = _imageConfig.MaxHeight;
            }
            var image = new Uri(new Uri(HttpContext.Request.GetEncodedUrl()), $"/Image/{token}.jpeg").AbsoluteUri;
            var w = h * _imageConfig.MaxWidth / _imageConfig.MaxHeight;

            var title = $"{stored.TimestampUtc:yyyy-MM-dd HH:mm:ss} Z - {stored.WorldName}";

            // Html adapted to Arma 3's RscHTML control
            // See https://community.bistudio.com/wiki/HTML_File_Format
            return Content($@"<!DOCTYPE html>
<html lang=""en"">
<head>
	<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
	<title>{HttpUtility.HtmlEncode(title)}</title>
</head>
<body>
<p align=""center""><img src=""{image}"" width=""{w}"" height=""{h}"" /></p>
</body>
</html>", "text/html");
        }

        [HttpGet]
        [Route("DownloadIntelFeed")]
        public IActionResult DownloadIntelFeed(string t)
        {
            return RedirectToAction(nameof(IntelController.FeedArchive), "Intel", new { t });
        }
    }
}
