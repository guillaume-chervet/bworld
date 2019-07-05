using Demo.User.Site;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Business.Command.Administration.User
{
    public class SaveSiteUserInput
    {
        [Required]
        public string Mail { get; set; }
        public bool? IsEmailNotif { get; set; }

        [Required]
        public string SiteId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Birthdate { get; set; }

        public IList<SiteUserRole> Roles { get; set; }
        public CivilityType? Civility { get; set; }
        public string SiteUserId { get; set; }

        public IList<string> Tags { get; set; }
        public string Comments { get; set; }
    }
}
