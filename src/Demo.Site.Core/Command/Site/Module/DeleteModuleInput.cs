namespace Demo.Business.Command.Site.Module
{
    public class DeleteModuleInput
    {
        public string ModuleId { get; set; }
        public CurrentRequest Site { get; set; }
    }
}