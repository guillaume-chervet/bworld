using System.Collections.Generic;
using Demo.Mvc.Core.Sites.Core.Command.Free.Models;
using Demo.Mvc.Core.Sites.Core.Models.Page;

namespace Demo.Mvc.Core.Sites.Core.Command.Site.Master
{
    public class Master
    {
        public string Title { get; set; }
        public string FacebookAuthenticationAppId { get; set; }
        public string GoogleAuthenticationAppId { get; set; }
        public string Id { get; set; }
        public SeoBusinessMaster Seo { get; set; }
        public string ColorBackgroundMenu { get; internal set; }
     public string ColorHoverBackgroundMenu { get; set; }
        public string ColorSelectedBackgroundMenu { get; set; }
        public string StyleTemplate { get; set; }
        public string Theme { get; set; }
        public IList<DataFileInput> ImageIcones { get; set; }
        public string ImageIconeId { get; set; }
        public IList<DataFileInput> ImageLogos { get; set; }
        public string ImageLogoId { get; set; }
        public string ImageLogoFileName { get; set; }
        public string ImageIconeFileName { get; set; }
        public bool IsJumbotron { get; set; }
        public string ColorH1 { get; set; }
        public string ColorH2 { get; set; }
        public string ColorJumbotron { get; internal set; }
        public string ColorBackgroundMenuBottom { get; internal set; }
        public string ColorH3 { get; internal set; }
        public string Color { get; set; }
        public string ColorBackground { get; set; }
    }
}