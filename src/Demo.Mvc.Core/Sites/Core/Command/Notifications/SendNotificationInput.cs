using System.Collections.Generic;
using Demo.Mvc.Core.Sites.Core.Command.Free;

namespace Demo.Mvc.Core.Sites.Core.Command.Notifications
{
    public class SendNotificationInput
    {

        public string SiteId { get; set; }

        public IList<string> SiteUserIds { get; set; }

        public SaveFreeInput Data { get; set; }

    }
}
