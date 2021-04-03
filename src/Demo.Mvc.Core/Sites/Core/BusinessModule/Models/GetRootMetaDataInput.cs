using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;

namespace Demo.Mvc.Core.Sites.Core.BusinessModule.Models
{
    public class GetRootMetaDataInput
    {
        public IDataFactory DataFactory { get; set; }
        public ItemDataModel ItemDataModel { get; set; }
    }
}