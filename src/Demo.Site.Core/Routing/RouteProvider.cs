using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Data;
using Demo.Data.Model.Cache;
using Demo.Data.Repository;
using Demo.Routing;
using Demo.Routing.Implementation;
using Demo.Routing.Interfaces;
using Demo.Business.Command.Site.Seo;
using Microsoft.AspNetCore.Hosting;

namespace Demo.Business.Routing
{
    public class RouteProvider : IRouteProvider
    {
        private readonly IDataFactory _dataFactory;
        private readonly BusinessModuleFactory _businessModuleFactory;
        private readonly IHostingEnvironment _env;


        public RouteProvider(IDataFactory dataFactory, BusinessModuleFactory businessModuleFactory, IHostingEnvironment env)
        {
            _dataFactory = dataFactory;
            _businessModuleFactory = businessModuleFactory;
            _env = env;
        }

        public async Task<IDictionary<string, string>> GetRootMetadataAsync(string siteId)
        {
            var data =
                await _dataFactory.CacheRepository.GetFromSiteIdAsync<Dictionary<string, string>>(siteId, CacheRepository.CacheRouteKey);
            if (data == null)
            {
                data = new Dictionary<string, string>();
            }
            return data;
        }

        public async Task<IEnumerable<Route>> GetRedirectRoutesAsync(string siteId)
        {
            var routes = new List<Route>();

            var seo = await GetSeoCommand.LoadSeoBusinessModelAsync(_dataFactory, siteId);

            if (seo != null && seo.Redirects != null)
            {
                foreach (var seoRedirect in seo.Redirects)
                {
                    routes.Add(
                  new Route
                  {
                      Identity = "1",
                      Action = "Index",
                      Controller = "Redirect",
                      Path = seoRedirect.PathSource,
                      Regex = "^"+ seoRedirect.PathSource + "$",
                      RedirectPath = seoRedirect.PathDestination
                  });
                }

            }

            return routes;
        }

        public string ProtocolDefault
        {
            get { return "http"; }
        }

        private List<Domain> domains;
        private List<Route> routes;

        public IEnumerable<Domain> Domains
        {
            get
            {
                if (domains != null)
                {
                    return domains;
                }

                domains = new List<Domain>();

                if (_env.IsDevelopment())
                {
                    var domain1 = new Domain
                    {
                        Id = "1",
                        Index = 1,
                        Path = "localhost/sites/{site}",
                        Regex = @"^localhost/sites/(?!bworld).*$",
                        SecureMode = SecureMode.Secure,
                        DomainLoginUrl = "https://www.bworld.fr",
                        DomainMasterId = "bworld",
                        ExcludedDomainData = new Dictionary<string, string>()
                    };
                    domain1.ExcludedDomainData.Add("site", "bworld");

                    domains.Add(
                        domain1
                   );

                    //fasiladanse: 07b42aab-3f2a-466f-ac26-7149ca343680
                    //bworld: c27e39ee-7ba9-46f8-aa7c-9e334c72a96c
                    //demo :24f11d9b-2273-404b-b89f-6295e9c54d25
                    //gc: 227aefdb-a2b9-4c27-98d9-2f0db43f99ca
                    //broderieenord:ae3c701d-39c0-42a8-a6e6-f66925f31f25
                    // lannexe-bretignolles:be444bf1-e105-4604-9814-11105aaa5ddd
                    domains.Add(
                        new Domain
                        {
                            Id = "7",
                            Index = 7,
                            Path = "localhost",
                            Regex = @"^localhost(?!\/sites).*$",
                            SiteId = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c",
                            SecureMode = SecureMode.Secure,
                            DomainMasterId = "bworld",
                            DomainLoginUrl = "https://www.bworld.fr",
                        });


                }
             
               
                {
                    var domain2 = new Domain
                    {
                        Id = "1",
                        Index = 1,
                        Path = "www.bworld.fr/sites/{site}",
                        Regex = @"www\.bworld\.fr/sites/(?!bworld).*$",
                        SecureMode = SecureMode.Secure,
                        DomainMasterId = "bworld",
                        ExcludedDomainData = new Dictionary<string, string>(),
                         DomainLoginUrl = "https://www.bworld.fr",
                    };
                    domain2.ExcludedDomainData.Add("site", "bworld");
                    domains.Add(domain2);

                   

                    domains.Add(new Domain
                    {
                        Id = "0",
                        Index = 0,
                        Path = "www.bworld.fr",
                        Regex = @"^www\.bworld\.fr(?!\/sites).*$",
                        SiteId = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c",
                        SecureMode = SecureMode.Secure,
                        DomainMasterId = "bworld",
                        XDomainRegex = "http://*.bworld.fr",
                        FacebookAppId = "544589308979814"
                    });

                    domains.Add(
                  new Domain
                  {
                      Id = "2",
                      Index = 2,
                      Path = "bworld.fr",
                      Regex = @"bworld.fr.*$",
                      SiteId = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c",
                      SecureMode = SecureMode.Secure,
                      DomainMasterId = "bworld",
                      RedirecToDomainId = "1"
                  });



                    domains.Add(new Domain
                    {
                        Id = "2",
                        Index = 2,
                        Path = "www.bworld.io",
                        Regex = @"www.bworld.io.*$",
                        SiteId = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c",
                        SecureMode = SecureMode.Secure,
                        DomainMasterId = "bworld",
                        RedirecToDomainId = "1"
                    });

                    domains.Add(
                        new Domain
                        {
                            Id = "10",
                            Index = 0,
                            Path = "bworld.io",
                            Regex = @"bworld.io.*$",
                            SiteId = "c27e39ee-7ba9-46f8-aa7c-9e334c72a96c",
                            SecureMode = SecureMode.Secure,
                            DomainMasterId = "bworld",
                            RedirecToDomainId = "1"
                        });

                }



                domains.Add(new Domain
                {
                    Id = "9",
                    Index = 0,
                    Path = "www.lannexe-bretignolles.fr",
                    Regex = @"www.lannexe-bretignolles.fr.*$",
                    SiteId = "be444bf1-e105-4604-9814-11105aaa5ddd",
                    SecureMode = SecureMode.Secure,
                    DomainLoginUrl = "https://www.bworld.fr",
                    DomainMasterId = "lannexe-bretignolles",
                    XDomainRegex = "http://*.lannexe-bretignolles.fr"
                });

                domains.Add(
                    new Domain
                    {
                        Id = "10",
                        Index = 0,
                        Path = "lannexe-bretignolles.fr",
                        Regex = @"lannexe-bretignolles.fr.*$",
                        SiteId = "be444bf1-e105-4604-9814-11105aaa5ddd",
                        SecureMode = SecureMode.Secure,
                        DomainMasterId = "lannexe-bretignolles",
                        RedirecToDomainId = "9"
                    });

                domains.Add(new Domain
                {
                    Id = "14",
                    Index = 0,
                    Path = "www.guillaume-chervet.fr",
                    Regex = @"www.guillaume-chervet.fr.*$",
                    SiteId = "227aefdb-a2b9-4c27-98d9-2f0db43f99ca",
                    SecureMode = SecureMode.Secure,
                    DomainLoginUrl = "https://www.bworld.fr",
                    DomainMasterId = "guillaumechervet",
                    XDomainRegex = "https://*.guillaume-chervet.fr"
                });

                domains.Add(
                    new Domain
                    {
                        Id = "13",
                        Index = 0,
                        Path = "guillaume-chervet.fr",
                        Regex = @"guillaume-chervet.fr.*$",
                        SiteId = "227aefdb-a2b9-4c27-98d9-2f0db43f99ca",
                        SecureMode = SecureMode.Secure,
                        DomainMasterId = "guillaumechervet",
                        RedirecToDomainId = "14"
                    });
                    
                domains.Add(new Domain
                {
                    Id = "20",
                    Index = 2,
                    Path = "www.broderieennord.com",
                    Regex = @"www.broderieennord.com.*$",
                    SiteId = "ae3c701d-39c0-42a8-a6e6-f66925f31f25",
                    SecureMode = SecureMode.Secure,
                    DomainLoginUrl = "https://www.bworld.fr",
                    DomainMasterId = "broderieennord",
                    XDomainRegex = "https://*.broderieenord.com"
                });

                domains.Add(
                    new Domain
                    {
                        Id = "21",
                        Index = 0,
                        Path = "broderieennord.com",
                        Regex = @"broderieennord.com.*$",
                        SiteId = "ae3c701d-39c0-42a8-a6e6-f66925f31f25",
                        SecureMode = SecureMode.Secure,
                        DomainMasterId = "broderieennord",
                        RedirecToDomainId = "20"
                    });

                domains.Add(new Domain
                {
                    Id = "22",
                    Index = 4,
                    Path = "www.broderieennord.fr",
                    Regex = @"www.broderieennord.fr.*$",
                    SiteId = "ae3c701d-39c0-42a8-a6e6-f66925f31f25",
                    SecureMode = SecureMode.Secure,
                    DomainLoginUrl = "https://www.bworld.fr",
                    DomainMasterId = "broderieennord",
                    RedirecToDomainId = "20"
                });

                domains.Add(
                    new Domain
                    {
                        Id = "23",
                        Index = 5,
                        Path = "broderieennord.fr",
                        Regex = @"broderieennord.fr.*$",
                        SiteId = "ae3c701d-39c0-42a8-a6e6-f66925f31f25",
                        SecureMode = SecureMode.Secure,
                        DomainMasterId = "broderieennord",
                        RedirecToDomainId = "20"
                    });

                domains.Add(new Domain
                {
                    Id = "12",
                    Index = 0,
                    Path = "www.fasiladanse.info",
                    Regex = @"www.fasiladanse.info.*$",
                    SiteId = "07b42aab-3f2a-466f-ac26-7149ca343680",
                    SecureMode = SecureMode.Secure,
                    DomainLoginUrl = "https://www.bworld.fr",
                    DomainMasterId = "fasiladanse",
                    XDomainRegex = "https://*.fasiladanse.info"
                });

                domains.Add(
                    new Domain
                    {
                        Id = "15",
                        Index = 0,
                        Path = "fasiladanse.info",
                        Regex = @"fasiladanse.info.*$",
                        SiteId = "07b42aab-3f2a-466f-ac26-7149ca343680",
                        SecureMode = SecureMode.Secure,
                        DomainMasterId = "fasiladanse",
                        RedirecToDomainId = "12"
                    });

                /*
                domains.Add(
                    new Domain
                    {
                        Id = "2",
                        Index = 2,
                        Path = "{domain}/{path}/sites/{site}",
                        Regex = @"^([^\/])+/([^\/])+/sites/([^\/])+.*$",
                        SecureMode = SecureMode.Secure,
                        DomainMasterId = "Sites",
                        DomainLoginUrl = "https://www.bworld.fr",
                        FacebookAppId = "544589308979814"
                    });

               

                domains.Add(
                    new Domain
                    {
                        Id = "5",
                        Index = 5,
                        Path = "{domain}/sites/{site}",
                        Regex = @"^([^\/])+/sites/([^\/])+.*$",
                        SecureMode = SecureMode.Secure,
                        DomainMasterId = "Sites"
                    });

                domains.Add(
                    new Domain
                    {
                        Id = "6",
                        Index = 6,
                        Path = "{domain}/{path}/sites/{site}",
                        Regex = @"^([^\/])+/([^\/])+/sites/([^\/])+.*$",
                        SecureMode = SecureMode.Secure,
                        DomainMasterId = "Sites"
                    });

                //fasiladanse: 07b42aab-3f2a-466f-ac26-7149ca343680
                //bworld: c27e39ee-7ba9-46f8-aa7c-9e334c72a96c
                //demo :24f11d9b-2273-404b-b89f-6295e9c54d25
                //gc: 227aefdb-a2b9-4c27-98d9-2f0db43f99ca
                //broderieenord:ae3c701d-39c0-42a8-a6e6-f66925f31f25
                // lannexe-bretignolles:be444bf1-e105-4604-9814-11105aaa5ddd
                domains.Add(
                    new Domain
                    {
                        Id = "7",
                        Index = 7,
                        Path = "{domain}",
                        Regex = @"^([^\/])+.*$",
                        SiteId = "227aefdb-a2b9-4c27-98d9-2f0db43f99ca",
                        SecureMode = SecureMode.Secure,
                        DomainMasterId = "Site",
                        //RedirecToDomainId = "20"
                    });*/

                return domains;
            }
        }

        public string ProtocolSecure
        {
            get { return "https"; }
        }

        public IEnumerable<Route> GetRoutes(IDictionary<string, string> domainDatas)
        {
            if (routes != null)
            {
                return routes;
            }

            routes = new List<Route>();
            routes.Add(
                new Route
                {
                    Identity = "1",
                    Action = "Sitemap",
                    Controller = "Seo",
                    Path = "sitemap.xml",
                    Regex = "^sitemap.xml$"
                });
            routes.Add(
                new Route
                {
                    Identity = "2",
                    Action = "Robots",
                    Controller = "Seo",
                    Path = "robots.txt",
                    Regex = "^robots.txt$"
                });
            routes.Add(
                new Route
                {
                    Identity = "4",
                    Action = "BingSiteAuth",
                    Controller = "Seo",
                    Path = "BingSiteAuth.xml",
                    Regex = "^BingSiteAuth.xml",
                    CancelRedirect = true
                });
            routes.Add(
                new Route
                {
                    Identity = "4.1",
                    Action = "GoogleAuth",
                    Controller = "Seo",
                    Path = "google{code}.html",
                    Regex = @"^google.+\.html$",
                    CancelRedirect = true
                });

            routes.Add(
                new Route
                {
                    Identity = "3",
                    Action = "Index",
                    Controller = "Admin",
                    Path = "administration",
                    Regex = "^administration.*$"
                });

            routes.Add(
                new Route
                {
                    Identity = "6",
                    Action = "Index",
                    Controller = "Admin",
                    Path = "super-administration",
                    Regex = "^super-administration.*$"
                });

            routes.Add(
                new Route
                {
                    Identity = "5",
                    Action = "Connexion",
                    Controller = "Utilisateur",
                    Path = "utilisateur",
                    Regex = @"^utilisateur.*$"
                });

            routes.Add(
                new Route
                {
                    Identity = "7",
                    Action = "NoPage",
                    Controller = "Default",
                    Path = "pas_de_page_configuree",
                    Regex = @"^pas_de_page_configuree$"
                });

            routes.Add(
                new Route
                {
                    Identity = "8",
                    Action = "Index",
                    Controller = "Home",
                    Path = "privee",
                    Regex = "^privee$"
                });

            var modules = _businessModuleFactory.GetModules();

            foreach (var businessModule in modules)
            {
                var routesTemp = businessModule.GetRoutes(domainDatas);
                if (routesTemp != null)
                    routes.AddRange(routesTemp);
            }

            return routes;
        }

        public Task<string> GetSiteIdAsync(IDictionary<string, string> data, string masterDomainId)
        {
            var getSiteId = new GetSiteId();
            getSiteId.Data = data;
            getSiteId.MasterDomainId = masterDomainId;

            return _dataFactory.CacheRepository.GetSiteIdFromKeyAsync(getSiteId, CacheRepository.CacheRouteKey);
        }

    }
}