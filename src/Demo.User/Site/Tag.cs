using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.User;
using MongoDB.Bson.Serialization.Attributes;
using Demo.User.Site;

namespace Demo.Business.Command.Administration.User
{
    [BsonIgnoreExtraElements]
    public class TagDbModel: DbModelBase
    {
        [BsonElement("Id")]
        public string Id { get; set; }
        [BsonElement("Name")]
        public string Name { get; set; }
        [BsonElement("IsDeleted")]
        public bool IsDeleted { get; set; }
    }
}
