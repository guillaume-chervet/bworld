namespace Demo.Mvc.Core.Sites.Core.Renderers
{
    public class CommentAddedMailModel
    {
        public string UserName { get; set; }
        public string ArticleUrl { get; set; }
        public string ArticleTitle { get; set; }
        public string SiteUrl { get; set; }
        public string SiteName { get; set; }
    }
}