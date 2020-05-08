namespace Demo.Mvc.Core.UserCore.Site
{
    public class SaveUserResult
    {
        public string Error { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string SiteUserId { get; set; }
        public SendMailAdmin SendMailAdmin { get; set; }
    }
}
