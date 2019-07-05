using Demo.Data;

namespace Demo.Business.BusinessModule.Models
{
    public class CreateNewInput
    {
        public IDataFactory DataFactory { get; set; }
        public SiteBusinessModel SiteDataModel { get; set; }
        public int IndexMenuItem { get; set; }
        public long CultureId { get; set; }
    }
}