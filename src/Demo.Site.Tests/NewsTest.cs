using System.Threading.Tasks;
using Demo.Business.Command.News;
using Demo.Business.Command.News.Models;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Message;
using Demo.Data.Stat;
using Demo.Log;
using Demo.User;
using Demo.User.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
/*
namespace Demo.Business.Tests
{
    [TestClass]
    public class NewsTest
    {
        private readonly BusinessFactory Business = new BusinessFactory(new DataFactoryMongo(new DataConfig(), new MongoBlob(new DataConfig())));

        [TestInitialize]
        public void Initialize()
        {
            var container = new UnityContainer();

            container.RegisterType<IDataConfig, DataConfig>(new PerResolveLifetimeManager());
            container.RegisterType<IDataFactory, DataFactoryMongo>(new PerResolveLifetimeManager());
            container.RegisterType<ILogService, LogServiceMongo>(new PerResolveLifetimeManager());
            container.RegisterType<UserService, UserService>(new PerResolveLifetimeManager());
            container.RegisterType<BusinessFactory, BusinessFactory>(new PerResolveLifetimeManager());
            container.RegisterType<IStatService, StatServiceMongo>(new PerResolveLifetimeManager());
            container.RegisterType<IMessageService, MessageServiceMongo>(new PerResolveLifetimeManager());

            // Service locator
            var serviceLocator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }

        [TestMethod]
        public async Task GetNewsCommandTest()
        {
            var moduleId = "000282f8-22bb-4fe4-bc6f-cac9a4050e37";
            var siteId = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c";
            var command = ServiceLocator.Current.GetInstance<GetNewsCommand>();
            var result =
                await
                    Business.InvokeAsync<GetNewsCommand, GetNewsInput, CommandResult<GetNewsResult>>(command, new GetNewsInput
                    {
                        ModuleId = moduleId,
                        SiteId = siteId
                    });
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetNewsCommandNextTest()
        {
            var moduleId = "000282f8-22bb-4fe4-bc6f-cac9a4050e37";
            var siteId = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c";
            var command = ServiceLocator.Current.GetInstance<GetNewsCommand>();
            var result =
                await
                    Business.InvokeAsync<GetNewsCommand, GetNewsInput, CommandResult<GetNewsResult>>(command, new GetNewsInput
                    {
                        ModuleId = moduleId,
                        SiteId = siteId
                    });
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetNewsCommandNextPageTest()
        {
            var moduleId = "000282f8-22bb-4fe4-bc6f-cac9a4050e37";
            var siteId = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c";
            var command = ServiceLocator.Current.GetInstance<GetNewsCommand>();
            var result =
                await
                    Business.InvokeAsync<GetNewsCommand, GetNewsInput, CommandResult<GetNewsResult>>(command, new GetNewsInput
                    {
                        ModuleId = moduleId,
                        SiteId = siteId,
                        FilterIndex = 29
                    });
            Assert.IsTrue(result.IsSuccess);
        }
    }
}*/