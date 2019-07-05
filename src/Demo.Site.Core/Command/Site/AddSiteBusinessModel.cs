using System.Collections.Generic;
using Demo.Business.Command.Free.Models;

namespace Demo.Business.Command.Site
{
    public class AddSiteBusinessModel : FreeBusinessModel
    {
        public IList<SiteTemplate> Templates { get; set; }
        public string UrlConditionsGeneralesUtilisations { get; set; }
    }
}