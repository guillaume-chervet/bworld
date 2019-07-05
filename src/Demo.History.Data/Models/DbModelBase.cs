using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Data.History.Models
{
    public abstract class DbModelBase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        internal ObjectId ObjectId { get; set; }       

        [BsonIgnore]
        public string Id
        {
            get
            {
                if (ObjectId != ObjectId.Empty)
                {
                    return ObjectId.ToString();
                }
                return string.Empty;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    ObjectId temp;
                    if (ObjectId.TryParse(value, out temp))
                    {
                        ObjectId = temp;
                    }
                }
            }
        }
    }
}