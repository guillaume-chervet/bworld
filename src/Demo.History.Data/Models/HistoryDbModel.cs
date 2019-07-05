using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Data.History.Models
{
    public class HistoryDbModel : DbModelBase
    {
        [BsonElement("ElementId")]
        public string ElementId { get; set; }
        [BsonElement("CheckPoints")]
        public IList<CheckPoint> CheckPoints { get; set; }
    }
}
