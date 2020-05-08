using Demo.Mvc.Core.Sites.Data;

namespace Demo.Mvc.Core.Sites.Core.BusinessModule.Models
{
    public class CreateNewInput
    {
        public IDataFactory DataFactory { get; set; }
        public SiteBusinessModel SiteDataModel { get; set; }
        public int IndexMenuItem { get; set; }
        public long CultureId { get; set; }
    }
}