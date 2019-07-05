using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Data.Repository.Model
{
    public class GetItems
    {
        public string SiteId { get; set; }

        public ItemFilters ItemFilters { get; set; }
    }
}
