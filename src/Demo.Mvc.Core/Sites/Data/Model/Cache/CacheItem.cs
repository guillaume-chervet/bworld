using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Demo.Mvc.Core.Sites.Data.Model.Cache
{
    public class CacheItem
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
                return null;
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

        [BsonElement("SiteId")]
        public string SiteId { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; }

        [BsonElement("Key")]
        public string Key { get; set; }

        [BsonElement("Value")]
        public string Value { get; set; }

        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

        public static string GetKey(CacheItem cacheItem)
        {
            return cacheItem.Type + ":" + cacheItem.Key;
        }
    }
}