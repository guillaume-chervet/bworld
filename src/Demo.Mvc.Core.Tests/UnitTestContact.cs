using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Business.Command.Contact.Message.Models;
using Demo.Business.Command.Contact.Message.Models.SendMessage;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Message.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Xunit;

namespace Demo.Mvc.Core.Tests
{
    public class UnitTestContact
    {
       /* internal readonly MongoDbRunner _mongoRunner;
        
        public UnitTestContactTests()
        {
            RadarTechno.History.History[] histories = new[]
            {
                new RadarTechno.History.History("author", "entity-technology", "id1", "diff"),
                new RadarTechno.History.History("author", "entity-technology", "id1", "diff2"),
                new RadarTechno.History.History("author", "entity-technology", "id1", "diff3"),
                new RadarTechno.History.History("author", "entity-technology", "id2", "diff")
            };

            _mongoRunner = MongoDbRunner.Start();
            MongoClient client = new MongoClient(_mongoRunner.ConnectionString);
            IMongoDatabase database = client.GetDatabase("radar-techno");
            var collection = database.GetCollection<RadarTechno.History.History>("history");
            collection.InsertMany(histories);
            var databaseSettings = new DatabaseSettings()
            {
                ConnectionString = _mongoRunner.ConnectionString,
                Database = "radar-techno"
            };
            IOptions<DatabaseSettings> options = Options.Create<DatabaseSettings>(databaseSettings);
            _database = new RadarDatabase(options);
            _repository = new HistoryRepository(_database);
        }
        [Fact]
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
            
          var command =  new SendMessageCommand();

          command.ExecuteAsync();


        }*/
}
}