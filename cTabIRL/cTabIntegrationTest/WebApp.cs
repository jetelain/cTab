using cTabWebApp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace cTabIntegrationTest
{
    public sealed class WebApp : IDisposable, IAsyncDisposable
    {
        private readonly IHost host;

        public WebApp()
        {
            host = Host.CreateDefaultBuilder().ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); }).Start();
        }

        public string BaseUrl => host.Services.GetRequiredService<IServer>().Features.Get<IServerAddressesFeature>()!.Addresses.First();

        public T Get<T>() where T : class
        {
            return host.Services.GetRequiredService<T>();
        }

        public void Dispose()
        {
            host.StopAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await host.StopAsync();
        }
    }
}
