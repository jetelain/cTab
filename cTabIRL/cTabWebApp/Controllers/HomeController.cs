using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using cTabWebApp.Models;
using cTabWebApp.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QRCoder;

namespace cTabWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly PublicUriService _publicUri;
        private readonly IPlayerStateService _service;

        public HomeController(PublicUriService publicUri, IPlayerStateService service)
        {
            _publicUri = publicUri;
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string t)
        {
            var state = _service.GetStateByToken(t);
            if (state != null)
            {
                return RedirectToAction(nameof(Map), new { t = state.Token });
            }
            var vm = await CreateHomeVmAsync();
            if (!string.IsNullOrEmpty(t))
            {
                vm.Error = SharedResource.InvalidQrCode;
            }
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnterKey([FromForm]string key)
        {
            var steamId = SteamHelper.GetSteamId(User);
            if (string.IsNullOrEmpty(steamId))
            {
                return Denied();
            }

            await Task.Delay(250);

            var result = _service.GetTokenBySteamIdAndKey(steamId, key);
            if (result.State == KeyLoginState.Ok)
            {
                return RedirectToAction(nameof(Map), new { t = result.Token });
            }
            var vm = await CreateHomeVmAsync();
            vm.Error = result.State == KeyLoginState.UnknownPlayer ? SharedResource.ArmaNotYetConnected : SharedResource.InvalidKey;
            return View(nameof(Index), vm);
        }

        private async Task<HomeVM> CreateHomeVmAsync()
        {
            var vm = new HomeVM();
            vm.PublicUri = _publicUri.GetPublicAdress(Request);
            vm.Providers = await GetExternalProvidersAsync();
            return vm;
        }

        [HttpGet]
        public async Task<IActionResult> Map(string t)
        {
            var state = _service.GetStateByToken(t);
            if (state == null)
            {
                var vm = await CreateHomeVmAsync();
                vm.Error = SharedResource.InvalidQrCode;
                return View(nameof(Index), vm);
            }
            return View(new MapVM()
            {
                Token = state.Token,
                InitialMap = state.LastMission?.WorldName?.ToLowerInvariant() ?? "altis"
            });
        }

        private async Task<AuthenticationScheme[]> GetExternalProvidersAsync()
        {
            var schemes = HttpContext.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();

            return (from scheme in await schemes.GetAllSchemesAsync()
                    where !string.IsNullOrEmpty(scheme.DisplayName)
                    select scheme).ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromForm] string provider, [FromForm] bool isPersistent)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                return BadRequest();
            }
            if (!await IsProviderSupportedAsync(provider))
            {
                return BadRequest();
            }
            return Challenge(new AuthenticationProperties { RedirectUri = "/", IsPersistent = isPersistent }, provider);
        }


        [HttpGet, HttpPost]
        public IActionResult SignOut()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private async Task<bool> IsProviderSupportedAsync(string provider)
        {

            return (from scheme in await GetExternalProvidersAsync()
                    where string.Equals(scheme.Name, provider, StringComparison.OrdinalIgnoreCase)
                    select scheme).Any();
        }

        public IActionResult Denied() => View("Denied");

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
