using Demo.Business.Command.Free.Models;
using System.Collections.Generic;

namespace Demo.Business.Command.Notifications
{
    public class SendNotificationInput
    {

        public string SiteId { get; set; }

        public IList<string> SiteUserIds { get; set; }

        public SaveFreeInput Data { get; set; }

    }
}
