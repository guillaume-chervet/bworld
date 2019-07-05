using System.Collections.Generic;

namespace Demo.Domain.Domain
{
    public class AvailableResult
    {
        public IDictionary<string, DomainStatus> Domains { get; set; }
    }
}