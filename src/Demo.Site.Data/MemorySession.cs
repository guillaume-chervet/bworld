using System.Collections.Generic;
using Demo.Data.Model;

namespace Demo.Data
{
    public class MemorySession<T> where T : ItemDataModelBase
    {
        public IList<T> DatabaseLoaded { get; } = new List< T>();
        public IList<T> DatabaseAdd { get; } = new List<T>();
        public IList<T> DatabaseDelete { get; } = new List<T>();
    }
}