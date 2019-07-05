using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Message;
using Demo.Data.Message.Models;
using Demo.Data.Stat;
using Demo.Log;
using Demo.User;
using Demo.User.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
/*
namespace Demo.Business.Tests
{
    [TestClass]
    public class ContactTest
    {
        private readonly BusinessFactory _business = new BusinessFactory(new DataFactoryMongo(new DataConfig(), new MongoBlob(new DataConfig())));

        [TestInitialize]
        public void Initialize()
        {

        }

        [TestMethod]
        public async Task ListMessageCommand()
        {
            // https://localhost:44301/api/contact/messages/0/
            var sendMessageInput = new ListMessageInput
            {
                BoxId = new BoxId {Id = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c", Type = TypeBox.Site}
            };
            var userInput = new UserInput<ListMessageInput>
            {
                UserId = "54b8c4278241320a28c8f024",
                Data = sendMessageInput
            };
          /*  var command = new ListMessageCommand();
            var result =
                await
                    _business
                        .InvokeAsync<ListMessageCommand, UserInput<ListMessageInput>, CommandResult<ListMessageResult>>(command,
                            userInput);

            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task SendMessageCommand()
        {
            var messageSiteNotAuthenticated = new MessageSiteNotAuthenticated
            {
                Email = "guillaume.chervet@gmail.com",
                FirstName = "Guillaume",
                LastName = "Chervet",
                Message = "message de test",
                Title = "demande aide",
                Phone = "06 76 96 56 13"
            };

            var userInput = new UserInput<SendMessageInput>
            {
                UserId = "54b8c4278241320a28c8f024",
                Data = new SendMessageInput
                {
                    To = new BoxId {Id = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c", Type = TypeBox.Site},
                    From = new BoxId(),
                    MessageJson = JsonConvert.SerializeObject(messageSiteNotAuthenticated),
                    Type = "SiteNotAuthenticated"
                }
            };
          /*  var command = ServiceLocator.Current.GetInstance<SendMessageCommand>();
            var result = await
                _business.InvokeAsync<SendMessageCommand, UserInput<SendMessageInput>, CommandResult<SendMessageResult>>(command, userInput);

            Assert.IsTrue(result.IsSuccess);
        }

        //df5889d8-edf5-4f56-be50-a4b20092d088

        [TestMethod]
        public async Task GetMessageCommand()
        {
            var userInput = new UserInput<GetMessageInput>
            {
                UserId = "54b8c4278241320a28c8f024",
                Data = new GetMessageInput
                {
                    BoxId = {Type = TypeBox.Site, Id = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c"},
                    ChatId = "df5889d8-edf5-4f56-be50-a4b20092d088"
                }
            };
       /*     var command = new GetMessageCommand>();
            var result = await
                _business.InvokeAsync<GetMessageCommand, UserInput<GetMessageInput>, CommandResult<GetMessageResult>>(command,
                    userInput);

            Assert.IsTrue(result.IsSuccess);
        }
    }
}*/
