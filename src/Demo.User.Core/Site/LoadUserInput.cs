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
   public class LoadUserInput
    {
        public string SiteId { get; set; }
        public string SiteUserId { get; set; }

    }
}
