namespace Demo.Mvc.Core.Sites.Core.Command.File.Models
{
    public class SaveFileTempInput
    {
        public FileData FileData { get; set; }
        public string SiteId { get; set; }
        public string ConfigJson { get; set; }
    }
}