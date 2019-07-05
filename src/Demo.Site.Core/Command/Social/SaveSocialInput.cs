using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Business.Command.Free.Models;
using Demo.Business.Command.Site.Master;

namespace Demo.Business.Command.Social
{
    public class SaveSocialInput : SaveModuleInputBase
    {
        public SocialBusinessModel Data { get; set; }
    }
}