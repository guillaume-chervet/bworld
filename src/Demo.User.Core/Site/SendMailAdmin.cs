using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.User;
using Demo.User.Identity;
using Demo.User.Site;

namespace Demo.Business.Command.Administration.User
{
    public class SendMailAdmin
    {
        public string Mail { get; set; }
        public ApplicationUser UserCreator { get; set; }
        public string SiteId { get; set; }
        public ApplicationUser UserCreated { get; set; }
    }
}
