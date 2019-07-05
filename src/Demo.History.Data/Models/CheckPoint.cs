using System;
using System.Collections.Generic;
using DiffMatchPatch;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Data.History.Models
{
    public class CheckPoint
    {
        public string Id { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("DateUpdate")]
        public DateTime DateUpdate { get; set; }

        [BsonElement("CheckPointType")]
        public CheckPointType CheckPointType { get; set; }

        public string Patch { get; set; }

        public string Hash { get; set; }
    }
}
