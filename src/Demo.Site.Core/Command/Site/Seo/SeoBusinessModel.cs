using System.Collections.Generic;

namespace Demo.Business.Command.Site.Seo
{
    public class SeoBusinessModel
    {
        public IList<SeoValidationMeta> Metas { get; set; }
        public IList<string> Disallows { get; set; }
        public IList<SeoRedirect> Redirects { get; set; }
        
    }
}