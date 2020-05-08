using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.Sites.Data.Repository;

namespace Demo.Mvc.Core.Sites.Data
{
    public class DataFactoryMock : IDataFactory
    {
        private readonly MemorySession<ItemDataModelBase> memorySession = new MemorySession<ItemDataModelBase>();

        public DataFactoryMock()
        {
            memorySession.DatabaseLoaded.Clear();
        }

        public IItemRepository ItemRepository
        {
            get { return new ItemRepositoryMock(memorySession); }
        }

        public CacheRepository CacheRepository { get; private set; }

        void IDataFactory.Add<T>(T o)
        {
            throw new NotImplementedException();
        }
       

        public Task DeleteFileAsync(string id)
        {
            throw new NotImplementedException();
        }

        Task IDataFactory.DeleteAsync<T>(T o)
        {
            throw new NotImplementedException();
        }

        Task IDataFactory.DeleteAsync<T>(string siteId, string id)
        {
            throw new NotImplementedException();
        }

        Task IDataFactory.DeleteAsync<T>(ICollection<T> list)
        {
            throw new NotImplementedException();
        }

        Task IDataFactory.SaveChangeAsync()
        {
            throw new NotImplementedException();
        }

        public Task UploadAsync(FileDataModel fileDataModel)
        {
            throw new NotImplementedException();
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        IItemRepository IDataFactory.ItemRepository
        {
            get { throw new NotImplementedException(); }
        }

        ICacheRepository IDataFactory.CacheRepository
        {
            get { throw new NotImplementedException(); }
        }

        public void Add<T>(T o) where T : DataModelBase
        {
            memorySession.DatabaseAdd.Add(o as ItemDataModel);
        }

        public void BeginTransaction()
        {
        }

        public void CommitTransaction()
        {
        }

        public void Delete<T>(T o) where T : DataModelBase
        {
            var itemDataModel = o as ItemDataModel;
            memorySession.DatabaseDelete.Add(itemDataModel);
        }

        public async Task Delete<T>(string siteId, string id) where T : DataModelBase
        {
            var obj = await ItemRepository.GetItemAsync(siteId, id);
            Delete(obj);
        }

        public void Delete<T>(ICollection<T> list) where T : DataModelBase
        {
            foreach (var element in list)
            {
                Delete(element);
            }
        }

        public void Dispose()
        {
        }

        public void RollBackTransaction()
        {
            memorySession.DatabaseDelete.Clear();
            memorySession.DatabaseAdd.Clear();
            memorySession.DatabaseLoaded.Clear();
        }

        public void SaveChange()
        {
            MemoryDatabase.SaveChange(memorySession);
        }
    }
}