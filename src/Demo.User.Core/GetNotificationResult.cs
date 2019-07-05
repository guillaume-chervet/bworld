using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Business.Command.User
{
    public class GetNotificationResult
    {
        public long NumberUnreadMessage { get; set; }
        public long NumberSiteUnreadMessage { get; set; }
    }
}
