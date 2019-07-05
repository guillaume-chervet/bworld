using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Data.Stat.Models
{
    [BsonIgnoreExtraElements]
    public class StatDbModel
    {
        private string _id;

        [BsonIgnore]
        internal Guid Guid { get; set; }

        [BsonElement("Id")]
        public string Id
        {
            get
            {
                if (Guid != Guid.Empty) return Guid.ToString();
                return _id;
            }

            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    Guid temp;
                    if (Guid.TryParse(value, out temp)) Guid = temp;
                    _id = value;
                }
            }
        }

        [BsonElement("Ip")]
        public string Ip { get; set; }

        [BsonElement("ClientSessionId")]
        public string ClientSessionId { get; set; }

        [BsonElement("IsNewClientSesssion")]
        public bool? IsNewClientSesssion { get; set; }

        [BsonElement("CookieSessionId")]
        public string CookieSessionId { get; set; }

        [BsonElement("IsNewCookieSesssion")]
        public bool? IsNewCookieSesssion { get; set; }

        [BsonElement("UserId")]
        public string UserId { get; set; }

        [BsonElement("SiteId")]
        public string SiteId { get; set; }

        [BsonElement("PageName")]
        public string PageName { get; set; }

        [BsonElement("PageParam")]
        public string PageParam { get; set; }

        [BsonElement("Url")]
        public string Url { get; set; }

        [BsonElement("Referrer")]
        public string Referrer { get; set; }

        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

        [BsonElement("Geo")]
        public GeoDbModel Geo { get; set; }

        [BsonElement("UserAgent")]
        public string UserAgent { get; set; }

        [BsonElement("TypeDevice")]
        public string TypeDevice { get; set; }

        [BsonElement("UniversType")]
        public UniversType UniversType { get; set; }
    }
}