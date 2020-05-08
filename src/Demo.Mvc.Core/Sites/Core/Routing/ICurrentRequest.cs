using System.Collections.Generic;

namespace Demo.Mvc.Core.Sites.Core.Routing
{
    /// <summary>
    ///     Possède les informations sur la requêtes qui permettent de
    ///     regénérer les URL, attention guillaume
    ///     ne rajoute jamais d'information lié à la requête car cette information servira à mettre en Cache
    ///     les données
    /// </summary>
    public interface ICurrentRequest
    {
        /// <summary>
        ///     Si la route courante est sécurisée
        /// </summary>
        bool IsSecure { get; }

        /// <summary>
        ///     On doit forcer la connexion sécurisée
        /// </summary>
        bool? IsForceSecure { get; }

        string SiteId { get; }
        string DomainId { get; }
        string Port { get; }

        /// <summary>
        ///     Liste des clé valeur associé à aux domaine néssessaire pour regérer une url
        /// </summary>
        IDictionary<string, string> DomainDatas { get; }
    }
}