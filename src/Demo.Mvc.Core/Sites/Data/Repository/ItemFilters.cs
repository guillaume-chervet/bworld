using System.Collections.Generic;

namespace Demo.Mvc.Core.Sites.Data.Repository
{
    public class ItemFilters : CountItemFilters
    {
        public bool? _SortAscending = true;
        public bool HasTracking { get; set; }

        public bool SortAscending
        {
            get { return _SortAscending ?? false; }
            set { _SortAscending = value; }
        }

        public bool SortDescending
        {
            get { return !_SortAscending ?? false; }
            set { _SortAscending = !value; }
        }

        public int? Limit { get; set; }

        public IList<string> ExcludedModules { get; set; }
        public IList<string> Tags { get; set; }

    }
}