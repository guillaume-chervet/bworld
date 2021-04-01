using System;
using System.IO;
using System.Linq;
using Demo.Business.Command.Contact.Message.SiteMap;
using Demo.Data;
using Demo.Log;
using Demo.Log.Core;
using Demo.Mvc.Core.Data;
using Demo.Mvc.Core.Email;
using Demo.Mvc.Core.Geo.Core;
using Demo.Mvc.Core.Logging;
using Demo.Mvc.Core.Message;
using Demo.Mvc.Core.Routing;
using Demo.Mvc.Core.Sites.Core;
using Demo.Mvc.Core.Sites.Data.Azure;
using Demo.Mvc.Core.Stats;
using Demo.Mvc.Core.Tags;
using Demo.Mvc.Core.UserCore;
using Demo.Seo;
using Demo.User.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace Demo.Mvc.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        private IWebHostEnvironment CurrentEnvironment { get; }
        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
         /*   services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "API Demo", Version = "v1"});
                c.IncludeXmlComments(
                    //The XML file's name has to be Wac.Api.xml
                    $@"{AppDomain.CurrentDomain.BaseDirectory}\Demo.Mvc.Core.xml");
            });
*/
            services.AddMemoryCache();

            var businessSection = Configuration.GetSection("Business");
            var businessConfig = businessSection.Get<BusinessConfig>();
            services.Configure<BusinessConfig>(businessSection);

            var origins = businessConfig.Domains.Where(d => String.IsNullOrEmpty(d.CorsOrigin) == false)
                .Select(d => d.CorsOrigin).ToArray();

            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.WithOrigins(origins)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .SetPreflightMaxAge(TimeSpan.FromSeconds(2520));
                        builder.SetIsOriginAllowedToAllowWildcardSubdomains();
                    });
            });

            services.AddOptions();
            services
                .AddControllersWithViews();
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/build"; });
            services.Configure<MvcOptions>(options => { options.Filters.Add(new RequireHttpsAttribute()); });
            services.ConfigureLogCore();
            services.ConfigureGeoCore();
            services.ConfigureMessageCore();
            services.Configure<EmailConfig>(Configuration.GetSection("Mail"));
            services.ConfigureMail(CurrentEnvironment.IsDevelopment());
            services.ConfigureSeo();
            services.ConfigureStatsCore();
            services.ConfigureUserCore();
            services.ConfigureTagsCore();
            services.ConfigureSiteCore(Configuration);
            services.ConfigureRouting();
           //services.ConfigureQueue();
            services.ConfigureDataMongo(Configuration);
            services.AddTransient<ISiteMap, MessageSiteMap>();

            services.Configure<StorageConfig>(Configuration.GetSection("Blob"));
            services.Configure<ApplicationConfig>(Configuration.GetSection("Site"));
            
            services.AddIdentity<ApplicationUser>(Configuration)
                .AddDefaultTokenProviders();

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
            });

            if (CurrentEnvironment.IsDevelopment())
            {
                services.AddHttpsRedirection(options => options.HttpsPort = 44301);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var connectionString = Configuration.GetValue<string>("MongoDb:ConnectionString");
            var databaseName = Configuration.GetValue<string>("MongoDb:DatabaseName");
            var logLevel = Configuration.GetValue<string>("LogginggMongoDb:Level");
            LogLevel myStatus;
            Enum.TryParse(logLevel, out myStatus);
            loggerFactory.AddMongoDb(new MongoConfiguration
                {
                    ConnectionString = connectionString,
                    DatabaseName = databaseName,
                    MinLevel = myStatus
                }
                , env.ApplicationName, env.EnvironmentName);

            if (!env.IsDevelopment())
            {
                app.UseHsts();
            }

            app.ExceptionMiddleware();
            app.UseHttpsRedirection();
            if (!env.IsDevelopment())
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.Combine(Directory.GetCurrentDirectory(), "ClientApp", "build")),
                    OnPrepareResponse = ctx =>
                    {
                        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=32000");
                        ctx.Context.Response.Headers.Append("User-Agent", "bworld server");
                    }
                });
            }
            else
            {
                app.UseStaticFiles();
                app.UseSpaStaticFiles();
            }

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
          //  app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Demo"); });

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                await next();
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
                endpoints.MapControllerRoute("Sitemap",
                    "sitemap.xml",
                    new {controller = "Home", action = "Index"});
                endpoints.MapControllerRoute(
                    "Robots",
                    "robots.txt",
                    new {controller = "Home", action = "Index"});
                endpoints.MapControllerRoute("BingSiteAuth",
                    "BingSiteAuth.xml",
                    new {controller = "Home", action = "Index"});
                endpoints.MapControllerRoute("GoogleAuth",
                    "google*.html",
                    new {controller = "Home", action = "Index"});
                endpoints.MapControllerRoute("Account",
                    "Account/{action}",
                    new {controller = "Account", action = "Index"});
                endpoints.MapControllerRoute("default",
                    "{controller}/{action=Index}/{id?}");
                
                if (!env.IsDevelopment())
                {
                    endpoints.MapControllerRoute( "Default",
                        "{*url}",
                        new {controller = "Home", action = "Index"});
                }
            });
      
            if (env.IsDevelopment())
            {
                // Only configure React dev server if in Development
                app.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                    spa.UseReactDevelopmentServer("start");
                });
            }
        }
    }
}