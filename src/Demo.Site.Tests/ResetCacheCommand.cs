using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Business.Command.Site.Cache;
using Demo.Business.Routing;
using Demo.Data;
using Demo.Data.Mongo;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Business.Tests
{
    [TestClass]
    public class ResetCacheCommand
    {
         [TestInitialize]
        public async Task RestCache()
        {
            /*var dataConfig = new DataConfig();
            dataConfig.ConnectionString = "mongodb://localhost:27017";
            dataConfig.DatabaseName = "bworld";
            var database = new MongoDatabase(Options.Create(dataConfig));
             var dataFactoryMongo = new DataFactoryMongo(database, new MongoBlob(database), Options.Create(dataConfig));
             var businessFactory = new BusinessModuleFactory(null);
             var resetSiteCacheCommand = new ResetSiteCacheCommand(dataFactoryMongo, new CacheProvider(dataFactoryMongo,businessFactory, new RouteProvider(dataFactoryMongo, businessFactory)  ) );

             var domainDatas = new Dictionary<string, string>();
             domainDatas.Add("domain", "localhost");
             resetSiteCacheCommand.Input = new ResetSiteCacheInput()
             {
                 Site = new CurrentRequest()
                 {
                     SiteId = "227aefdb-a2b9-4c27-98d9-2f0db43f99ca",
                     DomainDatas = domainDatas
                 }
             };
             
             await resetSiteCacheCommand.ExecuteAsync();*/
         }
    }
}