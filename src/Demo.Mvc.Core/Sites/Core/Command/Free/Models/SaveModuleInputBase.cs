namespace Demo.Mvc.Core.Sites.Core.Command.Free.Models
{
    public class SaveModuleInputBase
    {
        public string ModuleId { get; set; }
        public string ParentId { get; set; }
        public string PropertyName { get; set; }
        public CurrentRequest Site { get; set; }
    }
}