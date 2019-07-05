using Microsoft.Extensions.Logging;

namespace Demo.Log
{
    public class MongoConfiguration
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public LogLevel MinLevel { get; set; }
    }
}