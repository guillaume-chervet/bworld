using System.Collections.Generic;
using Demo.Mvc.Core.Sites.Data.Model;

namespace Demo.Mvc.Core.Sites.Data
{
    public class MemorySession<T> where T : ItemDataModelBase
    {
        public IList<T> DatabaseLoaded { get; } = new List< T>();
        public IList<T> DatabaseAdd { get; } = new List<T>();
        public IList<T> DatabaseDelete { get; } = new List<T>();
    }
}