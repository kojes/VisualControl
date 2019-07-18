using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using VueExample.Color;
using VueExample.Contexts;
using VueExample.Helpers;
using VueExample.Hubs;
using VueExample.Providers;
using VueExample.Providers.Srv6.Interfaces;
using VueExample.Services;

namespace VueExample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAutoMapper();
            services.AddLazyCache();
            services.AddSignalR();
            services.AddOptions();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ApplicationContext")));
                
            

            services.AddScoped<IUserProvider, UserProvider>();
            services.AddTransient<IWaferMapProvider, WaferMapProvider>();
            services.AddTransient<IDieProvider, DieProvider>();
            services.AddTransient<IMeasurementProvider, SimpleMeasurementProvider>();
            services.AddTransient<IGraphicProvider, BasicGraphicProvider>();
            services.AddTransient<IDefectProvider, DefectProvider>();
            services.AddTransient<IPhotoProvider, PhotoProvider>();
            services.AddTransient<IChartJSProvider, ChartJSProvider>();
            services.AddTransient<IDieValueService, DieValueService>();
            services.AddTransient<IColorService, ColorService>();
            services.AddTransient<IDeviceProvider, DeviceProvider>();
            services.AddTransient<IMeasurementSetProvider, MeasurementSetProvider>();
            services.AddTransient<IAtomicMeasurementProvider, AtomicMeasurementProvider>();
            services.AddTransient<IMaterialProvider, MaterialProvider>();
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseStatusCodePages();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSignalR(options =>
            {
                options.MapHub<LivePointHub>("/livepoint");
            });


            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    "spa-fallback",
                    new {controller = "Home", action = "Index"});
            });
        }
    }
}
