using System;
using System.Collections.Generic;
using Demo.Business.Command.Free.Models;
using Demo.User.Identity.Models;

namespace Demo.Business.Command.News.Models
{
    public class GetNewsItemBase : GetFreeResult
    {
        public string ModuleId { get; set; }
        public string ParentTitle { get; set; }
        public string ParentModuleId { get; set; }
    }
}