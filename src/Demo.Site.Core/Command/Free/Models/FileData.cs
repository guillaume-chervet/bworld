namespace Demo.Business.Command.Free.Models
{
    public class FileData
    {
        public string Id { get; set; }
        public string PropertyName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbDisplayMode { get; set; }
        public string Behavior { get; set; }
        public Link Link { get; set; }

    }
}