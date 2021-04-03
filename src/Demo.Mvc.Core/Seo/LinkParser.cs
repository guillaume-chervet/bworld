using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Demo.Seo
{
    public class LinkParser
    {
        private const string _SITEMAP_REGEX = "<loc>(.+)</loc>";
        private const string _LINK_REGEX = "href=\".+\"";
        public List<string> GoodUrls { get; set; } = new List<string>();
        public List<string> BadUrls { get; set; } = new List<string>();
        public List<string> OtherUrls { get; set; } = new List<string>();
        public List<string> ExternalUrls { get; set; } = new List<string>();
        public List<string> Exceptions { get; set; } = new List<string>();


        public static bool IsBad(string url){
                if(url.Contains("amp;amp;amp;")) {
                    return true;
                }
                return false;
        }

        /// <summary>
        ///     Parses a page looking for links.
        /// </summary>
        /// <param name="page">The page whose text is to be parsed.</param>
        /// <param name="sourceUrl">The source url of the page.</param>
        public void ParseLinks(InputSite inputSite, Page page, string sourceUrl)
        {
            if (sourceUrl.EndsWith(".xml"))
            {
                var matches = Regex.Matches(page.Text, _SITEMAP_REGEX);

                for (var i = 0; i <= matches.Count - 1; i++)
                {
                    var anchorMatch = matches[i];
                    var foundHref = BddJson.NormalizeUrl(anchorMatch.Value);
                    // TODO faire un Regex Match
                    foundHref = foundHref.Replace("<loc>", "");
                    foundHref = foundHref.Replace("</loc>", "");

                    if (!IsBad(foundHref) && !GoodUrls.Contains(foundHref))
                    {
                        GoodUrls.Add(foundHref);
                    }
                }
            }
            else
            {
                var matches = Regex.Matches(page.Text, _LINK_REGEX);

                for (var i = 0; i <= matches.Count - 1; i++)
                {
                    var anchorMatch = matches[i];

                    if (anchorMatch.Value == string.Empty)
                    {
                        BadUrls.Add("Blank url value on page " + sourceUrl);
                        continue;
                    }

                    string foundHref = null;
                    try
                    {
                        foundHref = anchorMatch.Value.Replace("href=\"", "");
                        foundHref = foundHref.Substring(0, foundHref.IndexOf("\""));
                    }
                    catch (Exception exc)
                    {
                        Exceptions.Add("Error parsing matched href: " + exc.Message);
                    }

                    foundHref = BddJson.NormalizeUrl(foundHref);

                    if (!IsBad(foundHref) && !GoodUrls.Contains(foundHref))
                    {
                        if (IsExternalUrl(inputSite, foundHref))
                        {
                            ExternalUrls.Add(foundHref);
                        }
                        else if (!IsAWebPage(foundHref))
                        {
                            foundHref = Crawler.FixPath(inputSite, sourceUrl);
                            OtherUrls.Add(foundHref);
                        }
                        else
                        {
                            GoodUrls.Add(foundHref);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Is the url to an external site?
        /// </summary>
        /// <param name="url">The url whose externality of destination is in question.</param>
        /// <returns>Boolean indicating whether or not the url is to an external destination.</returns>
        private static bool IsExternalUrl(InputSite inputSite, string url)
        {
            if (url.IndexOf(inputSite.BaseUrl) > -1)
            {
                return false;
            }
            if (url.Contains("http://") || url.Contains("www") || url.Contains("https://"))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Is the value of the href pointing to a web page?
        /// </summary>
        /// <param name="foundHref">The value of the href that needs to be interogated.</param>
        /// <returns>Boolen </returns>
        private static bool IsAWebPage(string foundHref)
        {
            if (foundHref.IndexOf("javascript:") == 0 || foundHref.IndexOf("{") == 0 || foundHref.Contains("css?"))
            {
                return false;
            }
            var extension = foundHref.Substring(foundHref.LastIndexOf(".") + 1,
                foundHref.Length - foundHref.LastIndexOf(".") - 1);
            switch (extension)
            {
                case "jpg":
                case "css":
                case "js":
                case "png":
                    return false;
                default:
                    return true;
            }
        }
    }
}