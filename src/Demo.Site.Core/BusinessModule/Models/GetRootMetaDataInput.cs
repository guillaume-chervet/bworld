using Demo.Data;
using Demo.Data.Model;

namespace Demo.Business.BusinessModule.Models
{
    public class GetRootMetaDataInput
    {
        public IDataFactory DataFactory { get; set; }
        public ItemDataModel ItemDataModel { get; set; }
    }
}