namespace Demo.Business.Renderers
{
    public class MessageReceiverMailModel
    {
        public string SiteUrl { get; set; }
        public string UserName { get; set; }
        public string SiteName { get; set; }
        public SenderModel Sender { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public string MessageUrl { get; set; }
        public bool IsReply { get; set; }
    }

    public class SenderModel
    {
        public string FullName { get; set; }
        public bool IsNotAuthenticated { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

}