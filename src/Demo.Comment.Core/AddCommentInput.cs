namespace Demo.Business.Command.Comment
{
    public class AddCommentInput
    {
        public string Comment { get; set; }
        public string UrlPath { get; set; }
        public string ArticleTitle { get; set; }
        public string ModuleId { get; set; }
        public string SiteId { get; set; }
        public CurrentRequest Site { get; set; }
    }
}