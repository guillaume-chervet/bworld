using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Demo.Data.Comment.Models
{
    [BsonIgnoreExtraElements]
    public class CommentsDbModel
    {
        private string _id;

        [BsonElement("_id")]
        [BsonId(IdGenerator = typeof (CombGuidGenerator))]
        internal ObjectId Guid { get; set; }

        [BsonIgnore]
        public string Id
        {
            get
            {
                if (!string.IsNullOrEmpty(Guid.ToString()))
                {
                    return Guid.ToString();
                }
                return _id;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Guid = new ObjectId(value);
                    _id = value;
                }
            }
        }

        [BsonElement("SiteId")]
        public string SiteId { get; set; }

        [BsonElement("ModuleId")]
        public string ModuleId { get; set; }

        [BsonElement("Comments")]
        public IList<CommentDbModel> Comments { get; set; }

        [BsonElement("NumberComments")]
        public int NumberComments { get; set; }
    }
}