using System;
using Demo.Common;
using MongoDB.Bson.Serialization.Attributes;
using IdentityUser = Microsoft.AspNetCore.Identity.MongoDB.IdentityUser;

namespace Demo.User.Identity
{
    // Vous pouvez ajouter des données de profil pour l'utilisateur en ajoutant plus de propriétés à votre classe ApplicationUser ; consultez http://go.microsoft.com/fwlink/?LinkID=317594 pour en savoir davantage.
    public class ApplicationUser : IdentityUser
    {
        private string firstName;
        private string lastName;
        public DateTime? CreatedDate { get; set; }
        
        public string FirstName
        {
            get { return firstName; }
            set { firstName = StringHelper.FirstLetterToUpper(value); }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = StringHelper.FirstLetterToUpper(value); }
        }

        [BsonIgnore]
        public string FullName
        {
            get { return GetFullName(FirstName, LastName); }
        }

        public bool IsUserNotifyComment { get; set; } = true;
        public string AuthorUrl { get; set; }

        public static string GetFullName(string FirstName, string LastName)
        {
            if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName))
            {
                return string.Empty;
            }
            return string.Concat(FirstName, " ", LastName);
        }


    }
}