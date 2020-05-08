using System.Collections.Generic;
using Demo.Mvc.Core.Sites.Core.Routing;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;

namespace Demo.Mvc.Core.Sites.Core.BusinessModule.Models
{
    public class GeMenuItemInput
    {
        public IDataFactory DataFactory { get; set; }
        public ItemDataModel ItemDataModel { get; set; }
        public ItemDataModel ParentItemDataModel { get; set; }
        public ICurrentRequest CurrentRequest { get; set; }
        public IDictionary<string, object> Master { get; set; }
        public bool IsSitemap { get; set; }
    }
}