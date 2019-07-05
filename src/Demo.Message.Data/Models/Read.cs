using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Data.Message.Models
{
    [BsonIgnoreExtraElements]
    public class Read
    {
        [BsonElement("BoxId")]
        public BoxId Id { get; set; }

        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}