using Demo.Routing.Extentions;

namespace Demo.Routing.Models
{
    public class FindRouteInput
    {
        public string Port { get; set; }

        private string _url;
        public string Url
        {
            get { return _url; }
            set
            {
                _url = UrlHelper.RemoveLastSeparator(value);
            }
        }
        public string FullUrl { get; set; }

        /// <summary>
        ///     Es ce que la requête courante est en https?
        /// </summary>
        public bool IsSecure { get; set; }
    }
}