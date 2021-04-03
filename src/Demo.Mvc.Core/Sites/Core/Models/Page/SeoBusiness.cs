using System;

namespace Demo.Mvc.Core.Sites.Core.Models.Page
{
    public class SeoBusiness : SeoBusinessMaster
    {
        #region Public Properties

        public DateTime? UpdateDate { get; set; }

        public SitemapFrequence SitemapFrequence { get; set; }
        public string MetaDescription { get; set; }
        public string MetaKeyword { get; set; }
        public string SocialImageUrl { get; set; }

        #endregion
    }
}