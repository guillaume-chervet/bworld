namespace Demo.Mvc.Core.Sites.Core.Command.File.Models
{
    public class FilesStatus
    {
        public string PropertyName { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }

        public string DisplayType
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Type) && Type.ToLower().Contains("video"))
                {
                    return "video";
                }
                return "image";
            }
        }

        public string DeleteUrl { get; set; }
        public string DeleteType { get; set; }
        public bool IsTemporary { get; set; }
        public string SiteId { get; set; }
        public FileDetail Detail { get; set; }
        public FileDetail DetailThumbmail { get; set; }
    }


    public class FileDetail
    {
        public string Url { get; set; }
        public ImageSize ImageSize { get; set; }
        public long Size { get; set; }
    }
}