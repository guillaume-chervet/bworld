using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Data.Stat.Models
{
    public abstract class DbModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        internal ObjectId Id { get; set; }
    }
}