using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Business.Tests
{
    [TestClass]
    public class CacheProviderTest 
    {

        public void MyClassInitialize()
        {

        }

        [TestMethod]
        public async Task InitCache()
        {
            MyClassInitialize();
            //var cacheProvider = container.Resolve<CacheProvider>();
            //await cacheProvider.InitializeCacheAsync();
        }

       /* [TestMethod]
        public async Task UpdateCacheAsync()
        {
            MyClassInitialize();
            var dataFactoryMongo = new DataFactoryMongo(new StorageConfig(), null);
            var businessFactory = new BusinessModuleFactory(container);
            var _cacheProvider = new CacheProvider(dataFactoryMongo, businessFactory, null);

            await _cacheProvider.UpdateCacheAsync("c27e39ee-7ba9-46f8-aa7c-9e334c72a96c");

        }*/
    }
}