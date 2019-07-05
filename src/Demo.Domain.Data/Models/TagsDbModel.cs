using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Demo.Data.Tags.Models
{
    [BsonIgnoreExtraElements]
    public class TagsDbModel : DbModelBase
    {
      /*  private string _id;

        [BsonElement("_id")]
        [BsonId(IdGenerator = typeof (CombGuidGenerator))]
        internal ObjectId Guid { get; set; }

        [BsonIgnore]
        public string Id
        {
            get
            {
                if (Guid != ObjectId.Empty  )
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
        }*/

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