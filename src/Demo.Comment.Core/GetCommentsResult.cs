using System.Collections.Generic;

namespace Demo.Business.Command.Comment
{
    public class GetCommentsResult
    {
        public IList<CommentItem> Comments { get; set; }
    }
}