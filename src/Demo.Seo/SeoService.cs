using System;
using System.Threading.Tasks;

namespace Demo.Seo
{
    public class SeoService
    {
        private readonly BddMongo _bdd;

        public SeoService(BddMongo bdd)
        {
            this._bdd = bdd;
        }

        /// <summary>
        ///     Récupère le html d'une adresse web
        /// </summary>
        /// <param name="url"></param>
        public async Task<Page> GetHtmlAsync(string url)
        {
            return await _bdd.GetAsync(url);
        }

    }
}