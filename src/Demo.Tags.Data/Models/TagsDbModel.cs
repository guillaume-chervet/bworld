using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Demo.Data.Tags.Models
{
    [BsonIgnoreExtraElements]
    public class TagsDbModel : DbModelBase
    {
        [BsonElement("SiteId")]
        public string SiteId { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; }

        [BsonElement("Tags")]
        public IList<TagDbModel> Tags { get; set; }

        [BsonElement("ModuleId")]
        public string ModuleId { get; set; }
    }
}