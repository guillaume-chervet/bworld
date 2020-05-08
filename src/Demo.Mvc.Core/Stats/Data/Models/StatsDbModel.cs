using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Data.Stat.Models
{
    public class StatsDbModel : DbModelBase
    {
        [BsonElement("Stats")]
        public IList<StatDbModel> Stats { get; set; }

        [BsonElement("Date")]
        public DateTime Date { get; set; }

        [BsonElement("SiteId")]
        public string SiteId { get; set; }
    }
}