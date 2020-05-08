using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Demo.Mvc.Core.Routing.Extentions;
using Demo.Mvc.Core.Routing.Implementation;
using Demo.Mvc.Core.Routing.Models;
using Demo.Mvc.Core.Routing.RewriteUrl;
using Demo.Mvc.Core.Sites.Data.Model.Cache;
using Demo.Mvc.Core.Sites.Data.Repository;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.Mvc.Core.Routing
{
    /// <summary>
    ///     Classe qui gère le routage de l'application
    /// </summary>
    public class RouteManager : IRouteManager
    {
        private readonly ICacheRepository _cacheRepository;
        private readonly ILogger<RouteManager> _logger;
        private readonly IRouteProvider _routeProvider;

        public RouteManager(IRouteProvider routeProvider, ICacheRepository cacheRepository,
            ILogger<RouteManager> logger)
        {
            _routeProvider = routeProvider;
            _cacheRepository = cacheRepository;
            _logger = logger;
        }

        /// <summary>
        ///     Récupère la première route qui match avec les parmètres
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<FindRouteResult> FindRouteAsync(FindRouteInput input)
        {
            var result = new FindRouteResult();
            result.IsSuccess = true;

            if (_routeProvider.Domains == null)
            {
                result.IsSuccess = false;
                result.HttpCode = "404";
                return result;
            }

            var url = input.Url;
            if (string.IsNullOrEmpty(url))
            {
                result.IsSuccess = false;
                result.HttpCode = "404";
                return result;
            }

            var datas = new Dictionary<string, string>();
            var domainDatas = new Dictionary<string, string>();

            var cacheResult =
                await _cacheRepository.GetValueAsync<FindRouteResult>(input, CacheRepository.CacheUrlRoutageKey);
            if (cacheResult != null) return cacheResult;

            #region 1: on récupère le domaine et la culture

            var loadDomainResult = await LoadDomainAsync(url, domainDatas, _routeProvider, _logger);
            // On récupère le nom de domaine
            var domain = loadDomainResult.Domain;
            var siteId = loadDomainResult.SiteId;

            // On éjecte les domaines non sécurisé
            if (domain == null)
            {
                result.IsSuccess = false;
                result.HttpCode = "404";
                return result;
            }

            if (domain.SecureMode == SecureMode.Secure && !input.IsSecure)
            {
                ComputeRedirection(input, result);
                return result;
            }

            #endregion

            #region 3: Route

            // On récupère la route associé
            var routePath = GetRoutePath(url, domain, domainDatas);

            var route = await LoadRouteAsync(routePath, datas, _routeProvider, domainDatas, siteId, _logger);
            if (route == null)
            {
                result.IsSuccess = false;
                result.HttpCode = "404";
                return result;
            }

            // on ajoute les valeurs par défaut de la route
            if (route.DefaultValues != null)
                foreach (var defaultValue in route.DefaultValues)
                    if (!datas.ContainsKey(defaultValue.Key))
                        datas.Add(defaultValue.Key, defaultValue.Value);

            #endregion

            if (!string.IsNullOrEmpty(route.Controller))
                SetValue(datas, "controller", route.Controller);

            if (!string.IsNullOrEmpty(route.Action))
                SetValue(datas, "action", route.Action);

            if (!string.IsNullOrEmpty(route.Namespace))
                SetValue(datas, "namespace", route.Namespace);

            // Gestion de la redirection de domain à domain
            if (!string.IsNullOrEmpty(domain.RedirecToDomainId))
                if (!route.CancelRedirect)
                {
                    var findPathInput = new FindPathInput();
                    findPathInput.DomainDatas = domainDatas;
                    findPathInput.DomainId = domain.RedirecToDomainId;
                    findPathInput.IsSecure = null;
                    findPathInput.Port = input.Port;
                    findPathInput.Datas = datas;

                    var resultFindDomainPath = await FindDomainPathAsync(findPathInput);

                    result.IsSuccess = false;
                    result.HttpCode = "301";
                    if (input.FullUrl.StartsWith(_routeProvider.ProtocolDefault))
                        result.RedirectionUrl = resultFindDomainPath.FullUrl;
                }

            if (!string.IsNullOrEmpty(route.RedirectPath))
            {
                result.IsSuccess = false;
                result.HttpCode = "301";
                if (input.FullUrl.StartsWith(_routeProvider.ProtocolDefault))
                    result.RedirectionUrl = route.RedirectPath; //UrlHelper.Concat(domain.Path, route.RedirectPath);
            }

            result.Datas = datas;
            result.DomainDatas = domainDatas;
            result.RouteId = route.Identity;
            result.SiteId = siteId;
            result.DomainId = domain.Id;

            result.Path = routePath;
            result.RewritePath = new Rewriter().Map(routePath, route, datas);
            result.Domain = domain.Path;

            await _cacheRepository.SaveAsync(new CacheItem
            {
                Key = JsonConvert.SerializeObject(input),
                Type = CacheRepository.CacheUrlRoutageKey,
                SiteId = siteId,
                Value = JsonConvert.SerializeObject(result),
                CreateDate = DateTime.Now
            });

            return result;
        }

        /// <summary>
        ///     Retourne le chemin complet de la route et domaine qui match
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<FindPathResult> FindDomainPathAsync(FindPathInput input)
        {
            var findPathResult = new FindPathResult();
            findPathResult.IsSuccess = false;

            if (_routeProvider.Domains == null)
                return findPathResult;

            var datas = new Dictionary<string, string>();
            var datasDomain = new Dictionary<string, string>();

            foreach (var data in input.Datas)
            {
                var key = data.Key;
                datas.Add(key, data.Value);
            }

            //Pour les données de nom de domaine, on persite les valeurs s'il nont pas été redéfinie
            if (input.DomainDatas != null)
                foreach (var domainData in input.DomainDatas)
                {
                    var key = domainData.Key;
                    if (!datasDomain.ContainsKey(key))
                        datasDomain.Add(key, domainData.Value.ToString(CultureInfo.InvariantCulture));
                }

            var domainMasterId = input.MasterDomainId;

            if (string.IsNullOrEmpty(domainMasterId))
                domainMasterId = (from d in _routeProvider.Domains
                    where input.DomainId == d.Id
                    select d.DomainMasterId).FirstOrDefault();

            if (string.IsNullOrEmpty(domainMasterId))
                return findPathResult;

            Route route;
            var routePath = GetRoutePath(_routeProvider, datas, input.DomainDatas, _logger, out route);

            Implementation.Domain domain;
            var isSecure = false;
            if (!string.IsNullOrEmpty(input.DomainId))
            {
                domain = (from d in _routeProvider.Domains
                    where
                        d.Id == input.DomainId
                    select d).FirstOrDefault();
                isSecure = domain.SecureMode == SecureMode.Secure;
                if (domain == null) return findPathResult;
            }
            else
            {
                if (input.IsSecure.HasValue)
                {
                    var secureMode = input.IsSecure.Value ? SecureMode.Secure : SecureMode.NoSecure;
                    isSecure = input.IsSecure.Value;
                    domain = (from d in _routeProvider.Domains
                        where
                            d.DomainMasterId == domainMasterId && d.SecureMode == secureMode &&
                            IsSameDomainType(datasDomain, d)
                        select d).FirstOrDefault();
                    if (domain == null) return findPathResult;
                }
                else
                {
                    domain = (from d in _routeProvider.Domains
                        where
                            d.DomainMasterId == domainMasterId &&
                            IsSameDomainType(datasDomain, d)
                        select d).FirstOrDefault();
                    if (domain == null) return findPathResult;

                    isSecure = domain.SecureMode == SecureMode.Secure;
                }
            }

            var domainPath = GetPath(domain.Path, datasDomain);

            var domainSplitted = domainPath.Split('/');
            var domainLenght = domainSplitted.Length;

            if (domainLenght <= 0)
                return findPathResult;

            var requestDomain = domainSplitted[0];
            var preUrl = domainPath.ReplaceIgnoreCase(requestDomain, string.Empty);

            var applicationPath = string.Empty;
            if (datasDomain.ContainsKey("application_path"))
            {
                applicationPath = UrlHelper.RemoveSeparator(datasDomain["application_path"]);
                preUrl = UrlHelper.RemoveSeparator(preUrl.ReplaceFirstIgnoreCase(applicationPath, string.Empty));
            }

            findPathResult.RoutePathWithoutHomePage = routePath;
            // Gestion de la page d'accueil
            var isHomePage = await IsHomePage(domain, datasDomain, datas);
            if (isHomePage) routePath = string.Empty;

            var path = GetPath(isSecure, requestDomain, applicationPath, preUrl, input.Port,
                routePath, _routeProvider.ProtocolSecure);

            findPathResult.PreUrl = preUrl;
            findPathResult.RequestDomain = requestDomain;
            findPathResult.Path = path;
            findPathResult.Route = route;
            findPathResult.IsSuccess = true;
            findPathResult.RoutePath = routePath;

            var protocole = isSecure ? _routeProvider.ProtocolSecure : _routeProvider.ProtocolDefault;
            findPathResult.FullUrl = string.Concat(protocole, "://", UrlHelper.Concat(requestDomain, path));
            findPathResult.BaseUrl = string.Concat(protocole, "://", requestDomain);

            findPathResult.IsSecure = isSecure;

            return findPathResult;
        }

        private void ComputeRedirection(FindRouteInput input, FindRouteResult result)
        {
            result.IsSuccess = false;
            result.HttpCode = "301";
            if (input.FullUrl.StartsWith(_routeProvider.ProtocolDefault))
                result.RedirectionUrl = input.FullUrl.Replace(_routeProvider.ProtocolDefault,
                    _routeProvider.ProtocolSecure);
        }

        private async Task<bool> IsHomePage(Implementation.Domain domain, IDictionary<string, string> domainDatas,
            IDictionary<string, string> routeDatas)
        {
            var siteId = string.Empty;
            if (!string.IsNullOrEmpty(domain.SiteId))
                siteId = domain.SiteId;
            else if (domainDatas.Count > 0)
                siteId = await _routeProvider.GetSiteIdAsync(domainDatas, domain.DomainMasterId);

            if (string.IsNullOrEmpty(siteId)) return false;

            return await IsHomePage(siteId, routeDatas);
        }

        private async Task<bool> IsHomePage(string siteId,
            IDictionary<string, string> routeDatas)
        {
            var metadatas = await _routeProvider.GetRootMetadataAsync(siteId);

            foreach (var metadata in metadatas)
            {
                if (!routeDatas.ContainsKey(metadata.Key)) return false;
                if (routeDatas[metadata.Key] != metadata.Value) return false;
            }

            return true;
        }

        /// <summary>
        ///     Récupère un domaine du même type si {culture} présent dans le domain 1 il faut que {culture} soit présent dans le
        ///     domain 2
        /// </summary>
        /// <param name="domain1"></param>
        /// <param name="domainDatas"> </param>
        /// <param name="domain2"></param>
        /// <returns></returns>
        private static bool IsSameDomainType(IDictionary<string, string> domainDatas, Implementation.Domain domain2)
        {
            var data2 = GetDomainDatas(domain2);

            foreach (var key in data2.Keys)
            {
                if (!domainDatas.ContainsKey(key))
                    return false;
                if (domain2.ExcludedDomainData != null && domain2.ExcludedDomainData.ContainsKey(key) &&
                    domain2.ExcludedDomainData[key] == domainDatas[key]) return false;
            }

            return true;
        }

        private static IDictionary<string, string> GetDomainDatas(Implementation.Domain domain1)
        {
            var domainRegex1 = CreateRegexDomain(domain1.Path);
            var domainMatch1 = domainRegex1.Match(domain1.Path.Replace("{", "").Replace("}", ""));
            IDictionary<string, string> data1 = new Dictionary<string, string>();
            GetDatas(data1, domainRegex1, domainMatch1);
            return data1;
        }

        private static string GetPath(bool isSecure, string requestDomain,
            string applicationPath, string preUrl,
            string port, string routePath, string protocolHttps, bool isFullPath = false)
        {
            var path = string.Empty;

            // Si bascule http/https on affiche un fullPath
            if (isFullPath)
            {
                string.Concat(UrlHelper.Protocol(isSecure, protocolHttps), "://", requestDomain);
                if (!string.IsNullOrEmpty(port) && port != "80" && port != "443") path = string.Concat(path, ":", port);
            }

            if (!string.IsNullOrEmpty(applicationPath))
                path = UrlHelper.Concat(path, applicationPath);
            if (!string.IsNullOrEmpty(preUrl))
                path = UrlHelper.Concat(path, preUrl);
            if (!string.IsNullOrEmpty(routePath))
                path = UrlHelper.Concat(path, routePath);

            path = UrlHelper.RemoveFirstSeparator(path);

            return path;
        }

        #region Methods interne

        /// <summary>
        ///     Retourne la première route qui match
        /// </summary>
        /// <returns></returns>
        private static string GetRoutePath(IRouteProvider cache, IDictionary<string, string> datas,
            IDictionary<string, string> domainDatas, ILogger<RouteManager> logger, out Route routeResult)
        {
            if (!datas.ContainsKey("action") || !datas.ContainsKey("controller"))
            {
                logger.LogWarning("no action or controller found");
                routeResult = null;
                return string.Empty;
            }

            var action = datas["action"];
            var controller = datas["controller"];

            foreach (var route in cache.GetRoutes(domainDatas))
            {
                var isCorrect = route.Controller == controller && route.Action == action;
                var isCorrect1 = string.IsNullOrEmpty(route.Controller) && string.IsNullOrEmpty(route.Action);
                var isCorrect2 = string.IsNullOrEmpty(route.Action) && route.Controller == controller;

                // TODO : finir tous les tests
                if (isCorrect || isCorrect1 || isCorrect2)
                {
                    // On génère le chemin
                    var path = GetPath(route.Path, datas, route.DefaultValues);

                    // C'est que le chemin n'est pas le bon
                    if (!string.IsNullOrEmpty(path) && path.Contains("{"))
                        continue;

                    routeResult = route;
                    return path;
                }
            }

            routeResult = null;
            return string.Empty;
        }

        /// <summary>
        ///     Récupère le chemin associé au domaine
        /// </summary>
        /// <param name="url"></param>
        /// <param name="domain"></param>
        /// <param name="domainDatas"> </param>
        /// <returns></returns>
        private static string GetRoutePath(string url, Implementation.Domain domain, Dictionary<string, string> domainDatas)
        {
            var routePath = GetPath(url, domainDatas);
            var domainPath = string.Empty;

            if (!string.IsNullOrEmpty(domain.Path))
                domainPath = GetPath(domain.Path, domainDatas);

            if (!string.IsNullOrEmpty(routePath))
                routePath = routePath.ReplaceIgnoreCase(domainPath, string.Empty).TrimStart('/');

            return routePath;
        }

        protected static Culture LoadCulture(string idCulture, IEnumerable<Culture> cultures)
        {
            return (from c in cultures where c.Id == idCulture select c).FirstOrDefault();
        }

        /// <summary>
        ///     En entre
        /// </summary>
        /// <param name="url"></param>
        /// <param name="datas"></param>
        /// <param name="routeDefaultvalues"></param>
        /// <returns></returns>
        public static string GetPath(string url, IDictionary<string, string> datas,
            IDictionary<string, string> routeDefaultvalues = null)
        {
            if (datas == null)
                return url;

            if (string.IsNullOrEmpty(url))
                return url;

            foreach (var data in datas)
            {
                var key = "{" + data.Key + "}";
                if (url.ContainsIgnoreCase(key))
                {
                    // Ce if permet de supprimer les // possible si des valeur par défaults sont null
                    if (string.IsNullOrEmpty(data.Value))
                        url = url.ReplaceIgnoreCase("/" + key, data.Value);
                    url = url.ReplaceIgnoreCase(key, data.Value);
                }
            }

            if (routeDefaultvalues != null)
                foreach (var data in routeDefaultvalues)
                {
                    var key = "{" + data.Key + "}";
                    if (url.ContainsIgnoreCase(key))
                    {
                        // Ce if permet de supprimer les // possible si des valeur par défaults sont null
                        if (string.IsNullOrEmpty(data.Value))
                            url = url.ReplaceIgnoreCase("/" + key, data.Value);
                        url = url.ReplaceIgnoreCase(key, data.Value);
                    }
                }

            return url;
        }


        /// <summary>
        ///     Retorune le premier domain qui match avec l'url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="routeProvider"></param>
        /// <param name="culture"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private static async Task<LoadDomainResult> LoadDomainAsync(string url, IDictionary<string, string> data,
            IRouteProvider routeProvider, ILogger<RouteManager> _logger)
        {
            var loadDomainResult = new LoadDomainResult();
            if (string.IsNullOrEmpty(url))
                return loadDomainResult;

            if (data == null)
                return loadDomainResult;

            if (routeProvider.Domains == null)
                return loadDomainResult;
            var siteId = string.Empty;

            foreach (var domain in routeProvider.Domains)
            {
                siteId = string.Empty;
                data.Clear();
                if (!string.IsNullOrEmpty(domain.Regex) && RegExMatch(domain.Regex, url))
                {
                    var domainRegex = CreateRegexDomain(domain.Path);
                    var domainMatch = domainRegex.Match(url);

                    if (domainMatch.Success)
                    {
                        GetDatas(data, domainRegex, domainMatch);

                        #region On récupère le site

                        if (!string.IsNullOrEmpty(domain.SiteId))
                            siteId = domain.SiteId;
                        else if (data.Count > 0)
                            siteId = await routeProvider.GetSiteIdAsync(data, domain.DomainMasterId);

                        if (string.IsNullOrEmpty(siteId))
                            continue;

                        #endregion


                        loadDomainResult.SiteId = siteId;
                        loadDomainResult.Domain = domain;
                        return loadDomainResult;
                    }

                    _logger.LogWarning("Erreur dans la configuration du domainId :" + domain.Id);
                }
            }

            return loadDomainResult;
        }

        private static bool RegExMatch(string pattern, string matchString)
        {
            var r1 = new Regex(pattern.TrimEnd(null), RegexOptions.IgnoreCase);
            return r1.Match(matchString.TrimEnd(null)).Success;
        }

        private static void GetDatas(IDictionary<string, string> datas, Regex domainRegex, Match domainMatch)
        {
            // on ajoute les valeur présente dans l'url qui surclasse celle par défault de la route
            // Iterate matching domain groups
            for (var i = 1; i < domainMatch.Groups.Count; i++)
            {
                var group = domainMatch.Groups[i];
                if (group.Success)
                {
                    var key = domainRegex.GroupNameFromNumber(i);

                    if (!string.IsNullOrEmpty(key) && !char.IsNumber(key, 0))
                        if (!string.IsNullOrEmpty(group.Value))
                        {
                            var value = group.Value;
                            SetValue(datas, key, value);
                        }
                }
            }
        }

        private static void SetValue(IDictionary<string, string> datas, string key, string value)
        {
            if (datas.ContainsKey(key))
                datas[key] = value;
            else
                datas.Add(key, value);
        }

        private static Regex CreateRegexDomain(string source)
        {
            // Perform replacements
            source = source.Replace("/", @"\/?");
            source = source.Replace(".", @"\.?");
            source = source.Replace("-", @"\-?");
            source = source.Replace("{", @"(?<");
            source = source.Replace("}", @">([a-zA-Z0-9_\-]*))"); //

            return new Regex("^" + source + ".*$", RegexOptions.IgnoreCase);
        }

        /// <summary>
        ///     Retorune le premier domain qui match avec l'url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <param name="routeProvider"> </param>
        /// <param name="cultureId"> </param>
        /// <returns></returns>
        private static async Task<Route> LoadRouteAsync(string url, IDictionary<string, string> data,
            IRouteProvider routeProvider, Dictionary<string, string> domainDatas, string siteId,
            ILogger<RouteManager> logger)
        {
            var routes = routeProvider.GetRoutes(domainDatas);

            if (routes == null)
                return null;

            // Home page
            if (string.IsNullOrEmpty(url))
            {
                var metadatas = await routeProvider.GetRootMetadataAsync(siteId);
                return RouteResult(data, routeProvider, domainDatas, metadatas, logger);
            }

            foreach (var route in routes)
            {
                var path = route.Path;

                // Cas particulier pour la racine du site
                if (string.IsNullOrEmpty(path))
                {
                    if (string.IsNullOrEmpty(url))
                        return route;
                    continue;
                }

                if (string.IsNullOrEmpty(url))
                    continue;

                if (CheckRoute(url, data, route, path)) return route;
            }

            var redirectRoutes = await routeProvider.GetRedirectRoutesAsync(siteId);

            foreach (var route in redirectRoutes)
            {
                var path = route.Path;
                if (CheckRoute(url, data, route, path)) return route;
            }

            return null;
        }

        private static Route RouteResult(IDictionary<string, string> data, IRouteProvider routeProvider,
            Dictionary<string, string> domainDatas,
            IDictionary<string, string> metadatas, ILogger<RouteManager> logger)
        {
            if (metadatas != null)
            {
                foreach (var defaultValue in metadatas)
                    data.Add(defaultValue);

                Route routeResult;
                GetRoutePath(routeProvider, data, domainDatas, logger, out routeResult);

                return routeResult;
            }

            throw new Exception("cache problem");
        }

        private static bool CheckRoute(string url, IDictionary<string, string> data, Route route, string path)
        {
            if (!string.IsNullOrEmpty(route.Regex) && RegExMatch(route.Regex, url))
            {
                var routeRegex = CreateRegexPath(path);
                var routeMatch = routeRegex.Match(url);

                if (routeMatch.Success)
                {
                    GetDatas(data, routeRegex, routeMatch);
                    return true;
                }

                throw new ArgumentException("Erreur dans la configuration de la routeId :" + route.Identity);
            }

            return false;
        }

        private static Regex CreateRegexPath(string source)
        {
            // Perform replacements
            source = source.Replace("?", @"\?");
            source = source.Replace("/", @"\/?");
            source = source.Replace(".", @"\.?");
            source = source.Replace("-", @"\-?");
            source = source.Replace("{", @"(?<");
            source = source.Replace("}", @">([a-zA-Z0-9_\-\.]*))");

            return new Regex("^" + source + ".*$", RegexOptions.IgnoreCase);
        }

        #endregion
    }
}