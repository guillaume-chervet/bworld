using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Business;
using Demo.Business.BusinessModule;
using Demo.Business.Command.Site.Cache;
using Demo.Business.Routing;
using Demo.Data;
using Demo.Data.Mongo;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Demo.Mvc.Core.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            try
            {
                var services = new ServiceCollection();
                var serviceProvider = services.BuildServiceProvider();
                
                var dataConfig = new DataConfig();
                dataConfig.ConnectionString = "mongodb://localhost:27017";
                dataConfig.DatabaseName = "bworld";
                var database = new MongoDatabase(Options.Create(dataConfig));
                var dataFactoryMongo =
                    new DataFactoryMongo(database, new MongoBlob(database), Options.Create(dataConfig));
                var businessFactory = new BusinessModuleFactory(serviceProvider);
                var cacheProvider = new CacheProvider(dataFactoryMongo, businessFactory,
                    new RouteProvider(dataFactoryMongo, businessFactory));
                
                
                var domainDatas = new Dictionary<string, string>();
                domainDatas.Add("domain", "localhost");
                var resetSiteCacheCommand = new ResetSiteCacheCommand(dataFactoryMongo, cacheProvider);
                  resetSiteCacheCommand.Input = new ResetSiteCacheInput()
                  {
                      Site = new CurrentRequest()
                      {
                          SiteId = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c", //"227aefdb-a2b9-4c27-98d9-2f0db43f99ca",
                          DomainDatas = domainDatas
                      }
                  };
                  
                 
                  
                  await cacheProvider.InitializeCacheAsync();

                  //await resetSiteCacheCommand.ExecuteAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            //await resetSiteCacheCommand.ExecuteAsync();
        }
    }
}