using System.Collections.Generic;
using Demo.Data.Model;

namespace Demo.Data.Repository
{
    public class CountItemFilters
    {
        public int? IndexGt { get; set; }
        public int? IndexLt { get; set; }
        public int? IndexLte { get; set; }
        public string ParentId { get; set; }
        public string Module { get; set; }
        public bool? IsTemporary { get; set; }

        public IList<ItemState> States { get; set; }
        public string PropertyName { get; set; }
        public int? IndexGte { get; set; }
        public IList<string> Tags { get; set; }
    }
}