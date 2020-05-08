
namespace Demo.Mvc.Core.Sites.Data.Repository.Model
{
    public class GetItem
    {
        public string SiteId { get; set; }
        public string Id { get; set; }
        public bool LoadChilds { get; set; }
        public bool SortAscending { get; set; }

    }
}
