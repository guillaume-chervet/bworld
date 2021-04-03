using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Mvc.Core.Routing.Implementation;

namespace Demo.Mvc.Core.Routing
{
    /// <summary>
    ///     Fournisseur de données pour les route
    /// </summary>
    public interface IRouteProvider
    {
        /// <summary>
        ///     Liste des domaines ordonnées par index décroissant
        /// </summary>
        IEnumerable<Implementation.Domain> Domains { get; }

        /// <summary>
        ///     Protocole à appliquer lorsque l'on est en sécurisé
        /// </summary>
        string ProtocolSecure { get; }

        /// <summary>
        ///     Protocole à appliquer par défault
        /// </summary>
        string ProtocolDefault { get; }

        Task<string> GetSiteIdAsync(IDictionary<string, string> data, string masterDomainId);

        /// <summary>
        ///     Retourne les informations qui permettent de retrouver la route par défault du site
        /// </summary>
        /// <returns></returns>
        Task<IDictionary<string, string>> GetRootMetadataAsync(string siteId);

        Task<IEnumerable<Route>> GetRedirectRoutesAsync(string siteId);

        /// <summary>
        ///     Liste des routes ordonnées par index décroissant
        /// </summary>
        IEnumerable<Route> GetRoutes(IDictionary<string, string> domainDatas);
    }
}