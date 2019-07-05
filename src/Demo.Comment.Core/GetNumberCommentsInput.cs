using System.Collections.Generic;

namespace Demo.Business.Command.Comment
{
    public class GetNumberCommentsInput
    {
        public IList<string> ModuleIds { get; set; }
    }
}