using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Domain.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Domain.Tests
{
    [TestClass]
    public class DomainServiceUnitTest
    {
       // private readonly DomainService domainService = new DomainService(new DomainConfig());
        /*  public void Talk()
        {
            string xmlResult = null;

            string botId = "c"; // enter your botid
            string talk = "Am I a human?";
            string custId = null; // (or a value )
            using (var wc = new WebClient())
            {
                var col = new NameValueCollection();

                col.Add("botid", botId);
                col.Add("input", talk);
                if (!String.IsNullOrEmpty(custId))
                {
                    col.Add("custid", custId);
                }

                //https://rpc.gandi.net/xmlrpc/

                byte[] xmlResultBytes = wc.UploadValues(
                    @"http://www.pandorabots.com/pandora/talk-xml",
                    "POST",
                    col);
                xmlResult = UTF8Encoding.UTF8.GetString(xmlResultBytes);
 
            }

            //raw result
            Console.WriteLine(xmlResult);

        }*/


       /* [TestMethod]
        public async Task TestMethodGetVersion()
        {
            var version = await domainService.GetVersionAsync();
        }

        [TestMethod]
        public async Task TestMethodDomainAvailable()
        {
            var input = new AvailableInput();
            input.Domains = new List<string>();
            input.Domains.Add("google.fr");

            var isAvailable1 = await domainService.AvailableAsync(input);

            input.Domains.Add("www.qsdqsdarvabett.fr");
            input.Domains.Add("youhouhtotototop.fr");

            var isAvailable2 = await domainService.AvailableAsync(input);
        }

        [TestMethod]
        public async Task TestMethodDomainInfo()
        {
            var isAvailable2 = await domainService.InfoAsync("www.guillaume-chervet.fr");
        }

        [TestMethod]
        public async Task TestMethodDomainCreate()
        {
            var input = new CreateInput();
            input.Domain = "sdsdsdsdze.fr";
            input.DomainCreate = new DomainCreate
            {
                Owner = "FLN1108-GANDI",
                Tech = "GC17-GANDI",
                Admin = "GC17-GANDI",
                Bill = "GC17-GANDI",
                Duration = 2
            };


            var isAvailable2 = await domainService.CreateAsync(input);
        }*/
    }
}