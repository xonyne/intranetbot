/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PersonalIntranetBot.Extensions;
using PersonalIntranetBot.Helpers;
using Microsoft.EntityFrameworkCore;
using PersonalIntranetBot.Models;
using PersonalIntranetBot.Services;

namespace PersonalIntranetBot
{

    public class Startup
    {
        private static string _connStr = @"
            Server=127.0.0.1,1401;
            Database=personalintranetbot;
            User Id=SA;
            Password=1StrongPassword!
        ";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public const string ObjectIdentifierType = "http://schemas.microsoft.com/identity/claims/objectidentifier";
        public const string TenantIdType = "http://schemas.microsoft.com/identity/claims/tenantid";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddAzureAd(options => Configuration.Bind("AzureAd", options))
            .AddCookie();

            services.AddMvc();
            /* Mac */
            /*var connection = _connStr;*/
            /* Windows */
            var connection = @"Server=localhost\SQLEXPRESS;Database=personalintranetbot;Trusted_Connection=True;ConnectRetryCount=0";
            services.AddDbContext<DBModelContext>
                (options => options.UseSqlServer(connection));

            // This sample uses an in-memory cache for tokens and subscriptions. Production apps will typically use some method of persistent storage.
            services.AddMemoryCache();
            services.AddSession();

            // Add application services.
            //services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IGraphAuthProvider, GraphAuthProvider>();
            services.AddTransient<IGraphSdkHelper, GraphSdkHelper>();
            services.AddTransient<IBingWebSearchService, BingWebSearchService>();
            services.AddTransient<IGoogleMapsService, GoogleMapsService>();
            services.AddTransient<IPersonalIntranetBotService, PersonalIntranetBotService>();
            services.AddTransient<ISocialLinkService, SocialLinksService    >();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
