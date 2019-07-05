using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Demo.Domain.Domain
{
    public class DomainService : ServiceBase
    {

        public DomainService(IOptions<DomainConfig> options) : base(options)
        {
        }
        public async Task<string> GetVersionAsync()
        {
            var requestTemplate = @"
   <methodCall>
    <methodName>version.info</methodName>
    <params>
      <param>
         <value><string>§model.ApiKey§</string></value>
         </param>
      </params>
   </methodCall>";

            var model = new ModelBase {ApiKey = _domainConfig.ApiKey};
            var request = TemplateRenderer.Render(requestTemplate, model);
            var response = await SendRequestAsync(request);
            return response;
        }

        public async Task<AvailableResult> AvailableAsync(AvailableInput input)
        {
            var requestTemplate = @"
<methodCall>
   <methodName>domain.available</methodName>
   <params>
      <param>
 <value><string>§model.ApiKey§</string></value>
 </param>
 <param>
<array>
   <data>
        §model.Data.Domains:{it|<value><string>§it§</string></value>}§
      </data>
   </array>
</param>
      </params>
   </methodCall>";

            var model = new ModelBase<AvailableInput>();
            model.ApiKey = _domainConfig.ApiKey;
            model.Data = input;
            var request = TemplateRenderer.Render(requestTemplate, model);
            var response = await SendRequestAsync(request);

            while (response.Contains("pending"))
            {
                await Task.Delay(700);
                response = await SendRequestAsync(request);
            }

            dynamic res = JArray.Parse("[" + response + "]");

            var result = new AvailableResult();
            result.Domains = new Dictionary<string, DomainStatus>();
            //{"?xml":{"@version":"1.0"},
            //"methodResponse":{"params":
            //{ "param":{"value":{"struct":{"member":{"name":"google.fr","value":{"string":"unavailable"}}}}}}}}

            var methodResponse = res.Last;
            var data = methodResponse.methodResponse.@params.param;
            {
                var member = data.value.@struct.member;

                if (member is JArray)
                {
                    foreach (var m in member)
                    {
                        ParseDomain(m, result);
                    }
                }
                else
                {
                    ParseDomain(member, result);
                }
            }

            return result;
        }

        private static void ParseDomain(dynamic test, AvailableResult result)
        {
            var status = (string) test.value.@string;

            DomainStatus domainStatus;
            switch (status)
            {
                case "unavailable":
                    domainStatus = DomainStatus.Unavailable;
                    break;
                case "available":
                    domainStatus = DomainStatus.Available;
                    break;
                default:
                    domainStatus = DomainStatus.Invalid;
                    break;
            }

            string domainName = test.name;
            result.Domains.Add(domainName, domainStatus);
        }

        public async Task<CreateResult> CreateAsync(CreateInput input)
        {
            var requestTemplate = @"
               <methodCall>
                <methodName>domain.create</methodName>
                <params>
                   <param>  <value><string>§model.ApiKey§</string></value></param>
                    <param> <value><string>§model.Data.Domain§</string></value></param>
                  
 <param>  
<struct>
       <member><name>admin</name><value><string>§model.Data.DomainCreate.Admin§</string></value></member>
       <member><name>bill</name><value><string>§model.Data.DomainCreate.Bill§</string></value></member>
       <member><name>owner</name><value><string>§model.Data.DomainCreate.Owner§</string></value></member>
       <member><name>tech</name><value><string>§model.Data.DomainCreate.Tech§</string></value></member>
       <member><name>duration</name><value><int>§model.Data.DomainCreate.Duration§</int></value></member>
   </struct></param>
                  </params>
               </methodCall>";

            var methodResponse = await MethodResponse(requestTemplate, input);
            var result = new CreateResult();
            return result;
        }

        public async Task<DomainReturn> InfoAsync(string domain)
        {
            var requestTemplate = @"
               <methodCall>
                <methodName>contact.info</methodName>
                <params>
                  <param>
                     <value><string>§model.ApiKey§</string></value>
                     </param>
                     <param>§model.Data§</param>
                  </params>
               </methodCall>";
            var model = new ModelBase<string> {ApiKey = _domainConfig.ApiKey};
            model.Data = domain;
            var request = TemplateRenderer.Render(requestTemplate, model);
            var response = await SendRequestAsync(request);

            return new DomainReturn();
        }

       
    }
}