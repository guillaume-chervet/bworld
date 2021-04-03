using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Demo.Mvc.Core.Sites.Data.Azure
{
    [BsonIgnoreExtraElements]
    public class FileMetadata
    {
        private string id;

        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
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
                return id;
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
                    id = value;
                }
            }
        }

        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

        [BsonElement("UpdateDate")]
        public DateTime? UpdateDate { get; set; }

        [BsonElement("PropertyName")]
        public string PropertyName { get; set; }

        [BsonElement("SiteId")]
        public string SiteId { get; set; }

        [BsonElement("ParentId")]
        public string ParentId { get; set; }

        [BsonElement("Index")]
        public int Index { get; set; }

        [BsonElement("Json")]
        public string Json { get; set; }

        /// <summary>
        ///     Nombre de byte présent dans le JSon
        /// </summary>
        [BsonElement("SizeBytes")]
        public long? SizeBytes { get; set; }

        [BsonElement("TypeFile")]
        public string TypeFile { get; set; }

        [BsonElement("Filename")]
        public string Filename { get; set; }
        [BsonElement("ContentType")]
        public string ContentType { get; set; }

        [BsonElement("IsTemporary")]
        public bool IsTemporary { get; set; }
        public string Module { get; internal set; }
        public string Type { get; internal set; }

        /*
        document["id"] = fileDataModel.SiteId;
        document["type"] = 0;
        document["index"] = fileDataModel.Index;
        document["isTemporary"] = fileDataModel.IsTemporary;
        document["module"] = fileDataModel.Module;
        document["parentId"] = fileDataModel.ParentId;
        document["propertyName"] = fileDataModel.PropertyName;
        document["createDate"] = fileDataModel.CreateDate;
        document["updateDate"] = fileDataModel.UpdateDate;
        if (fileDataModel.Data != null)
        {
            document["json"] = JsonConvert.SerializeObject(fileDataModel.Data);
        }   
    */
    }
}
