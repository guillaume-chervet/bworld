using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Business.Command.User.Models
{
    public class SendConfirmEmailModel
    {

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Provider { get; set; }
    }
}
