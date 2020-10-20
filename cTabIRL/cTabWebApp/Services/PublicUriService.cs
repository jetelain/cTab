using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace cTabWebApp.Services
{
    public class PublicUriService
    {
        private readonly IServerAddressesFeature addresses;

        public PublicUriService(IServer host)
        {
            addresses = host.Features.Get<IServerAddressesFeature>();
        }
        public string GetPublicAdress(HttpRequest request)
        {
            var uri = addresses.Addresses.FirstOrDefault(a => !a.Contains("localhost") && !a.Contains("127.0.0.1"));
            if (uri == null)
            {
                return new Uri(new Uri(request.GetEncodedUrl()), "/").AbsoluteUri;
            }
            return uri;
        }

        public string GetPublicAdress(HttpContext context)
        {
            return GetPublicAdress(context.Request);
        }
    }
}
