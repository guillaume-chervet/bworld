namespace Demo.Business.Command.Social
{
    public class SocialBusinessModel
    {
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// If "true" then the button is always visible even in mobile mode
        /// </summary>
        public bool IsAlwaysVisible { get; set; }
        public Socials Socials { get; set; }
    }
}