using Demo.Business;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Mvc.Core.Api
{
    //[MyCorsPolicy()]
    public class ApiControllerBase : Controller
    {
        public ApiControllerBase(BusinessFactory business)
        {
            Business = business;
        }

        public BusinessFactory Business { get; }

        /*public override async Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)
        {
            if (controllerContext.Request.Headers.AcceptLanguage != null &&
                controllerContext.Request.Headers.AcceptLanguage.Count > 0)
            {
                string language = controllerContext.Request.Headers.AcceptLanguage.First().Value;
                var culture = CultureInfo.CreateSpecificCulture(language);
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }

           return await base.ExecuteAsync(controllerContext, cancellationToken);
        }*/
    }
}