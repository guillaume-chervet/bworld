using System;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Log
{
    public class Log
    {
        public Log()
        {
            CreateAt = DateTimeOffset.Now;
        }

        [BsonId] public ObjectId Id { get; set; }

        public DateTimeOffset CreateAt { get; set; }

        public string Message { get; set; }
        public string Exception { get; set; }
        public LogLevel Level { get; set; }
        public string ApplicationName { get; set; }
        public string Environment { get; set; }
    }
}