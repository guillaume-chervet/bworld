using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Business.Command;
using Demo.Business.Command.Free;
using Demo.Business.Command.Free.Models;
using Demo.Business.Routing;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Azure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Demo.Business.Tests
{
    [TestClass]
    public class FreeTest 
    {
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
        }

        /*  [TestMethod]
          public void GetFree()
          {
              using (var business = new BusinessFactory(new DataFactoryMock()))
              {
                  var command = new GetFreeCommand>();
                  var freeCommand =
                      business.Invoke<GetFreeCommand, GetModuleInput, CommandResult<GetFreeResult>>(command, new GetModuleInput
                      {
                          ModuleId = "2"
                      });

                  Assert.IsTrue(freeCommand.IsSuccess);
              }
          }*/

        /*   [TestMethod]
           public async Task UpdateFreeAsync()
           {
               using (var business = new BusinessFactory(new DataFactoryMock()))
               {
                   var input =
                       new SaveFreeInput
                       {
                           ModuleId = "2"
                       };

                   var userInput = new UserInput<SaveFreeInput> {Data = input, UserId = "TOTO"};
                   var command = ServiceLocator.Current.GetInstance<SaveFreeCommand>();
                   var updateFreeCommand =
                       await
                           business.InvokeAsync<SaveFreeCommand, UserInput<SaveFreeInput>, CommandResult<dynamic>>(command,
                               userInput);

                   Assert.IsTrue(updateFreeCommand.IsSuccess);
               }
           }*/
    }
}