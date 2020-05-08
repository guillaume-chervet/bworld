using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Data.Message.Models
{
    [BsonIgnoreExtraElements]
    public class MessageDbModel : DbModelBase
    {
        [BsonElement("FromId")]
        public BoxId FromId { get; set; }

        [BsonElement("Message")]
        public string Message { get; set; }

        [BsonElement("MessageType")]
        public string MessageType { get; set; }
        [BsonElement("Source")]
        public string Source { get; set; }

        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

    }
}