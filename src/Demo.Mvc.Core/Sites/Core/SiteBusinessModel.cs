namespace Demo.Mvc.Core.Sites.Core
{
    public class SiteBusinessModel
    {
        public string CategoryId { get; set; }

        /// <summary>
        ///     Nom du site, celui qui apparaît dans l'url
        /// </summary>
        public string Name { get; set; }

        public string Domain { get; set; }

        public string CultureId { get; set; }
        public string MasterDomainId { get; set; }
    }
}