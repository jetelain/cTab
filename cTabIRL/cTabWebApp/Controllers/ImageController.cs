using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using cTabWebApp.Services;
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
        private const int MaxImageSizeInBytes = 1_048_576; // 1 MiB

        public ImageController(IPlayerStateService service, IImageService images)
        {
            _service = service;
            _images = images;
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
            if (file.Length > MaxImageSizeInBytes)
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

            var stored = await _images.SaveImageAsync(state, mem.ToArray(), data);
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
            if (stored == null || stored.IsExpired)
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

        // https://ctab.plan-ops.fr/Image/1234567890123456xiHI_x1KzSuaQ6if5JaNk0y_aqbQqeDYs

        [HttpGet]
        [Route("Image/{token}.html")]
        public IActionResult GetImageHtml(string token, int w = ImageService.TargetWidth)
        {
            var stored = _images.GetImage(token);
            if (stored == null || stored.IsExpired)
            {
                return NotFound();
            }
            var image = new Uri(new Uri(HttpContext.Request.GetEncodedUrl()), $"/Image/{token}.jpeg").AbsoluteUri;
            var h = w * ImageService.TargetHeight / ImageService.TargetWidth;

            var title = "Some title";

            // Html adapted to Arma 3's RscHTML control
            // See https://community.bistudio.com/wiki/HTML_File_Format
            return Content($@"<html>
<head>
	<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"">
	<title>{HttpUtility.HtmlEncode(title)}</title>
</head>
<body>
<h1 align=""center"">{HttpUtility.HtmlEncode(title)}</h1>
<p align=""center""><img src=""{image}"" width=""{w}"" height=""{h}"" /></p>
</body>
</html>", "text/html");
        }


    }
}
