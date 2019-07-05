using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Demo.Business;
using Demo.Business.Command.Contact.Message;
using Demo.Business.Command.Contact.Message.SiteMap;
using Demo.Data;
using Demo.Data.Mongo;
using Demo.Email;
using Demo.Geo.Core;
using Demo.Log;
using Demo.Log.Core;
using Demo.Mvc.Core.Api.Attributes;
using Demo.Mvc.Core.Logging;
using Demo.Mvc.Core.Message;
using Demo.Queue;
using Demo.Seo;
using Demo.Stats.Core;
using Demo.User.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;

namespace Demo.Mvc.Core
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        private IHostingEnvironment CurrentEnvironment { get; }
        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info {Title = "API Demo", Version = "v1"});
                c.IncludeXmlComments(
                    //The XML file's name has to be Wac.Api.xml
                    $@"{AppDomain.CurrentDomain.BaseDirectory}\Demo.Mvc.Core.xml");
            });

            services.AddMemoryCache();
            // Add service and create Policy with options
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder =>
                    {
                        IList<string> origins = new List<string>
                        {
                            "https://www.bworld.fr",
                            "https://www.lannexe-bretignolles.fr",
                            "https://www.guillaume-chervet.fr",
                            "https://www.broderieennord.com",
                            "https://www.fasiladanse.info"
                        };
                        builder.WithOrigins(origins.ToArray())
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .SetPreflightMaxAge(TimeSpan.FromSeconds(2520));
                        builder.SetIsOriginAllowedToAllowWildcardSubdomains();
                    });
            });

            services.AddOptions();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver =
                    new CamelCasePropertyNamesContractResolver();
            });
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
            services.ConfigureQueue();
            services.ConfigureDataMongo(Configuration);
            services.AddTransient<ISiteMap, MessageSiteMap>();

            services.Configure<StorageConfig>(Configuration.GetSection("Blob"));
            services.Configure<ApplicationConfig>(Configuration.GetSection("Site"));
            services.Configure<BusinessConfig>(Configuration.GetSection("Business"));

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
            };
        }
             

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
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

            if (!env.IsDevelopment()) app.UseHsts();

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
                app.UseSpaStaticFiles();
            }

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Demo"); });

            /*app.MapWhen(IsSpaRoute, spaApp => {
                // Only configure React dev server if in Development
                UseSpaWithoutIndexHtml(spaApp, ConfigureSpaDefaults);
            });*/
            //app.UseResponseCompression();

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                await next();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    "Sitemap",
                    "sitemap.xml",
                    new {controller = "Home", action = "Index"});
                routes.MapRoute(
                    "Robots",
                    "robots.txt",
                    new {controller = "Home", action = "Index"});

                routes.MapRoute(
                    "BingSiteAuth",
                    "BingSiteAuth.xml",
                    new {controller = "Home", action = "Index"});

                routes.MapRoute(
                    "GoogleAuth",
                    "google*.html",
                    new {controller = "Home", action = "Index"});

                routes.MapRoute(
                    "Account",
                    "Account/{action}",
                    new {controller = "Account", action = "Index"});

                routes.MapRoute(
                    "default",
                    "{controller}/{action=Index}/{id?}");

                if (!env.IsDevelopment())
                {
                    routes.MapRoute(
                        "Default",
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
        
        private static Action<ISpaBuilder> ConfigureSpaDefaults =
            spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                spa.UseReactDevelopmentServer("start");
            };
        
        private static bool IsSpaRoute(HttpContext context)
        {
            var path = context.Request.Path;
            // This should probably be a compiled regex
            return path.StartsWithSegments("/static")
                   || path.StartsWithSegments("/sockjs-node")
                   || path.StartsWithSegments("/socket.io")
                   || path.ToString().Contains(".hot-update.");
        }

        private static void UseSpaWithoutIndexHtml(IApplicationBuilder app, Action<ISpaBuilder> configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            // Use the options configured in DI (or blank if none was configured). We have to clone it
            // otherwise if you have multiple UseSpa calls, their configurations would interfere with one another.
            var optionsProvider = app.ApplicationServices.GetService<IOptions<SpaOptions>>();
            var options = new SpaOptions();

            var spaBuilder = new DefaultSpaBuilder(app, options);
            configuration.Invoke(spaBuilder);
        }
        
        private class DefaultSpaBuilder : ISpaBuilder
        {
            public IApplicationBuilder ApplicationBuilder { get; }

            public SpaOptions Options { get; }

            public DefaultSpaBuilder(IApplicationBuilder applicationBuilder, SpaOptions options)
            {
               // spa.Options.SourcePath = "ClientApp";
                ApplicationBuilder = applicationBuilder
                                     ?? throw new ArgumentNullException(nameof(applicationBuilder));

               /* if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }*/
               Options = options ?? throw new ArgumentNullException(nameof(options));
        }
    }
        
        
    }
}