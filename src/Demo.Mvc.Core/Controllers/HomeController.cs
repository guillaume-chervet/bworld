using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Demo.Business;
using Demo.Business.BusinessModule;
using Demo.Business.Command;
using Demo.Business.Command.Site.Cache;
using Demo.Business.Command.Site.Seo;
using Demo.Common.Command;
using Demo.Data.Model;
using Demo.Mvc.Core.Controllers.Models;
using Demo.Routing.Extentions;
using Demo.Routing.Interfaces;
using Demo.Routing.Models;
using Demo.Seo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Demo.Mvc.Core.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly ApplicationConfig _applicationConfig;

        private readonly GetSeoCommand _getSeoCommand;
        private readonly ILogger<HomeController> _logger;
        private readonly ModuleManager _moduleManager;
        private readonly ResetSiteCacheCommand _resetSiteCacheCommand;
        private readonly IRouteManager _routeManager;

        private readonly SeoService _seoService;
        //
        // GET: /Home/

        public HomeController(BusinessFactory business, IRouteManager routeManager, ModuleManager moduleManager,
            GetSeoCommand getSeoCommand, SeoService seoService, ResetSiteCacheCommand resetSiteCacheCommand,
            IOptions<ApplicationConfig> options, ILogger<HomeController> logger)
            : base(business)
        {
            _routeManager = routeManager;
            _moduleManager = moduleManager;
            _seoService = seoService;
            _resetSiteCacheCommand = resetSiteCacheCommand;
            _logger = logger;
            _applicationConfig = options.Value;
            _getSeoCommand = getSeoCommand;
        }

        public async Task<ActionResult> Index()
        {
            var requestedDomain = $"{Request.Scheme}://{Request.Host.Host}";
            string applicationPath = Request.PathBase;
            string requestedPath = Request.Path;
            var fullRequestUrl = UriHelper.BuildAbsolute(Request.Scheme, Request.Host, Request.PathBase, Request.Path,
                Request.QueryString);

            var isSecure = Request.IsHttps;
            var fullUrl = UrlHelper.Concat(requestedDomain, applicationPath, requestedPath);

            foreach (var key in Request.Query)
                // on enleve les querry facebook
                if (key.Key.StartsWith("fb_"))
                    return Redirect("/");
            // Logger.Default.Info(String.Concat("URL:", fullRequestUrl, " - UserAgent:", Request.UserAgent));

            var userAgent = Request.Headers["User-Agent"];
            ViewBag.Version = _applicationConfig.Version;
            ViewBag.MainDomainUrl = _applicationConfig.MainDomainUrl;
            ViewBag.IsDebug = _applicationConfig.IsDebug;

            // If the request is not from a bot => control goes to 
            if (IsReturnNormalView(fullUrl, userAgent)) return await NormalView(fullUrl, fullRequestUrl, isSecure);

            // If the request contains the _escaped_fragment_, then we return an HTML Snapshot to the bot
            //We�ll crawl the normal url without _escaped_fragment_
            //var html = Crawl(Request.Url.AbsoluteUri.Replace("?_escaped_fragment_=", "#!"));
            var url = fullRequestUrl.Replace("?_escaped_fragment_=", "");
            var response = await _seoService.GetHtmlAsync(url);

            if (response.StatusCode == 200)
            {
                Response.ContentType = "text/html";
                ViewBag.Content = response.Text;
                return View("IndexServerSideRendering");
            }

            if (response.StatusCode == 400)
            {
                _logger.LogWarning(string.Concat("STATUS CODE: " + response.StatusCode + " SEO URL:", fullRequestUrl,
                    " - UserAgent:", userAgent));
                return await NormalView(fullUrl, fullRequestUrl, isSecure);
            }

            _logger.LogWarning(string.Concat("STATUS CODE: " + response.StatusCode + "SEO URL:", fullRequestUrl,
                " - UserAgent:", userAgent));
            Response.ContentType = "text/html";
            HttpContext.Response.StatusCode = response.StatusCode;
            return View("IndexServerSideRendering");
        }

        private async Task<ActionResult> NormalView(string fullUrl, string fullRequestUrl, bool isSecure)
        {
            var findRouteInput = new FindRouteInput();
            findRouteInput.Url = fullUrl.Replace("https://", "").Replace("http://", "");
            findRouteInput.FullUrl = fullRequestUrl;
            findRouteInput.IsSecure = isSecure;
            findRouteInput.Port = Request.Host.Port.ToString();

            var findRouteResult = await _routeManager.FindRouteAsync(findRouteInput);

            if (!findRouteResult.IsSuccess)
            {
                if (findRouteResult.HttpCode == "301") return RedirectPermanent(findRouteResult.RedirectionUrl);

                Response.StatusCode = 404;
                return View("404");
            }

            var currentRequest = new CurrentRequest();

            var siteId = findRouteResult.SiteId;
            currentRequest.SiteId = siteId;
            currentRequest.DomainId = findRouteResult.DomainId;
            currentRequest.DomainDatas = new Dictionary<string, string>();
            currentRequest.IsSecure = isSecure;
            currentRequest.Port = findRouteInput.Port;
            if (findRouteResult.DomainDatas != null)
                foreach (var domainData in findRouteResult.DomainDatas)
                    currentRequest.DomainDatas.Add(domainData);

            // ICurrentRequest
            if (findRouteResult.Datas["controller"] == "Seo")
            {
                if (findRouteResult.Datas["action"] == "Sitemap")
                {
                    var sitemap = await _moduleManager.GetSitemapAsync(currentRequest);
                    var elements = SiteMapElements(fullRequestUrl, findRouteResult, sitemap);

                    Response.ContentType = "text/xml";
                    return View("SiteMap", elements);
                }

                var userInput = new UserInput<GetSeoInput>
                {
                    UserId = string.Empty,
                    Data = new GetSeoInput {SiteId = siteId, IsVerifyAuthorisation = false}
                };

                var seoResult =
                    await
                        Business.InvokeAsync<GetSeoCommand, UserInput<GetSeoInput>, CommandResult<SeoBusinessModel>>(
                            _getSeoCommand, userInput);
                if (findRouteResult.Datas["action"] == "Robots")
                {
                    Response.ContentType = "text/txt";
                    ViewBag.Disallows = seoResult.Data.Disallows;
                    return View("Robots");
                }

                if (findRouteResult.Datas["action"] == "BingSiteAuth")
                {
                    var meta = seoResult.Data.Metas.FirstOrDefault(m => m.Engine == SeoEngine.Bing);
                    if (meta != null && !string.IsNullOrEmpty(meta.Code))
                    {
                        ViewBag.Code = meta.Code;
                        return View("BingSiteAuth");
                    }
                }

                if (findRouteResult.Datas["action"] == "GoogleAuth")
                {
                    var meta = seoResult.Data.Metas.FirstOrDefault(m => m.Engine == SeoEngine.Google);
                    if (meta != null && !string.IsNullOrEmpty(meta.Code))
                    {
                        ViewBag.Code = meta.Code;
                        return View("GoogleAuth");
                    }
                }

                return View("404");
            }

            dynamic master;
            try
            {
                master = await _moduleManager.GetMasterAsync(currentRequest);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "This catch should be remove");
                master =
                    await
                        Business.InvokeAsync<ResetSiteCacheCommand, ResetSiteCacheInput, CommandResult<dynamic>>(
                            _resetSiteCacheCommand,
                            new ResetSiteCacheInput {Site = currentRequest});
            }

            var jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            dynamic masterJson = null;

            if (_applicationConfig.MinifyHtml)
                masterJson = JsonConvert.SerializeObject(master, Formatting.None, jsonSerializerSettings);
            else
                masterJson = JsonConvert.SerializeObject(master, Formatting.Indented, jsonSerializerSettings);

            ViewBag.MasterJson = masterJson;

            if (Request.Query["display_menu"].Count > 0)
                ViewBag.IsDisplayMenu = false;
            else
                ViewBag.IsDisplayMenu = true;

            if (Request.Query["font_size"].Count > 0)
                ViewBag.FontSize = Request.Query["font_size"].FirstOrDefault();
            else
                ViewBag.FontSize = null;

            ViewBag.Header = GetHeader(findRouteResult, master, Request, fullRequestUrl);
            ViewBag.Hash = _applicationConfig.Hash;

            string[] filePaths = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "ClientApp", "build", "static", "js"), "main.*",
                    SearchOption.TopDirectoryOnly);

            ViewBag.Script = "/static/js/" + Path.GetFileName(filePaths[0]);
            
            return View("Index");
        }

        private bool IsReturnNormalView(string absoluteUri, string userAgent)
        {
            var url = absoluteUri.ToLower();
            if (url.EndsWith("robots.txt") || url.EndsWith("sitemap.xml")) return true;

            // Bing et google
            if (!string.IsNullOrEmpty(userAgent))
            {
                userAgent = userAgent.ToLower();
                if (userAgent.Contains("bingbot") || userAgent.Contains("msnbot") || userAgent.Contains("googlebot")
                ) return false;
            }

            // Google
            if (Request.QueryString.Value.Contains("_escaped_fragment_")) return false;

            return true;
        }

        private string GetTheme(dynamic master)
        {
            var masterModel = (IDictionary<string, object>) CacheProvider.ToExpando(master.Master);
            if (masterModel.ContainsKey("Theme"))
            {
                var theme = (string) masterModel["Theme"];
                if (!string.IsNullOrEmpty(theme)) return theme;
            }

            return "default";
        }

        public static Header GetHeader(FindRouteResult findRouteResult, dynamic master, HttpRequest request,
            string fullRequestUrl)
        {
            var masterModel = master.Master;
            var header = new Header();
            header.FacebookAppId = masterModel.FacebookAuthenticationAppId;
            header.SiteName = masterModel.Title;

            header.Style = masterModel.StyleTemplate;

            var baseUrl = UriHelper.BuildAbsolute(request.Scheme, request.Host).TrimEnd('/');
            header.BaseUrl = baseUrl;

            var siteId = master.Site.SiteId;
            header.FullUrl = fullRequestUrl;
            header.LogoUrl =
                $@"{baseUrl}/api/file/get/{siteId}/{masterModel.ImageLogoId}/{"ImageThumb"}/{
                        UrlHelper.NormalizeTextForUrl(masterModel.ImageLogoFileName)
                    }";
            header.IconeUrl =
                $@"{baseUrl}/api/file/get/{siteId}/{masterModel.ImageIconeId}/{"ImageThumb"}/{
                        UrlHelper.NormalizeTextForUrl(masterModel.ImageIconeFileName)
                    }";
            var baseUrlSite = fullRequestUrl.Replace(baseUrl, "");
            if (!string.IsNullOrEmpty(findRouteResult.Path))
                baseUrlSite = baseUrlSite.Replace(findRouteResult.Path, "");
            if (request.QueryString.HasValue) baseUrlSite = baseUrlSite.Replace(request.QueryString.Value, "");
            if (string.IsNullOrEmpty(baseUrlSite)) baseUrlSite = "/";
            if (!baseUrlSite.StartsWith("/")) baseUrlSite = string.Concat("/", baseUrlSite);
            if (!baseUrlSite.EndsWith("/")) baseUrlSite = string.Concat(baseUrlSite, "/");
            header.BaseUrlSite = baseUrlSite;

            var masterDictionnary = (IDictionary<string, object>) master;
            InitHeader(findRouteResult, master, masterDictionnary, header, baseUrl);
            return header;
        }

        private static Header InitHeader(FindRouteResult findRouteResult, dynamic master,
            IDictionary<string, object> masterDictionnary,
            Header header, string baseUrl)
        {
            foreach (var key in masterDictionnary.Keys)
                if (key.ToLower().EndsWith("menuitems"))
                {
                    dynamic menuItems = masterDictionnary[key] as ArrayList;

                    if (menuItems == null) menuItems = masterDictionnary[key] as List<object>;

                    if (menuItems != null)
                        foreach (var menuItem in menuItems)
                        {
                            var menuItemExpendo = CacheProvider.ToExpando(menuItem);
                            var dico = (IDictionary<string, object>) menuItemExpendo;
                            var resultHeader = InitHeader(findRouteResult, master, dico, header, baseUrl);
                            if (resultHeader != null) return resultHeader;

                            var moduleId = string.Empty;
                            if (findRouteResult.Datas.ContainsKey("moduleId"))
                                moduleId = findRouteResult.Datas["moduleId"];

                            if (findRouteResult.Datas.ContainsKey("moduleid"))
                                moduleId = findRouteResult.Datas["moduleid"];
                            if (moduleId == menuItem.ModuleId)
                            {
                                header.Title = string.Concat(master.Master.Title, " ", menuItem.Title);
                                var seo = CacheProvider.ToExpando(menuItem.Seo) as IDictionary<string, object>;
                                if (seo != null)
                                {
                                    if (seo.ContainsKey("MetaDescription") &&
                                        !string.IsNullOrEmpty(menuItem.Seo.MetaDescription))
                                        header.MetaDescription = menuItem.Seo.MetaDescription;

                                    if (seo.ContainsKey("MetaKeywords") &&
                                        !string.IsNullOrEmpty(menuItem.Seo.MetaKeywords))
                                        header.MetaKeywords = menuItem.Seo.MetaKeywords;

                                    if (seo.ContainsKey("SocialImageUrl") &&
                                        !string.IsNullOrEmpty(menuItem.Seo.SocialImageUrl))
                                        header.LogoUrl = UrlHelper.Concat(baseUrl, menuItem.Seo.SocialImageUrl);
                                }

                                return header;
                            }
                        }
                }


            return null;
        }

        private static List<SiteMapElement> SiteMapElements(string fullRequestUrl, FindRouteResult result,
            dynamic master)
        {
            var websiteUrl = fullRequestUrl.Replace(result.Path, string.Empty);
            var elements = new List<SiteMapElement>();

            var masterDictionnary = (IDictionary<string, object>) master;

            InitSitemap(masterDictionnary, websiteUrl, elements);
            return elements;
        }
        private static void InitSitemap(IDictionary<string, object> masterDictionnary, string websiteUrl,
            List<SiteMapElement> elements)
        {
            foreach (var key in masterDictionnary.Keys)
                if (key.ToLower().EndsWith("menuitems"))
                {
                    dynamic menuItems = masterDictionnary[key] as ArrayList;

                    if (menuItems == null) menuItems = masterDictionnary[key] as List<object>;

                    if (menuItems != null)
                        foreach (var menuItem in menuItems)
                        {
                            var menuItemExpendo = CacheProvider.ToExpando(menuItem);
                            var dico = (IDictionary<string, object>) menuItemExpendo;
                            if (dico.ContainsKey("State") && (System.Int64)dico["State"] != (System.Int64)ItemState.Published)
                            {
                                continue;
                            }

                            InitSitemap(dico, websiteUrl, elements);

                            var updateDate = menuItem.Seo.UpdateDate;
                            var element = new SiteMapElement
                            {
                                Location = UrlHelper.Concat(websiteUrl, "/", menuItem.RoutePath),
                                Priority = "0.5",
                                DateUpdate = updateDate.ToString("yyyy-MM-dd"),
                                Frequence = menuItem.Seo.SitemapFrequence.ToString().ToLower()
                            };
                            elements.Add(element);
                        }
                }
        }
    }
}