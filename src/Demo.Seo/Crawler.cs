using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Demo.Log;
using Demo.Routing.Extentions;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium.Chrome;

namespace Demo.Seo
{
    public class Crawler
    {
        private readonly ILogger<Crawler> _logger;
        private readonly BddMongo _bdd;
        private readonly List<string> _classes = new List<string>();
        private readonly List<string> _exceptions = new List<string>();
        private readonly List<string> _externalUrls = new List<string>();
        private readonly List<string> _failedUrls = new List<string>();
        private readonly List<string> _otherUrls = new List<string>();
        private readonly List<Page> _pages = new List<Page>();

        public Crawler(ILogger<Crawler> logger, BddMongo bdd)
        {
            _logger = logger;
            _bdd = bdd;
        }
        
        /// <summary>
        ///     Crawls a site.
        /// </summary>
        public async Task CrawlSiteAsync(InputSite inputSite, string[] urls)
        {
            _logger.LogInformation("Beginning crawl:" + inputSite.BaseUrl);
            var dateTime = DateTime.Now;

            foreach (var url in urls)
            {
                _logger.LogInformation("before crawl:" + url);
                await CrawlPageAsync(inputSite, url, dateTime);
                _logger.LogInformation("after crawl:" + url);
            }

            _logger.LogInformation("before clear:" + inputSite.SiteId);
            await _bdd.ClearAsync(inputSite.SiteId, dateTime);

            //  foreach (var page in _pages)
            {
                //_logger.LogInformation("before save:" + page.Url);
                await _bdd.SaveAsync(_pages);
            }

            _logger.LogInformation("Finished crawl:" + inputSite.BaseUrl);
        }

        /// <summary>
        ///     Crawls a page.
        /// </summary>
        /// <param name="url">The url to crawl.</param>
        private async Task CrawlPageAsync(InputSite inputSite, string url, DateTime dateTime)
        {
            url = BddJson.NormalizeUrl(url);
            if (!PageHasBeenCrawled(url))
            {
                var response = await GetWebTextAsync(url);

                // TODO remove when server side rendering will be available
                var loader1 = "id=\"loading-layer\" ng-show=\"loader.isLoading\" class=\"ng-animate ng-hide-animate ng-hide-add\"";
                var loader1Replace = "id=\"loading-layer\" ng-show=\"loader.isLoading\" class=\"ng-hide\"";
                var loader2 = "id=\"loading\" ng-show=\"loader.isLoading\" class=\"ng-animate ng-hide-animate ng-hide-add\"";
                var loader2Replace = "id=\"loading\" ng-show=\"loader.isLoading\" class=\"ng-hide\"";

                var page = new Page();
                page.Text = response.Content.Replace("ng-cloak", "").Replace("ng-app=\"mw\"", "").Replace("ng-enter", "").Replace(loader1, loader1Replace).Replace(loader2, loader2Replace);
                page.StatusCode = response.StatusCode;
                page.Url = BddJson.NormalizeUrl(url);
                page.SiteId = inputSite.SiteId;
                page.Date = dateTime;

                _pages.Add(page);

                if (response.StatusCode == 200)
                {
                    var linkParser = new LinkParser();
                    linkParser.ParseLinks(inputSite, page, url);

                    var classParser = new CSSClassParser();
                    classParser.ParseForCssClasses(page);

                    //Add data to main data lists
                    AddRangeButNoDuplicates(_externalUrls, linkParser.ExternalUrls);
                    AddRangeButNoDuplicates(_otherUrls, linkParser.OtherUrls);
                    AddRangeButNoDuplicates(_failedUrls, linkParser.BadUrls);
                    AddRangeButNoDuplicates(_classes, classParser.Classes);

                    foreach (var exception in linkParser.Exceptions)
                        _exceptions.Add(exception);

                    //Crawl all the links found on the page.
                    foreach (var link in linkParser.GoodUrls)
                    {
                        var formattedLink = link;
                        try
                        {
                            formattedLink = FixPath(inputSite, formattedLink);

                            if (formattedLink != string.Empty)
                            {
                                await CrawlPageAsync(inputSite, formattedLink, dateTime);
                            }
                        }
                        catch (Exception exc)
                        {
                            _failedUrls.Add(formattedLink + " (on page at url " + url + ") - " + exc.Message);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Fixes a path. Makes sure it is a fully functional absolute url.
        /// </summary>
        /// <param name="originatingUrl">The url that the link was found in.</param>
        /// <param name="link">The link to be fixed up.</param>
        /// <returns>A fixed url that is fit to be fetched.</returns>
        public static string FixPath(InputSite inputSite, string originatingUrl)
        {
            if (originatingUrl.Contains("#"))
            {
                originatingUrl = originatingUrl.Split('#')[0];
            }

            if (!originatingUrl.Contains("http://") && !originatingUrl.Contains("https://"))
            {
                return UrlHelper.Concat(inputSite.BaseUrl, originatingUrl);
            }

            return originatingUrl;
        }

        /// <summary>
        ///     Needed a method to turn a relative path into an absolute path. And this seems to work.
        /// </summary>
        /// <param name="relativeUrl">The relative url.</param>
        /// <param name="originatingUrl">The url that contained the relative url.</param>
        /// <returns>A url that was relative but is now absolute.</returns>
        private string ResolveRelativePaths(string relativeUrl, string originatingUrl)
        {
            var resolvedUrl = string.Empty;

            var relativeUrlArray = relativeUrl.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            var originatingUrlElements = originatingUrl.Split(new[] {'/'}, StringSplitOptions.RemoveEmptyEntries);
            var indexOfFirstNonRelativePathElement = 0;
            for (var i = 0; i <= relativeUrlArray.Length - 1; i++)
            {
                if (relativeUrlArray[i] != "..")
                {
                    indexOfFirstNonRelativePathElement = i;
                    break;
                }
            }

            var countOfOriginatingUrlElementsToUse = originatingUrlElements.Length - indexOfFirstNonRelativePathElement -
                                                     1;
            for (var i = 0; i <= countOfOriginatingUrlElementsToUse - 1; i++)
            {
                if (originatingUrlElements[i] == "http:" || originatingUrlElements[i] == "https:")
                    resolvedUrl += originatingUrlElements[i] + "//";
                else
                    resolvedUrl += originatingUrlElements[i] + "/";
            }

            for (var i = 0; i <= relativeUrlArray.Length - 1; i++)
            {
                if (i >= indexOfFirstNonRelativePathElement)
                {
                    resolvedUrl += relativeUrlArray[i];

                    if (i < relativeUrlArray.Length - 1)
                        resolvedUrl += "/";
                }
            }

            return resolvedUrl;
        }

        /// <summary>
        ///     Checks to see if the page has been crawled.
        /// </summary>
        /// <param name="url">The url that has potentially been crawled.</param>
        /// <returns>Boolean indicating whether or not the page has been crawled.</returns>
        private bool PageHasBeenCrawled(string url)
        {
            foreach (var page in _pages)
            {
                if (page.Url == url)
                    return true;
            }

            return false;
        }

        /// <summary>
        ///     Merges a two lists of strings.
        /// </summary>
        /// <param name="targetList">The list into which to merge.</param>
        /// <param name="sourceList">The list whose values need to be merged.</param>
        private void AddRangeButNoDuplicates(List<string> targetList, List<string> sourceList)
        {
            foreach (var str in sourceList)
            {
                if (!targetList.Contains(str))
                    targetList.Add(str);
            }
        }

        private WebClient client = new WebClient();

        /// <summary>
        ///     Gets the response text for a given url.
        /// </summary>
        /// <param name="url">The url whose text needs to be fetched.</param>
        /// <returns>The text of the response.</returns>
        private async Task<Reponse> GetWebTextAsync(string url)
        {
            var response = new Reponse();
            response.StatusCode = 200;
            try
            {
                _logger.LogInformation("-Crawling:" + url);
                string html;
                {
                   // using (var client = new WebClient())
                    {
                        client.Headers["User-Agent"] = "bworld";
                        html = await client.DownloadStringTaskAsync(new Uri(url));

                        if (!html.Contains("<meta name=\"fragment\" content=\"!\""))
                        {
                            response.Content = html;
                            _logger.LogInformation("no ajax url:" + url);
                            return response;
                        }
                    }
                }

                var ajaxHtml = Crawl(url);

                if (!string.IsNullOrEmpty(ajaxHtml) && (ajaxHtml.StartsWith("ERROR") || ajaxHtml.StartsWith("PHANTOM ERROR")))
                {
                    _logger.LogWarning("Error:" + ajaxHtml);
                }
                else
                {
                    if (!string.IsNullOrEmpty(ajaxHtml))
                    {
                        html = ajaxHtml;
                    }
                    else
                    {
                        _logger.LogWarning("no ajax html data found:" + url);
                    }

                    if (string.IsNullOrEmpty(html))
                    {
                        _logger.LogWarning("no html data found:" + url);
                    }
                }

              /*  if (!string.IsNullOrEmpty(html) && html.Contains("SSL handshake failed"))
                {
                    //Unable to load resource (#3URL:https://www.bworld.fr/js?v=lP6VZs4JATmctyhqOWXg6D7ZBjsocSQHmxMz1-dXSp81)\r\nError code: 6. Description: SSL handshake failed\r\n
                    Logger.Default.Warn("error SSL:" + url + " Content:" + html);
                }*/

                response.Content = html;
            }
            catch (WebException exception)
            {
                response.StatusCode = (int) ((HttpWebResponse) exception.Response).StatusCode;
                response.Content = exception.Message;
            }
            catch (Exception exception)
            {
                response.StatusCode = 500;
                response.Content = exception.Message;
            }
            return response;
        }

        /// <summary>
        ///     Start a new phantomjs process for crawling
        /// </summary>
        /// <param name="url">The target url</param>
        /// <returns>Html string</returns>
        public string Crawl(string url)
        {
            var option = new ChromeOptions();
            option.AddArgument("--headless");
            option.AddArgument("--disable-gpu");
            option.AddArgument("--window-size=800x600");
            var _driver = new ChromeDriver(option);
            _driver.Manage().Timeouts().AsynchronousJavaScript = new TimeSpan(0,2,0);
            _driver.Manage().Timeouts().ImplicitWait = new TimeSpan(0,2,0);
            _driver.Manage().Timeouts().PageLoad = new TimeSpan(0,2,0);
            _driver.Navigate().GoToUrl(url);

            var pageSource = _driver.PageSource;
            return pageSource;
            /*try
            {
                var appRoot = Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory);

                if (string.IsNullOrEmpty(appRoot))
                {
                    throw new ArgumentException("appRoot is empty");
                }

                var phantomjs = Path.Combine(appRoot, "phantomjs.exe");
                if (!File.Exists(phantomjs))
                {
                    phantomjs = Path.Combine(appRoot, "approot\\phantomjs.exe");
                }

                var snapshot = Path.Combine(appRoot, "Scripts\\createSnapshot.js");
                if (!File.Exists(snapshot))
                {
                    snapshot = Path.Combine(appRoot, "approot\\Scripts\\createSnapshot.js");
                }

                var startInfo = new ProcessStartInfo
                {
                    Arguments = $"{"--ssl-protocol=any"} {snapshot} {url}",
                    FileName = phantomjs,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    StandardOutputEncoding = Encoding.UTF8
                };
                _logger.LogInformation("start phantomjs:" + url);
                var p = new Process();
                p.StartInfo = startInfo;
                p.Start();
                var output = p.StandardOutput.ReadToEnd();
                var error = p.StandardError.ReadToEnd();
                p.WaitForExit();
                if (!string.IsNullOrEmpty(error))
                {
                    _logger.LogError(null, error);
                }

                _logger.LogInformation("end phantomjs:" + url);

                return output;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error:" + url);
            }*/
            return string.Empty;
        }
    }
}
