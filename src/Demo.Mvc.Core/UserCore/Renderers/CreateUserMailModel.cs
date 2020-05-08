namespace Demo.Mvc.Core.UserCore.Renderers
{
    public class CreateUserMailModel
    {
        public string UserName { get; set; }
        public string Provider { get; set; }
        public string CallbackUrl { get; set; }
    }
}