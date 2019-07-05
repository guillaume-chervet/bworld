using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Demo.Data.Comment.Models
{
    public abstract class DbModelBase
    {
        private string _id;

        [BsonElement("_id")]
        [BsonId(IdGenerator = typeof (CombGuidGenerator))]
        internal Guid Guid { get; set; }

        [BsonIgnore]
        public string Id
        {
            get
            {
                if (Guid != Guid.Empty)
                {
                    return Guid.ToString();
                }
                return _id;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Guid temp;
                    if (Guid.TryParse(value, out temp))
                    {
                        Guid = temp;
                    }
                    _id = value;
                }
            }
        }
    }
}