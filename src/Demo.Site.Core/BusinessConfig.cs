using Demo.Business.Routing;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Demo.Business
{
    public class BusinessConfig
    {
        public ICollection<DomainSettings> Domains { get; set; }
    }
}