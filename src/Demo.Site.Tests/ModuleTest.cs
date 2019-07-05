using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Data;
using Demo.Data.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Business.Tests
{
    [TestClass]
    public class ModuleTest
    {
        /// <summary>
        ///     Obtient ou définit le contexte de test qui fournit
        ///     des informations sur la série de tests active ainsi que ses fonctionnalités.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ItemDataModel()
        {
            /*  var itemDataModel  = new ItemDataModel<FreeBusinessModule>();
            itemDataModel.ParentId = null;
            itemDataModel.SiteId = "1";
            itemDataModel.PropertyName = "MenuItems";
            itemDataModel.Module = "Free";
           itemDataModel.Index = 100; // TODO

            IList<Element> elements = new List<Element>();
            elements.Add(new Element() { Data = "youhou" });
            itemDataModel.Data = elements;


            var item = new Item();
            item.Index = itemDataModel.Index;
           // item.IsDelete = itemDataModel.IsDelete;
            item.IsTemporary = itemDataModel.IsTemporary;
            item.Json = itemDataModel.Json;
            item.Module = itemDataModel.Module;
            item.ParentId = itemDataModel.ParentId;
            item.PropertyName = itemDataModel.PropertyName;
            item.SiteId = itemDataModel.SiteId;
            item.CreateDate = DateTime.Now;



            var test = GetItemDataModel("Free");
          */
            //   object o = Activator.CreateInstance(makeme);


            // var itemDataModel2 = Activator.CreateInstance(type1) as ItemDataModel;


            // Assert.IsTrue(itemDataModel2.GetType() == itemDataModel.GetType());
        }

        public ItemDataModel GetItemDataModel(string moduleName)
        {
            var assembly =
                AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => a.ManifestModule.Name == "Demo.Business.dll")
                    .FirstOrDefault();

            foreach (var type in assembly.GetTypes())
            {
                if (type.Name == moduleName + "BusinessModel")
                {
                    var d1 = typeof (ItemDataModel).Assembly.GetType("Demo.Data.Model.Web.ItemDataModel`1");
                        // GenericTest was my namespace, add yours
                    Type[] typeArgs = {type};
                    var genericType = d1.MakeGenericType(typeArgs);
                    return Activator.CreateInstance(genericType) as ItemDataModel;
                }
            }

            return null;
        }

     /*   [TestMethod]
        public void ListModule()
        {
            var modules = new BusinessModuleFactory().GetModules();
        }*/

     /*   [TestMethod]
        public async Task GetMaster()
        {
            using (var businessFactory = new BusinessFactory(new DataFactoryMock()))
            {
                var moduleManager = businessFactory.ModuleManager;
                var masterPage = await moduleManager.GetMasterAsync(new CurrentCurrentRequest());
            }
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