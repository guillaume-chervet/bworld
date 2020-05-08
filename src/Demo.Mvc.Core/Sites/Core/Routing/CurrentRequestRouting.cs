using System.Collections.Generic;

namespace Demo.Mvc.Core.Sites.Core.Routing
{
    public class CurrentRequestRouting : ICurrentRequest
    {
        private readonly CurrentRequest currentRequest;

        public CurrentRequestRouting(CurrentRequest currentRequest)
        {
            this.currentRequest = currentRequest;
        }

        public bool IsSecure
        {
            get { return currentRequest.IsSecure; }
        }

        public bool? IsForceSecure
        {
            get { return currentRequest.IsForceSecure; }
        }

        public string SiteId
        {
            get { return currentRequest.SiteId; }
        }

        public string DomainId
        {
            get { return currentRequest.DomainId; }
        }

        public string Port
        {
            get { return currentRequest.Port; }
        }

        public IDictionary<string, string> DomainDatas
        {
            get { return currentRequest.DomainDatas; }
        }
    }
}