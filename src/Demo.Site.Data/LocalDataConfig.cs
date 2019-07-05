using Demo.Data.Mongo;

namespace Demo.Data
{
    public class LocalDataConfig : DataConfig
    {
        public LocalDataConfig()
        {
            MongoConnectionString = "mongodb://localhost:27017";
            MongoDatabaseName = "bworld"; 
        }

        public string MongoConnectionString { get; }
        public string MongoDatabaseName { get; }
    }
}