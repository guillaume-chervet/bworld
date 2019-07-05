using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Demo.Domain.Domain;
using Demo.Renderer;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Demo.Domain
{
    public class ServiceBase
    {
        protected readonly DomainConfig _domainConfig;
        protected readonly StringTemplateRenderer TemplateRenderer = new StringTemplateRenderer();

        public ServiceBase(IOptions<DomainConfig> options)
        {
            _domainConfig = options.Value;
        }

        protected async Task<string> SendRequestAsync(string request)
        {
            try
            {
                using (var wc = new WebClient())
                {
                    wc.Encoding = new UTF8Encoding();
                    wc.Headers.Add("Content-Type", "text/xml");
                    wc.Headers["User-Agent"] = "bworld";
                    var header = "<?xml version=\"1.0\"?>";
                    request = header + "\n\r" + request;
                    var xmlResult = await wc.UploadStringTaskAsync(_domainConfig.ApiUrl,
                        "POST", request);

                    var doc = new XmlDocument();
                    doc.LoadXml(xmlResult);
                    var json = JsonConvert.SerializeXmlNode(doc);
                    return json;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw;
            }
        }

        protected async Task<dynamic> MethodResponse<T>(string requestTemplate, T data)
        {
            var model = new ModelBase<T> {ApiKey = _domainConfig.ApiKey};
            model.Data = data;
            var request = TemplateRenderer.Render(requestTemplate, model);
            var response = await SendRequestAsync(request);

            if (response.Contains("faultCode"))
            {
                throw new ArgumentException(response);
            }

            dynamic res = JArray.Parse("[" + response + "]");
            var methodResponse = res.Last;
            return methodResponse;
        }
    }
}