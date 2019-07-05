using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Demo.Data.Model
{
    [BsonIgnoreExtraElements]
    public class Item
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

        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

        [BsonElement("UpdateDate")]
        public DateTime? UpdateDate { get; set; }

        [BsonElement("PropertyName")]
        public string PropertyName { get; set; }

        [BsonElement("Module")]
        public string Module { get; set; }

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
        public int? SizeBytes { get; set; }

        [BsonIgnore]
        public string Type {
            get
            {
                switch (Module)
                {
                    case "Master":
                        return "Demo.Business.Command.Site.Master.MasterBusinessModel";
                    case "Free":
                        return "Demo.Business.Command.Free.Models.FreeBusinessModel";
                    case "ImageData":
                        return "Demo.Business.Command.File.Models.ImageDataBusinessModel";
                    case "Image":
                        return "Demo.Business.Command.File.Models..ImageBusinessModel";
                    case "Seo":
                        return "Demo.Business.Command.Site.Seo.SeoBusinessModel";
                    case "Notification":
                        return "Demo.Business.Command.Notifications.NotificationBusinessModel";
                    case "NotificationItem":
                        return "Demo.Business.Command.Notifications.NotificationItemBusinessModel";
                    case "News":
                        return "Demo.Business.Command.News.Models.NewsBusinessModel";
                    case "NewsItem":
                        return "Demo.Business.Command.News.Models.NewsItemBusinessModel";
                    case "Site":
                        return "Demo.Business.SiteBusinessModel";
                    case "AddSite":
                        return "Demo.Business.Command.Site.AddSiteBusinessModel";
                    default:
                        return string.Empty;
                }
            } }

        [BsonElement("Tags")]
        public IList<string> Tags { get; set; }

        [BsonElement("IsTemporary")]
        public bool IsTemporary { get; set; }
        
        [BsonElement("State")]
        public ItemState State { get; set; } = ItemState.Published;
        
        
        /*ùinternal int StateInt { get => (int) State;
            set => State = (ItemState) value;
        } */
    }
}