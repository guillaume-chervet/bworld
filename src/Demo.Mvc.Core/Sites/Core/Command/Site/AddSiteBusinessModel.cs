using System.Collections.Generic;
using Demo.Mvc.Core.Sites.Core.Command.Free.Models;

namespace Demo.Mvc.Core.Sites.Core.Command.Site
{
    public class AddSiteBusinessModel : FreeBusinessModel
    {
        public IList<SiteTemplate> Templates { get; set; }
        public string UrlConditionsGeneralesUtilisations { get; set; }
    }
}