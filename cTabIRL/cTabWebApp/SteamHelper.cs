using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace cTabWebApp
{
    public class SteamHelper
    {
        public static string GetSteamId(ClaimsPrincipal user)
        {
            if (user?.Identity?.IsAuthenticated ?? false)
            {
                var nameClaim = user.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                if (nameClaim != null && nameClaim.Value.StartsWith("https://steamcommunity.com/openid/id/"))
                {
                    return nameClaim.Value.Substring("https://steamcommunity.com/openid/id/".Length);
                }
            }
            return null;
        }
    }
}
