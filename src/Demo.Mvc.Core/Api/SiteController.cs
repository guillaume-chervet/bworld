using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Business;
using Demo.Business.BusinessModule;
using Demo.Business.Command;
using Demo.Business.Command.Site;
using Demo.Business.Command.Site.Cache;
using Demo.Business.Command.Site.Master;
using Demo.Common.Command;
using Demo.Mvc.Core.Api.Extentions;
using Demo.Mvc.Core.Controllers;
using Demo.Mvc.Core.Controllers.Models;
using Demo.Routing.Extentions;
using Demo.Routing.Interfaces;
using Demo.Routing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Demo.Mvc.Core.Api
{
    public class SiteController : ApiControllerBase
    {
        private readonly ILogger<SiteController> _logger;

        public SiteController(BusinessFactory business, ILogger<SiteController> logger)
            : base(business)
        {
            _logger = logger;
        }


        [HttpGet]
        [ResponseCache(Duration = 0)]
        [Route("api/site/load/{siteId}/{moduleId}")]
        public async Task<CommandResult> Load([FromServices]LoadAddSiteCommand _loadAddSiteCommand, string siteId, string moduleId)
        {
            var result =
                await Business.InvokeAsync<LoadAddSiteCommand, LoadAddSiteInput, CommandResult<dynamic>>(
                    _loadAddSiteCommand, new LoadAddSiteInput {ModuleId = moduleId, SiteId = siteId});
            return result;
        }


        [HttpPost]
        [Route("api/site/check")]
        public async Task<CommandResult<dynamic>> Check([FromServices]CheckAddSiteCommand _checkAddSiteCommand, [FromBody] CheckAddSiteInput checkAddSiteInput)
        {
            var result =
                await Business.InvokeAsync<CheckAddSiteCommand, CheckAddSiteInput, CommandResult<dynamic>>(
                    _checkAddSiteCommand, checkAddSiteInput);

            if (result.IsSuccess)
            {
                var authentificationConfig = new ApplicationConfig();

                var cookie = new CookieOptions();
                //var cookie = new CookieHeaderValue("add-site", JsonConvert.SerializeObject(checkAddSiteInput));
                cookie.Expires = DateTimeOffset.Now.AddDays(1);
                cookie.Domain = authentificationConfig.CookieDomain;
                cookie.Path = "/";
                Response.Cookies.Append("add-site", JsonConvert.SerializeObject(checkAddSiteInput), cookie);
            }

            return result;
        }


        [HttpPost]
        [Authorize]
        [Route("api/site/add")]
        public async Task<CommandResult<dynamic>> Add([FromServices]AddSiteCommand _addSiteCommand, [FromBody] AddSiteInput addSiteInput)
        {
            var userId = User != null && User.Identity != null ? User.GetUserId() : string.Empty;
            var userInput = new UserInput<AddSiteInput>
            {
                Data = addSiteInput,
                UserId = userId
            };

            var result =
                await Business.InvokeAsync<AddSiteCommand, UserInput<AddSiteInput>, CommandResult<dynamic>>(
                    _addSiteCommand, userInput);

            return result;
        }
        
        [Authorize]
        [HttpPost]
        [Route("api/site/saveaddsite")]
        public async Task<CommandResult> SaveAddSite([FromServices]SaveAddSiteCommand _saveAddSiteCommand, [FromBody] SaveAddSiteInput addSiteInput)
        {
            var userInput = new UserInput<SaveAddSiteInput>
            {
                Data = addSiteInput,
                UserId = User.GetUserId()
            };

            var result = await
                Business.InvokeAsync<SaveAddSiteCommand, UserInput<SaveAddSiteInput>, CommandResult<dynamic>>(
                    _saveAddSiteCommand, userInput);

            return result;
        }

        // [AntiForgeryValidate]
        [Authorize]
        [HttpPost]
        [Route("api/site/save")]
        public async Task<CommandResult> Save([FromServices]SaveMasterCommand _saveMasterCommand, [FromBody] SaveMasterInput saveMasterInput)
        {
            var userInput = new UserInput<SaveMasterInput>
            {
                Data = saveMasterInput,
                UserId = User.GetUserId()
            };

            var result = await
                Business.InvokeAsync<SaveMasterCommand, UserInput<SaveMasterInput>, CommandResult<dynamic>>(
                    _saveMasterCommand, userInput);
            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("api/site/get/{siteId}")]
        public async Task<CommandResult> Get([FromServices]GetMasterCommand _getMasterCommand,string siteId)
        {
            var userInput = new UserInput<string>
            {
                Data = siteId,
                UserId = User.GetUserId()
            };

            var result = await
                Business.InvokeAsync<GetMasterCommand, UserInput<string>, CommandResult<MasterBusinessModel>>(
                    _getMasterCommand, userInput);

            return result;
        }

        [Authorize]
        [HttpGet]
        [Route("api/site/getsites")]
        public async Task<CommandResult> GetSites([FromServices]GetSitesCommand _getSitesCommand)
        {
            var userInput = new UserInput<string>
            {
                Data = "",
                UserId = User.GetUserId()
            };

            var result = await
                Business.InvokeAsync<GetSitesCommand, UserInput<string>, CommandResult<IList<GetSitesResult>>>(
                    _getSitesCommand, userInput);

            return result;
        }
        
        [Authorize]
        [HttpDelete]
        [Route("api/site/delete/{siteId}")]
        public async Task<CommandResult> DeleteSite([FromServices]DeleteSiteCommand _deleteSiteCommand, string siteId)
        {
            var userInput = new UserInput<string>
            {
                Data = siteId,
                UserId = User.GetUserId()
            };

            var result =
                await Business.InvokeAsync<DeleteSiteCommand, UserInput<string>, CommandResult>(_deleteSiteCommand,
                    userInput);

            return result;
        }

        [Authorize]

        [HttpGet]
        [Route("api/site/clearcache")]
        public async Task<CommandResult> ClearCache([FromServices] ClearCacheCommand _clearCacheCommand)
        {
            var userInput = new UserInput<string>
            {
                UserId = ""//User.GetUserId()
            };

            var result =
                await Business.InvokeAsync<ClearCacheCommand, UserInput<string>, CommandResult>(_clearCacheCommand,
                    userInput);

            return result;
        }
       /* [HttpGet]
                [Route("api/site/resetcache")]
                public async Task<CommandResult> ClearCache([FromServices] ClearCacheCommand _clearCacheCommand)
                {
                    var userInput = new UserInput<string>
                    {
                        UserId = ""//User.GetUserId()
                    };
        
                    var result =
                        await Business.InvokeAsync<ClearCacheCommand, UserInput<string>, CommandResult>(_clearCacheCommand,
                            userInput);
        
                    return result;
                }*/

        [HttpGet]
        [ResponseCache(Duration = 0)]
        [Route("api/site/master")]
        public async Task<ActionResult<BaseParameters>> Master([FromServices] ModuleManager moduleManager,[FromServices] ResetSiteCacheCommand resetSiteCacheCommand,[FromServices]IRouteManager routeManager, [FromServices]IOptions<ApplicationConfig> options,[FromQuery]string url, [FromQuery] string port="" )
        {
            var fullRequestUrl = url;
            var fullUrl = fullRequestUrl.Split('?')[0];
            if (fullUrl.Contains(":"))
            {
                fullUrl = fullUrl.Replace($":{port}", "");
            }

            var parameters = new BaseParameters();
            var simpleUrl =  fullUrl.Replace("https://", "").Replace("http://", "");
            var findRouteInput = new FindRouteInput();
            findRouteInput.Url = simpleUrl;
            findRouteInput.FullUrl = fullRequestUrl;
            findRouteInput.IsSecure = true;
            findRouteInput.Port = Request.Host.Port.ToString();

            var findRouteResult = await routeManager.FindRouteAsync(findRouteInput);

            if (!findRouteResult.IsSuccess)
            {
                return NotFound();
            }

            var currentRequest = new CurrentRequest();

            var siteId = findRouteResult.SiteId;
            currentRequest.SiteId = siteId;
            currentRequest.DomainId = findRouteResult.DomainId;
            currentRequest.DomainDatas = new Dictionary<string, string>();
            currentRequest.IsSecure = true;
            currentRequest.Port = findRouteInput.Port;
            if (findRouteResult.DomainDatas != null)
                foreach (var domainData in findRouteResult.DomainDatas)
                    currentRequest.DomainDatas.Add(domainData);
            
            dynamic master;
            try
            {
                master = await moduleManager.GetMasterAsync(currentRequest);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "This catch should be remove");
                master =
                    await
                        Business.InvokeAsync<ResetSiteCacheCommand, ResetSiteCacheInput, CommandResult<dynamic>>(
                            resetSiteCacheCommand,
                            new ResetSiteCacheInput {Site = currentRequest});
            }

            var value = options.Value;
            var baseUrlJs = value.MainDomainUrl;
            parameters.Master = JsonConvert.SerializeObject(master, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            parameters.Version = value.Version;
            parameters.MainDomainUrl = @UrlHelper.Concat(baseUrlJs);
            parameters.IsDebug = value.IsDebug;
            parameters.BaseUrlJs = baseUrlJs;
            parameters.Header = HomeController.GetHeader(findRouteResult, master, Request, fullRequestUrl);
            parameters.BaseUrlSite = parameters.Header.BaseUrlSite;
            return parameters;
        }
    }
    
    public class BaseParameters
    {
        public string Version { get; set; }
        public string BaseUrlJs { get; set; }
        public string BaseUrlSite { get; set; }
        public string MainDomainUrl { get; set; }
        public Header Header {get;set;}
        public dynamic Master { get; set; }
        public bool IsDebug { get; set; }
    }
}