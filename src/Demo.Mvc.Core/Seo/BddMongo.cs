using System;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Collections.Generic;
using Demo.Mvc.Core.Data;

namespace Demo.Seo
{
    public class BddMongo
    {
        private readonly IMongoCollection<Page> _collection;

        public BddMongo(IDatabase db)
        {
            var database = db.GetDatabase();
            _collection = database.GetCollection<Page>("site.seo");
        }

        public async Task SaveAsync(IList<Page> pages)
        {
            if (pages == null)
            {
                return;
            }
            /*  foreach (var page in pages)
              {
                  page.Url =(page.Url);
                  Console.WriteLine(page.Url);
              }*/
              foreach(var page in pages)
            {
                page.Id = Guid.NewGuid();
            }
             await _collection.InsertManyAsync(pages);
        }

        public async Task ClearAsync(string siteId, DateTime date)
        {
            var builder = Builders<Page>.Filter;
            var filter = builder.Eq(x => x.SiteId, siteId) & builder.Lt(x => x.Date, date);

            var result = await _collection.DeleteManyAsync(filter);
            Console.WriteLine("Nombre d'élément effacés:" + result.DeletedCount);
        }

        private async Task<Page> GetPageAsync(string url)
        {
            var builder = Builders<Page>.Filter;
            var filter = builder.Eq(x => x.Url, url);
            var pageT = await _collection.Find(filter).FirstOrDefaultAsync();

            if (pageT != null)
            {
                return pageT;
            }

            return null;
        }

        public async Task<Page> GetAsync(string url)
        {
            url = BddJson.NormalizeUrl(url);
            var page = await GetPageAsync(url);

            if (page != null)
            {
                return page;
            }

            page = new Page();
            page.StatusCode = 400;
            page.Url = BddJson.NormalizeUrl(url);

            return page;
        }

    }
}