using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Demo.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Demo.Mvc.Core.Api.Attributes
{
    public class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task Invoke(HttpContext context)
        {

            Exception exception = null;
            try
            {
                await _next.Invoke(context);
            }
            catch (NotAuthentifiedException ex)
            {
                _logger.LogInformation(ex, ex.Message);
                context.Response.StatusCode = 401;
                exception = ex;
            }
            catch (NotAuthorizedException ex)
            {
                _logger.LogInformation(ex, ex.Message);
                context.Response.StatusCode = 403;
                exception = ex;
            }
            catch (Exception ex)
            {
                _logger.LogError(new EventId(3, "Global Message"), ex, ex.Message);
                context.Response.StatusCode = 500;
                exception = ex;
            }

            if (exception == null)
            {
                return;
            }

            if (!context.Response.HasStarted)
            {
                if (context.Request.ContentType == "application/json")
                {
                    context.Response.ContentType = "application/json";
                    var json = JsonConvert.SerializeObject(exception, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                    await context.Response.WriteAsync(json);
                }
                else
                {
                    await context.Response.WriteAsync(exception.ToString());
                }
            }
        }
    }

    public static class ExceptionExtensions
    {
        public static IApplicationBuilder ExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}

