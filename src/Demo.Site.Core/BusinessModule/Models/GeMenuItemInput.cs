using System.Collections.Generic;
using Demo.Business.Routing;
using Demo.Data;
using Demo.Data.Model;

namespace Demo.Business.BusinessModule.Models
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