using System.Collections.Generic;
using Demo.Mvc.Core.User;
using Demo.User.Site;
using MongoDB.Bson.Serialization.Attributes;

namespace Demo.User.SiteData.Model
{
    [BsonIgnoreExtraElements]
    public class UserDataDbModel : DbModelBase
    {
        [BsonElement("SiteId")]
        public string SiteId { get; set; }
        [BsonElement("CookieSessionId")]
        public string CookieSessionId { get; set; }
        [BsonElement("UserId")]
        public string UserId { get; set; }
        [BsonElement("ModuleId")]
        public string ModuleId { get; set; }
        [BsonElement("ElementId")]
        public string ElementId { get; set; }
        [BsonElement("Type")]
        public string Type { get; set; } = "form";
        [BsonElement("BeginTicks")]
        public long BeginTicks { get; set; }
        [BsonElement("EndTicks")]
        public long EndTicks { get; set; }
        [BsonElement("Json")]
        public string Json { get; set; }
    }
}
