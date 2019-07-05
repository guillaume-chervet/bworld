namespace Demo.Business.Command.Free.Models
{
    public class DataFileVideoInput : DataFileInput
    {
        public DataFileVideoInput(DataFileInput input, string url)
        {
            this.Id = input.Id;
            this.PropertyName = input.PropertyName;
            this.Name = input.Name;
            this.Type = input.Type;
            this.Size = input.Size;
            this.Witdh = input.Witdh;
            this.Height = input.Height;
            this.IsTemporary = input.IsTemporary;
            this.Title = input.Title;
            this.Description = input.Description;
            this.ThumbDisplayMode = input.ThumbDisplayMode;
            this.Behavior = input.Behavior;
            this.Url = url;
        }

        public string Url { get; set; }
}
}