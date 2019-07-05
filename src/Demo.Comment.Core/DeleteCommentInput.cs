namespace Demo.Business.Command.Comment
{
    public class DeleteCommentInput
    {
        public string ModuleId { get; set; }
        public string SiteId { get; set; }
        public string CommentId { get; set; }
    }
}