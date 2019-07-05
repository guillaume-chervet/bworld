using System.Collections.Generic;

namespace Demo.Business.Command.Comment
{
    public class GetNumberCommentsResult
    {
        public IDictionary<string, int> Comments { get; set; }
    }
}