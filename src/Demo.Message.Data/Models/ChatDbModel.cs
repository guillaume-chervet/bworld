using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Data.Message.Models
{
    [BsonIgnoreExtraElements]
    public class ChatDbModel : DbModelBase
    {
        [BsonElement("Title")]
        public string Title { get; set; }

        [BsonElement("Messages")]
        public IList<MessageDbModel> Messages { get; set; }

        [BsonElement("NumberMessages")]
        public int NumberMessages { get; set; }

        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

        [BsonElement("UpdateDate")]
        public DateTime? UpdateDate { get; set; }

        [BsonElement("To")]
        public IList<To> To { get; set; }

        [BsonElement("Reads")]
        public IList<Read> Reads { get; set; }

        [BsonElement("LastReads")]
        public IList<Read> LastReads { get; set; }
    }
}