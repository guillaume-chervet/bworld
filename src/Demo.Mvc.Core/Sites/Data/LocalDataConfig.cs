using Demo.Mvc.Core.Data;

namespace Demo.Mvc.Core.Sites.Data
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