using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Demo.Mvc.Core.Data
{
    public class MongoDatabase : IDatabase
    {
        private readonly MongoDB.Driver.IMongoDatabase _database;

        public MongoDatabase(IOptions<DataConfig> otpions)
        {
            var dataConfig = otpions.Value;
            var client = new MongoClient(dataConfig.ConnectionString);
            _database = client.GetDatabase(dataConfig.DatabaseName);
        }

        public IMongoDatabase GetDatabase()
        {
            return _database;
        }
    }
}