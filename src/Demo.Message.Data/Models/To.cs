using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Data.Message.Models
{
    [BsonIgnoreExtraElements]
    public class To
    {
        [BsonElement("BoxId")]
        public BoxId Id { get; set; }
    }
}