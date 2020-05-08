using Demo.Mvc.Core.Routing.Extentions;

namespace Demo.Mvc.Core.Routing.Models
{
    public class FindRouteInput
    {
        private string _url;
        public string Port { get; set; }

        public string Url
        {
            get => _url;
            set => _url = UrlHelper.RemoveLastSeparator(value);
        }

        public string FullUrl { get; set; }

        /// <summary>
        ///     Es ce que la requête courante est en https?
        /// </summary>
        public bool IsSecure { get; set; }
    }
}