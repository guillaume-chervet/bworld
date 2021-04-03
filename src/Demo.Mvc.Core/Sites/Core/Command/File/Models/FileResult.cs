using System.Collections.Generic;

namespace Demo.Mvc.Core.Sites.Core.Command.File.Models
{
    public class FileResult
    {
        public FileResult()
        {
            Files = new List<FilesStatus>();
        }

        public List<FilesStatus> Files { get; set; }
    }
}