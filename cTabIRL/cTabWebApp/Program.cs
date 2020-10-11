using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace cTabWebApp
{
    public class Program
    {
        private const string localUri = "http://localhost:5000/";

        public static void Main(string[] args)
        {
            var urls = GetUrls();
            var host = CreateHostBuilder(args, urls).Build();
            host.Start();
            Thread.Sleep(1000);
            Console.WriteLine($"=========================================================");
            Console.WriteLine($" Open your mobile browser on {urls.FirstOrDefault()}");
            Console.WriteLine($"=========================================================");
            if (!Debugger.IsAttached)
            {
                Process.Start(new ProcessStartInfo($"{localUri}Home/HowToConnect") { UseShellExecute = true });
            }
            host.WaitForShutdown();
        }

        private static string[] GetUrls()
        {
            var localEntry = Dns.GetHostEntry("");
            var ipv4 = localEntry.AddressList.Where(e => e.AddressFamily == AddressFamily.InterNetwork && !IPAddress.IsLoopback(e)).ToList();
            var httpPublic = $"http://{ipv4.FirstOrDefault()}:5000/";
            return new[] { httpPublic, localUri };
        }

        public static IHostBuilder CreateHostBuilder(string[] args, string[] urls) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    if (urls != null)
                    {
                        webBuilder.UseUrls(urls);
                    }
                    webBuilder.UseStartup<Startup>();
                });
    }
}
