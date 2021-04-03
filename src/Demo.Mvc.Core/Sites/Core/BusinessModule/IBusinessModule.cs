using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Mvc.Core.Routing.Implementation;
using Demo.Mvc.Core.Sites.Core.BusinessModule.Models;

namespace Demo.Mvc.Core.Sites.Core.BusinessModule
{
    public interface IBusinessModule
    {
        /// <summary>
        ///     Menu(s) lié au module
        /// </summary>
        /// <param name="geMenuItemInput"></param>
        /// <returns></returns>
        Task GetInfoAsync(GeMenuItemInput geMenuItemInput);

        /// <summary>
        ///     Récupère les Metadada du module root (main page)
        /// </summary>
        /// <param name="getRootMetaDataInput"></param>
        /// <returns></returns>
        IDictionary<string, string> GetRootMetadata(GetRootMetaDataInput getRootMetaDataInput);

        /// <summary>
        ///     Liste des route associé au module
        /// </summary>
        /// <param name="domainDatas"></param>
        /// <returns></returns>
        IEnumerable<Route> GetRoutes(IDictionary<string, string> domainDatas);
    }
}