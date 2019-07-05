using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Demo.Data.Message.Models
{
    public abstract class DbModelBase
    {

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
                return string.Empty;
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
                }
            }
        }
    }
}