using System.Collections.Generic;

namespace Demo.Routing.Models
{
    public class FindPathInput
    {
        /// <summary>
        ///     Si la route courante est sécurisée
        /// </summary>
        public bool? IsSecure { get; set; }

        public string DomainId { get; set; }

        /// <summary>
        ///     Non obligatoire si Domain Id renseigné
        /// </summary>
        public string MasterDomainId { get; set; }

        public string Port { get; set; }

        /// <summary>
        ///     Liste des clé valeur associé à aux domaine néssessaire pour regérer une url
        /// </summary>
        public IDictionary<string, string> DomainDatas { get; set; }

        /// <summary>
        ///     Data utilisée pour générer le nouveau chemin
        /// </summary>
        public IDictionary<string, string> Datas { get; set; }
    }
}