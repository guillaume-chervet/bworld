namespace Demo.Business.Command.File.Models
{
    public class GetFileInput
    {
        public string Id { get; set; }
        public string PropertyName { get; set; }
        public string SiteId { get; set; }
        public string UserId { get; set; }
        public string Key { get; set; }

        public string Module { get; set; }
    }
}