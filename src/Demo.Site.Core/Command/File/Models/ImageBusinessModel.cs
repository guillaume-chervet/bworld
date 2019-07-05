namespace Demo.Business.Command.File.Models
{
    public class ImageBusinessModel
    {
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public int FileSize { get; set; }
        public byte[] Contents { get; set; }
        public ImageSize Size { get; set; }
    }
}