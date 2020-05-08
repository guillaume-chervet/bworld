namespace Demo.Mvc.Core.Sites.Core.Command.Site.Module
{
    public class DeleteModuleInput
    {
        public string ModuleId { get; set; }
        public CurrentRequest Site { get; set; }
    }
}