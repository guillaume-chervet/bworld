using System.Collections.Generic;

namespace Demo.Mvc.Core.Routing.Implementation
{
    public class Domain
    {
        public string Id { get; set; }
        public string SiteId { get; set; }
        public string Path { get; set; }
        public IDictionary<string, string> ExcludedDomainData { get; set; }

        public string Regex { get; set; }

        //public string XDomainRegex { get; set; }
        public int Index { get; set; }
        public SecureMode SecureMode { get; set; }
        public string DomainMasterId { get; set; }

        /// <summary>
        ///     Indique si le domaine doit être redirigé en http302 vers un autre domaine
        /// </summary>
        public string RedirecToDomainId { get; set; }

        /// <summary>
        ///     Indique l'url domaine de login du site (sans le path)
        /// </summary>
        public string DomainLoginUrl { get; set; }

        public string FacebookAppId { get; set; }
    }
}