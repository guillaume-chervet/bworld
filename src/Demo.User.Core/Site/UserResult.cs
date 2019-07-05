using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.User.Site;

namespace Demo.Business.Command.Administration.User
{
    public class UserResult
    {
        public IList<SiteUserRole> Roles { get; set; }

        public string Mail { get; set; }
        public bool MailConfirmed { get; set; }
        public bool? IsEmailNotif { get; set; }
        public string SiteUserId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthdate { get; set; }
        public string FullName { get; internal set; }
        public string Comments { get; internal set; }
        public IList<string> Tags { get; internal set; }
        public CivilityType? Civility { get; internal set; }
    }
}
