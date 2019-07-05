namespace Demo.Business.Command.File.Models
{
    public class SaveFileResult
    {
        public string Id { get; set; }
        public string SiteId { get; set; }
        public string PropertyName { get; set; }
        public string Url { get; set; }
        public ImageSize ImageUploadedSize { get; set; }
        public ImageSize ImageThumbSize { get; set; }
    }
}