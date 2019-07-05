using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Data.Tags.Models
{
    [BsonIgnoreExtraElements]
    public class TagDbModel : DbModelBase
    {
        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("IsDeleted")]
        public bool IsDeleted { get; set; }
    }
}