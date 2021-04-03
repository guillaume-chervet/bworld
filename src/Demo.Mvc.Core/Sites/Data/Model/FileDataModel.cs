namespace Demo.Mvc.Core.Sites.Data.Model
{
    public class FileDataModel : ItemDataModelBase
    {
        public FileDataModel()
        {
            FileData = new FileDataInfo();
        }

        public FileDataInfo FileData { get; set; }
    }
}