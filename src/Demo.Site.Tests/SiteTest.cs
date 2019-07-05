using System.Threading.Tasks;
using Demo.Business.Command.Site;
using Demo.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Business.Tests
{
    [TestClass]
    public class SiteTest 
    {
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
        }


        [TestMethod]
        public void InitIndexMongo()
        {
            MongoConfig.InitIndex();
        }

        /*  [TestMethod]
        public void TransertSiteMockToMongo()
        {
            using (var business = new BusinessFactory(new DataFactoryMock()))
            {
                // MyWorld
                var data = new DataFactoryMongo(new DataConfig());
                var transfertSiteCommand = new TransfertSiteCommand(new DataFactoryMock(), data, new CacheProvider(data));
                transfertSiteCommand.Input = new TransfertSiteInput() { SiteId = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c" };
                transfertSiteCommand.Execute();

                var result = transfertSiteCommand.Result;

                Assert.IsTrue(result.IsSuccess);

                // Boulangerie-Demo
                transfertSiteCommand.Input = new TransfertSiteInput() { SiteId = "654ff501-b076-4e15-87c1-3d400a8e4f37" };
                transfertSiteCommand.Execute();

                result = transfertSiteCommand.Result;

                Assert.IsTrue(result.IsSuccess);
            }
        }*/

    /*    [TestMethod]
        public async Task TransertSiteMongoToMock()
        {
            {
                // MyWorld
                var data = new DataFactoryMongo(new LocalDataConfig(), new MongoBlob(new LocalDataConfig()));
                var transfertSiteCommand = new TransfertSiteCommand(new DataFactoryMongo(new DataConfig(), new MongoBlob(new DataConfig())), data,
                    new CacheProvider(data));
                transfertSiteCommand.Input = new TransfertSiteInput() { SiteId = "654ff501-b076-4e15-87c1-3d400a8e4f37" };
                await transfertSiteCommand.ExecuteAsync();

                var result = transfertSiteCommand.Result;

                Assert.IsTrue(result.IsSuccess);
            }
        }*/
    }
}