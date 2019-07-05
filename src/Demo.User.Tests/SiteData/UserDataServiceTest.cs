using System;
using System.Threading.Tasks;
using Demo.User.SiteData;
using Demo.User.SiteData.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.User.Tests.SiteData
{
    [TestClass]
    public class UserDataServiceTest
    {
        [TestMethod]
        public async Task SaveAsync()
        {
            /*var userItemDbModel = new UserItemDbModel()
            {
                ElementId = "ElementId",
                BeginTicks = 100,
                EndTicks = 100,
                Type = "Type",
                ModuleId = "ModuleId",
                Json = "Jso1n"
            };
            string id= await (new UserDataService()).SaveAsync("SiteId", "UserId2", userItemDbModel);

            var userItemDbModel2 = new UserItemDbModel()
            {
                Id = id,
                ElementId = "ElementId",
                BeginTicks = 100,
                EndTicks = 100,
                Type = "Type",
                ModuleId = "ModuleId",
                Json = "Jso2n"
            };
            await (new UserDataService()).SaveAsync("SiteId", "UserId2", userItemDbModel2);*/
        }

       /* [TestMethod]
        public async Task GetAsync()
        {
           var result = await(new UserDataService()).GetAsync("SiteId", "UserId");
           Assert.AreNotEqual(result, null);
        }*/

    }
}