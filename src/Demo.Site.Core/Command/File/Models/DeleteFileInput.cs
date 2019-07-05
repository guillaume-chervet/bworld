namespace Demo.Business.Command.File.Models
{
    public class DeleteFileInput
    {
        public string Id { get; set; }
        public string SiteId { get; set; }
        public string PropertyName { get; set; }
    }
}