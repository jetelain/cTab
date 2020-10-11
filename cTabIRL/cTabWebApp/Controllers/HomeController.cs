using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using cTabWebApp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using QRCoder;

namespace cTabWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IServerAddressesFeature addresses;

        public HomeController(IServer host)
        {
            addresses = host.Features.Get<IServerAddressesFeature>();
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult HowToConnect()
        {
            return base.View(new HowToConnectVM() { Uri = GetPublicAdress() });
        }

        private string GetPublicAdress()
        {
            return addresses.Addresses.FirstOrDefault(a => !a.Contains("localhost"));
        }

        public ActionResult QrCode()
        {
            var generator = new PayloadGenerator.Url(GetPublicAdress());
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(generator.ToString(), QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(5);
            var bitmapBytes = BitmapToBytes(qrCodeImage); //Convert bitmap into a byte array
            return File(bitmapBytes, "image/png"); //Return as file result
        }

        // This method is for converting bitmap into a byte array
        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
