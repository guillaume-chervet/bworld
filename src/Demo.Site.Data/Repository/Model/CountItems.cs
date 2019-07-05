using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Data.Repository.Model
{
    public class CountItems
    {
        public string SiteId { get; set; }

        public CountItemFilters CountItemFilters { get; set; }
    }
}
