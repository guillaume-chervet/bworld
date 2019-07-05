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
   public class ListUserResult
    {
        public List<UserResult> Users { get; set; }

    }
}
