using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Demo.Mvc.Core.Routing.Extentions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Demo.Seo
{
    public class BddJson
    {
        private readonly IList<Page> pages = new List<Page>();

        public BddJson()
        {
            Load();
        }

        public void SaveOrUpdate(Page page)
        {
            if (page == null)
            {
                return;
            }
            page.Url = NormalizeUrl(page.Url);

            var existingPage = GetPage(page.Url);

            lock (pages)
            {
                var jsonSerializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                var dataJson = JsonConvert.SerializeObject(pages.Distinct().ToList(), Formatting.Indented,
                    jsonSerializerSettings);
                var path = GetFilePath();

                File.WriteAllText(path, dataJson);
            }
        }

        public Page GetPage(string url)
        {
            lock (pages)
            {
                foreach (var page in pages)
                {
                    if (url == page.Url)
                    {
                        return page;
                    }
                }
            }

            return null;
        }

        public string Get(string url)
        {
            url = NormalizeUrl(url);
            lock (pages)
            {
                var page = GetPage(url);

                if (page != null)
                {
                    return page.Text;
                }
            }

            return null;
        }

        public static string NormalizeUrl(string path1)
        {
            if (string.IsNullOrEmpty(path1))
            {
                return string.Empty;
            }

            if (path1.Contains("#"))
            {
                var tabs = path1.Split('#');
                if (tabs.Length == 2)
                {
                    return UrlHelper.Concat(tabs[0], string.Concat("#", RemoveLastSeparator(tabs[1])));
                }
            }

            return RemoveLastSeparator(path1);
        }

        private static string RemoveLastSeparator(string path1)
        {
            if (string.IsNullOrEmpty(path1))
                return path1;

            return path1.TrimEnd('/');
        }

        private void Load()
        {
            var path = GetFilePath();

            if (File.Exists(path))
            {
                var text = File.ReadAllText(path);

                if (!string.IsNullOrEmpty(text))
                {
                    var list = JsonConvert.DeserializeObject<List<Page>>(text);
                    foreach (var elem in list)
                    {
                        pages.Add(elem);
                    }
                }
            }
        }

        private string GetFilePath()
        {
            string filePath = null;
            // Dans le cas d'un projet Web
            if (!string.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath))
            {
                var temp = AppDomain.CurrentDomain.RelativeSearchPath.Replace(@"\bin", "");
                filePath = Path.Combine(temp, @"App_Data\seo.json");
            }
            else // Dans le cas d'un batch ou autre
            {
                filePath = Path.Combine(Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), "seo.json");
            }
            return filePath;
        }
    }
}