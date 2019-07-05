using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Business;
using Demo.Business.BusinessModule;
using Demo.Business.Command.Contact.Message.SiteMap;
using Demo.Business.Command.Site;
using Demo.Business.Routing;
using Demo.Data;
using Demo.Data.Mongo;
using Demo.Data.Repository;
using Demo.Log;
using Demo.Mvc.Core.Message;
using Demo.Routing;
using Demo.Routing.Interfaces;
using Demo.Site.Helper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo.Seo.Console
{
    public class Program
    {
        private static bool _isRunning = true;

        private static void Main(string[] args)
        {
            
            var task = Run();
            task.Wait();
        }

        public static void Stop()
        {
            _isRunning = false;
        }

        public static async Task Run()
        {
            //Logger<>.Default.Info("Démarage batch seo : " + DateTime.Now);
            _isRunning = true;
            while (_isRunning)
            {
                await DoTask();
                Thread.Sleep(60000*10);
            }
        }

        public static async Task DoTask()
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            IConfiguration Configuration =  new ConfigurationBuilder()
                .AddEnvironmentVariables()
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
                .Build();
            
            var services = new ServiceCollection();

            //Runner is the custom class

            services.AddTransient<ILoggerFactory, LoggerFactory>();
            services.AddTransient(typeof(ILogger<>), typeof(Logger<>));
            services.AddLogging((builder) => builder.SetMinimumLevel(LogLevel.Trace));
            
            services.ConfigureSiteCore(Configuration);
            services.ConfigureRouting();
            services.ConfigureSeo();
            services.ConfigureDataMongo(Configuration);
            services.AddTransient<ISiteMap, MessageSiteMap>();
            services.AddTransient<Crawler, Crawler>();

            services.Configure<StorageConfig>(Configuration.GetSection("Blob"));
            var serviceProvider = services.BuildServiceProvider();
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

        
            try
            {
                using (IDataFactory dataFactory = serviceProvider.GetService<IDataFactory>() )
                {
                    var cacheItems = await dataFactory.CacheRepository.ListAsync("Seo");
                    var siteRepository = dataFactory.ItemRepository;
                    var crawler = serviceProvider.GetService<Crawler>();

                    foreach (var cacheItem in cacheItems)
                    {
                        try
                        {
                            var itemDataModel = await siteRepository.GetItemAsync(null, cacheItem.SiteId);
                            var routeManager = serviceProvider.GetService<IRouteManager>();
                            var sitemap = await SiteMap.SitemapUrlAsync(itemDataModel, dataFactory, routeManager);

                            var inputSite = new InputSite();
                            inputSite.BaseUrl = sitemap.BaseUrl;
                            inputSite.SiteId = itemDataModel.Id;

                            await crawler.CrawlSiteAsync(inputSite, new[] {sitemap.Url});

                            await dataFactory.CacheRepository.RemoveAsync(cacheItem.Id);
                        }
                        catch (Exception ex)
                        {
                           // Logger<>.Default.Error(ex,
                            System.Console.WriteLine(string.Concat("Console seo site exception for siteId: ", cacheItem.SiteId, " - exception: ", ex.ToString()));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine( "Console seo générale exception");
            }
            //Logger<>.Default.Info("Dernière execution : " + DateTime.Now);
            System.Console.WriteLine("Dernière execution : " + DateTime.Now);
        }
    }
}