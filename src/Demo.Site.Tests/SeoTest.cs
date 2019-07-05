using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Business.Command.Site.Seo;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Message;
using Demo.Data.Stat;
using Demo.Log;
using Demo.User;
using Demo.User.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Business.Tests
{
    //[TestClass]
 /*   public class SeoTest : BusinessTestBase
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
        public async Task GetSeoCommandWithIdentity()
        {
            var userInput = new UserInput<GetSeoInput>
            {
                UserId = "54b8c4278241320a28c8f024",
                Data = new GetSeoInput {SiteId = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c", IsVerifyAuthorisation = true}
            };
            var command = ServiceLocator.Current.GetInstance<GetSeoCommand>();
            var result =
                await
                    Business.InvokeAsync<GetSeoCommand, UserInput<GetSeoInput>, CommandResult<SeoBusinessModel>>(command,
                        userInput);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetSeoCommandWithoutIdentity()
        {
            var userInput = new UserInput<GetSeoInput>
            {
                UserId = "",
                Data = new GetSeoInput {SiteId = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c", IsVerifyAuthorisation = false}
            };
            var command = ServiceLocator.Current.GetInstance<GetSeoCommand>();
            var result =
                await
                    Business.InvokeAsync<GetSeoCommand, UserInput<GetSeoInput>, CommandResult<SeoBusinessModel>>(command,
                        userInput);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task SaveSeoCommand()
        {
            var userInput = new UserInput<SaveSeoInput>
            {
                UserId = "54b8c4278241320a28c8f024",
                Data = new SaveSeoInput
                {
                    SiteId = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c",
                    Seo = new SeoBusinessModel
                    {
                        Metas =
                            new List<SeoValidationMeta> {new SeoValidationMeta {Code = "123", Engine = SeoEngine.Bing}}
                    }
                }
            };
            var command = ServiceLocator.Current.GetInstance<SaveSeoCommand>();
            var result =
                await Business.InvokeAsync<SaveSeoCommand, UserInput<SaveSeoInput>, CommandResult<dynamic>>(command, userInput);

            Assert.IsTrue(result.IsSuccess);
        }
    }*/
}