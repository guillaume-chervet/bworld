using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Business.Routing;
using Demo.Data;
using Demo.Routing.Models;
using Demo.Routing.Tests.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Demo.Business.Tests;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace Demo.Routing.Tests
{
    /// <summary>
    ///     Description résumée pour UnitTest1
    /// </summary>
    [TestClass]
    public class RoutingUnitTest
    {
        /// <summary>
        ///     Obtient ou définit le contexte de test qui fournit
        ///     des informations sur la série de tests active ainsi que ses fonctionnalités.
        /// </summary>
        public TestContext TestContext { get; set; }

        /*[TestMethod]
        public async void TestMethodFindRouteSitePrincipal()
        {
            var routeManager = new RouteManager(new RouteProviderMock(), new CacheRepositoryMock(), new Logger<RouteManager>());

            var input = new FindRouteInput();
            input.Url = "http://www.demo.fr/Accueil";
            input.IsSecure = false;

            var result = await routeManager.FindRouteAsync(input);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async void TestMethodFindRoute()
        {
            var mockLogger = new Mock<ILogger<RouteManager>>();
            var routeManager = new RouteManager(new RouteProviderMock(), new CacheRepositoryMock());

            var input = new FindRouteInput();
            input.Url = "http://site.demo.fr/MonSiteDemo/Accueil";
            input.IsSecure = false;

            var result = await routeManager.FindRouteAsync(input);

            Assert.IsTrue(result.IsSuccess);
        }*/

        [TestMethod]
        public async Task TestMethodFindRouteShouldReturn301()
        {
        /*    var routeManager = new RouteManager(new RouteProvider(new DataFactoryMongo(new LocalDataConfig(), new MongoBlob(new DataConfig()))), new CacheRepositoryMock());

            var input = new FindRouteInput();
            input.Url = "fasila-danse.bworld.fr";
            input.FullUrl = "https://fasila-danse.bworld.fr";
            input.Port = "443";
            input.IsSecure = true;

            var result = await routeManager.FindRouteAsync(input);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(!string.IsNullOrEmpty(result.RedirectionUrl));*/
        }

        [TestMethod]
        public async Task TestMethodFindRouteShouldCancel301()
        {
        /*    var routeManager = new RouteManager(new RouteProvider(new DataFactoryMongo(new LocalDataConfig(), new MongoBlob(new DataConfig()))), new CacheRepositoryMock());

            var input = new FindRouteInput();
            input.Url = "fasiladanse.info/google74a387435fc78efd.html";
            input.FullUrl = "http://fasiladanse.info/google74a387435fc78efd.html";
            input.Port = "80";
            input.IsSecure = false;

            var result = await routeManager.FindRouteAsync(input);

            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(string.IsNullOrEmpty(result.RedirectionUrl));*/
        }

      /*  [TestMethod]
        public async void TestMethodFindRootRoute()
        {
            var routeManager = new RouteManager(new RouteProviderMock(), new CacheRepositoryMock());

            var input = new FindRouteInput();
            input.Url = "site.demo.fr";
            input.IsSecure = false;

            var result = await routeManager.FindRouteAsync(input);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async void TestMethodFindPath()
        {
            var routeManager = new RouteManager(new RouteProviderMock(), new CacheRepositoryMock());

            var input = new FindPathInput();
            input.DomainDatas = new Dictionary<string, string>();
            input.DomainDatas.Add("site", "MonSiteDemo");
            //input.DomainDatas.Add("domain", "site.demo.fr");

            input.Datas = new Dictionary<string, string>();
            input.Datas.Add("action", "Index");
            input.Datas.Add("controller", "Home");

            input.DomainId = "1";

            input.IsSecure = false;

            var result = await routeManager.FindDomainPathAsync(input);

            Assert.IsTrue(result.IsSuccess);
        }*/

        #region Attributs de tests supplémentaires

        //
        // Vous pouvez utiliser les attributs supplémentaires suivants lorsque vous écrivez vos tests :
        //
        // Utilisez ClassInitialize pour exécuter du code avant d'exécuter le premier test de la classe
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Utilisez ClassCleanup pour exécuter du code une fois que tous les tests d'une classe ont été exécutés
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Utilisez TestInitialize pour exécuter du code avant d'exécuter chaque test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Utilisez TestCleanup pour exécuter du code après que chaque test a été exécuté
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion
    }
}