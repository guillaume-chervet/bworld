using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Routing.Implementation;
using Demo.Routing.Interfaces;

namespace Demo.Routing.Tests.Implementation
{
    public class RouteProviderMock : IRouteProvider
    {
        public RouteProviderMock()
        {
            ProtocolSecure = "https";
            ProtocolDefault = "http";
        }

        public IEnumerable<Culture> Cultures
        {
            get
            {
                var cultures = new List<Culture>();
                cultures.Add(new Culture {Id = "1", Key = "fr", Lcid = 1});
                cultures.Add(new Culture {Id = "2", Key = "en", Lcid = 2});
                return cultures;
            }
        }

        public Task<IDictionary<string, string>> GetRootMetadataAsync(string siteId)
        {
            IDictionary<string, string> data = new Dictionary<string, string>();
            data.Add("action", "Index");
            data.Add("controller", "Free");

            return Task.FromResult(data);
        }

        public IEnumerable<Domain> Domains
        {
            get
            {
                var domains = new List<Domain>();
                domains.Add(new Domain
                {
                    Id = "1",
                    Index = 1,
                    Path = "{site}.demo.fr",
                    Regex = @"([^\/])\.demo\.fr.*$",
                    SecureMode = SecureMode.NoSecure,
                    DomainMasterId = "1"
                });
                domains.Add(new Domain
                {
                    Id = "2",
                    Index = 2,
                    Path = "https://site.demo.fr/{site}",
                    Regex = @"^https://site\.demo\.fr/([^\/])+.*$",
                    SecureMode = SecureMode.Secure,
                    DomainMasterId = "1"
                });
                domains.Add(new Domain
                {
                    Id = "3",
                    Index = 3,
                    Path = "http://site.demo.fr/{site}/{culture}",
                    Regex = @"^http://site\.demo\.fr/([^\/])+/([^\/])+.*$",
                    SecureMode = SecureMode.NoSecure,
                    DomainMasterId = "1"
                });
                domains.Add(new Domain
                {
                    Id = "4",
                    Index = 4,
                    Path = "https://site.demo.fr/Secure{site}/{culture}",
                    Regex = @"^https://site\.demo\.fr/(Secure)([^\/])+/([^\/])+.*$",
                    SecureMode = SecureMode.Secure,
                    DomainMasterId = "1"
                });
                domains.Add(new Domain
                {
                    Id = "5",
                    Index = 5,
                    Path = "https://www.demo.fr",
                    Regex = @"^https://www\.demo\.fr+.*$",
                    SecureMode = SecureMode.NoSecure,
                    DomainMasterId = "2",
                    SiteId = "1"
                });

                return domains;
            }
        }

        public Task<string> GetSiteIdAsync(IDictionary<string, string> data, string masterDomainId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Route> GetRoutes(IDictionary<string, string> domainDatas)
        {
            var routes = new List<Route>();
            routes.Add(new Route
            {
                Identity = "1",
                Action = "Index",
                Controller = "Home",
                Path = "Accueil",
                Regex = "^accueil$"
            });
            routes.Add(new Route
            {
                Identity = "2",
                Action = "Index",
                Controller = "Home",
                Path = "Home",
                Regex = "^home$"
            });
            routes.Add(new Route
            {
                Identity = "3",
                Action = "Sitemap",
                Controller = "Seo",
                Path = "Home",
                Regex = "^sitemap.xml$"
            });
            routes.Add(new Route
            {
                Identity = "4",
                Action = "Robot",
                Controller = "Seo",
                Path = "Home",
                Regex = "^robot.txt$"
            });
            return routes;
        }

        public string ProtocolSecure { get; }

        public string ProtocolDefault { get; }

        IEnumerable<Domain> IRouteProvider.Domains => throw new NotImplementedException();

        public IEnumerable<Culture> GetCultures(string siteId)
        {
            return Cultures;
        }

        public string GetDefaultCulture(string masterDomainId, string siteId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Routing.Implementation.Site> Sites(string masterDomaiId)
        {
            var sites = new List<Routing.Implementation.Site>();

            var sitePrincipal = new Routing.Implementation.Site
            {
                Id = "1",
                DefaultCultureId = "1",
                DefaultValues = new Dictionary<string, string>()
            };

            var site1 = new Routing.Implementation.Site
            {
                Id = "2",
                DefaultCultureId = "1",
                DefaultValues = new Dictionary<string, string>()
            };
            site1.DefaultValues.Add("site", "MonSiteDemo");

            var site2 = new Routing.Implementation.Site
            {
                Id = "3",
                DefaultCultureId = "1",
                DefaultValues = new Dictionary<string, string>()
            };
            site2.DefaultValues.Add("site", "Toto");

            sites.Add(sitePrincipal);
            sites.Add(site1);
            sites.Add(site2);

            return sites;
        }

        public Task<IEnumerable<Route>> GetRedirectRoutesAsync(string siteId)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Route>> IRouteProvider.GetRedirectRoutesAsync(string siteId)
        {
            throw new NotImplementedException();
        }

        IEnumerable<Route> IRouteProvider.GetRoutes(IDictionary<string, string> domainDatas)
        {
            throw new NotImplementedException();
        }
    }
}