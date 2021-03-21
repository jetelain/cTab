using System;
using System.IO;
using Arma3TacMapLibrary;
using cTabWebApp.Models;
using cTabWebApp.Services;
using cTabWebApp.TacMaps;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace cTabWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR(o =>
            {
                o.MaximumReceiveMessageSize = null;
            });
            services.AddLocalization();
            services.AddControllersWithViews()
                .AddViewLocalization();
#if CLOUD
            services.AddSingleton<IPlayerStateService, PlayerStateService>();
#else
            services.AddSingleton<IPlayerStateService, SinglePlayerStateService>();
#endif
            services.AddSingleton<PublicUriService>();

            var steamKey = Configuration.GetValue<string>("SteamKey");
            if (!string.IsNullOrEmpty(steamKey))
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/";
                    options.LogoutPath = "/Home/SignOut";
                    options.AccessDeniedPath = "/Home/Denied";
                })
                .AddSteam(s => s.ApplicationKey = steamKey);
            }
            var communauty = Configuration.GetSection("Communauty").Get<CommunautyInfos>() ?? new CommunautyInfos();
            services.AddSingleton(communauty);

            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                var dir = Configuration.GetValue<string>("UnixKeysDirectory");
                if (!string.IsNullOrEmpty(dir))
                {
                    services.AddDataProtection()
                        .PersistKeysToFileSystem(new DirectoryInfo(dir))
                        .SetApplicationName("cTabWebApp");
                }
            }

            services.AddSingleton<TacMapService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedProto
                });
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseArma3TacMapStaticFiles();

            if (!string.IsNullOrEmpty(Configuration.GetValue<string>("SteamKey")))
            {
                app.UseAuthentication();
            }

            app.UseCookiePolicy(new CookiePolicyOptions()
            {
                HttpOnly = HttpOnlyPolicy.Always,
                Secure = CookieSecurePolicy.SameAsRequest,
                MinimumSameSitePolicy = SameSiteMode.Lax
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<CTabHub>("/hub");
            });
        }
    }
}
