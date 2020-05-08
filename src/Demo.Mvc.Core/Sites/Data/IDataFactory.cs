using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.Sites.Data.Repository;

namespace Demo.Mvc.Core.Sites.Data
{
    public interface IDataFactory : IDisposable
    {
        IItemRepository ItemRepository { get; }
        ICacheRepository CacheRepository { get; }
        void Add<T>(T o) where T : DataModelBase;
        Task DeleteFileAsync(string id);
        Task DeleteAsync<T>(T o) where T : DataModelBase;
        Task DeleteAsync<T>(string siteId, string id) where T : DataModelBase;
        Task DeleteAsync<T>(ICollection<T> list) where T : DataModelBase;
        Task SaveChangeAsync();

    }
}