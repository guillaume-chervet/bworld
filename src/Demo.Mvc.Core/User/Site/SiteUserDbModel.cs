using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Business.Command.Administration.User;
using Demo.Common;
using Demo.Mvc.Core.Common;
using Demo.Mvc.Core.User;
using MongoDB.Bson.Serialization.Attributes;
using Demo.User.Identity;

namespace Demo.User.Site
{
    [BsonIgnoreExtraElements]
    public class SiteUserDbModel : DbModelBase
    {
        [BsonElement("Mail")]
        public string Mail { get; set; }
        [BsonElement("SiteId")]
        public string SiteId { get; set; }
        [BsonElement("UserId")]
        public string UserId { get; set; }

        private string firstName;
        private string lastName;    
        [BsonElement("FirstName")]
        public string FirstName
        {
            get { return firstName; }
            set { firstName = StringHelper.FirstLetterToUpper(value); }
        }

        [BsonElement("LastName")]
        public string LastName
        {
            get { return lastName; }
            set { lastName = StringHelper.FirstLetterToUpper(value); }
        }

        [BsonIgnore]
        public string FullName
        {
            get { return ApplicationUser.GetFullName(FirstName, LastName); }
        }

        [BsonElement("Birthdate")]
        public DateTime? Birthdate { get; set; }

        [BsonElement("FlaggedRoles")]
        public IList<SiteUserRole> FlaggedRoles { get; set; }

        [BsonElement("Tags")]
        public IList<string> Tags { get; set; }

        [BsonElement("Comments")]
        public string Comments { get; set; }

        [BsonElement("Civility")]
        public CivilityType? Civility { get; set; }
        [BsonElement("IsEmailNotif")]
        public bool? IsEmailNotif { get; set; }

    }
}
