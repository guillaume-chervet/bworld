using System.Threading.Tasks;
using Demo.Mvc.Core.Routing.Models;

namespace Demo.Mvc.Core.Routing
{
    public interface IRouteManager
    {
        /// <summary>
        ///     Récupère la première route qui match avec les parmètres
        /// </summary>
        /// <param name="input"></param>
        /// <param name="routeProvider"> </param>
        /// <returns></returns>
        Task<FindRouteResult> FindRouteAsync(FindRouteInput input);

        /// <summary>
        ///     Retourne le chemin complet de la route et domaine qui match
        /// </summary>
        /// <param name="input"></param>
        /// <param name="routeProvider"> </param>
        /// <returns></returns>
        Task<FindPathResult> FindDomainPathAsync(FindPathInput input);
    }
}