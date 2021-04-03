using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Data.Message.Models
{
    [BsonIgnoreExtraElements]
    public class BoxId
    {
        [BsonElement("Id")]
        public string Id { get; set; }

        [BsonElement("Type")]
        public TypeBox Type { get; set; }
    }
}