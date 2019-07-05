using System.Collections.Generic;

namespace Demo.Business.Command.File.Models
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