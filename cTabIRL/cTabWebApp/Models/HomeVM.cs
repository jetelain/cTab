using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace cTabWebApp.Models
{
    public class HomeVM
    {
        public string Error { get; internal set; }
        public string PublicUri { get; internal set; }
        public AuthenticationScheme[] Providers { get; internal set; }
    }
}
