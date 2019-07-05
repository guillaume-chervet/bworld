using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Data.Comment.Models
{
    [BsonIgnoreExtraElements]
    public class CommentDbModel : DbModelBase
    {
        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("Comment")]
        public string Comment { get; set; }

        [BsonElement("DateCreate")]
        public DateTime DateCreate { get; set; }

        [BsonElement("IsDeleted")]
        public bool IsDeleted { get; set; }
    }
}