namespace Demo.Business.Command.File.Models
{
    public class SaveFileTempInput
    {
        public FileData FileData { get; set; }
        public string SiteId { get; set; }
        public string ConfigJson { get; set; }
    }
}