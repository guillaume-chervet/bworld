using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Stats.Data;
using Demo.Mvc.Core.User;
using Demo.User.Identity;
using Demo.User.Site;

namespace Demo.Mvc.Core.Stats
{
    public class GetStatsCommand : Command<UserInput<GetStatsInput>, CommandResult<GetStatsResult>>
    {
        private readonly IStatService _statService;
        private readonly UserService _userService;

        public GetStatsCommand(IStatService statService, UserService userService)
        {
            _statService = statService;
            _userService = userService;
        }

        protected override async Task ActionAsync()
        {
            var siteId = Input.Data.SiteId;
            await UserSecurity.CheckHasOneRolesAsync(_userService, Input.UserId, siteId, SiteUserRole.Administrator, SiteUserRole.PrivateUser);

            var beginDate = DateTime.Today;
            var endDate = DateTime.Today.AddDays(1);

            if (Input.Data.Date.HasValue)
            {
                beginDate = Input.Data.Date.Value.Date;
                endDate = Input.Data.Date.Value.Date.AddDays(1);
            }

            Result.Data = new GetStatsResult();

            var stats = await _statService.GetStatsync(beginDate, endDate, siteId);

            var pages = (from p in stats
                group p by p.PageName
                into g
                select new
                {
                    PageName = g.Key,
                    PageParams = (from newp in g.ToList() group newp by newp.PageParam into newg select new {Param=newg.Key, NbView =newg.Count(), NbNewClientSession = newg.Count(p => p.IsNewClientSesssion == true), NbNewCookieSession = newg.Count(p => p.IsNewCookieSesssion == true) }).ToList(),
                    g.First().Url,
                    NbView = g.Count(),
                    NbNewClientSession = g.Count(p => p.IsNewClientSesssion == true),
                    NbNewCookieSession = g.Count(p => p.IsNewCookieSesssion == true)
                }
                ).OrderBy(p=>p.PageName).ToList();

            var referrers = (from p in stats.Where(a => a.IsNewClientSesssion == true)
                group p by p.Referrer
                into g
                select new
                {
                    Referrer = g.Key,
                    g.First().Url,
                    NbNewClientSession = g.Count(),
                    NbNewCookieSession = g.Count(p => p.IsNewCookieSesssion == true)
                }
                ).OrderBy(p => p.Referrer).ToList();

            var regions = (from p in stats.Where(a => a.IsNewClientSesssion == true && a.Geo != null && a.Geo.RegionName != null)
                group p by p.Geo.RegionName.ToLower()
                into g
                select new
                {
                    g.First().Geo.RegionName,
                    g.First().Geo.Country,
                    NbNewClientSession = g.Count(),
                    NbNewCookieSession = g.Count(p => p.IsNewCookieSesssion == true)
                }
                ).OrderBy(p => p.Country).OrderBy(p => p.RegionName).ToList();

            var devices = (from p in stats.Where(a => a.IsNewClientSesssion == true)
                group p by p.TypeDevice
                into g
                select new
                {
                    Device = g.First().TypeDevice,
                    NbNewClientSession = g.Count(),
                    NbNewCookieSession = g.Count(p => p.IsNewCookieSesssion == true)
                }
                ).ToList();

            var hoursList = (from p in stats
                group p by p.CreateDate.ToLocalTime().Hour
                into g
                select new
                {
                    Hour = g.Key,
                    NbView = g.Count(),
                    NbNewClientSession = g.Count(p => p.IsNewClientSesssion == true),
                    NbNewCookieSession = g.Count(p => p.IsNewCookieSesssion == true)
                }
                ).ToList();


            var hours = new List<dynamic>();
            for (var i = 0; i <= 24; i++)
            {
                var info = hoursList.FirstOrDefault(p => p.Hour == i);

                if (info == null)
                {
                    info = new
                    {
                        Hour = i,
                        NbView = 0,
                        NbNewClientSession = 0,
                        NbNewCookieSession = 0
                    };
                }

                hours.Add(info);
            }

            var nbVisit = stats.Count(p => p.IsNewClientSesssion == true);
            var nbNewVisitor = stats.Count(p => p.IsNewCookieSesssion == true);
            var nbPageView = stats.Count();

            var data = Result.Data;
            data.NbVisit = nbVisit;
            data.NbNewVisitor = nbNewVisitor;
            data.NbPageView = nbPageView;
            data.Pages = pages;
            data.Referrers = referrers;
            data.Hours = hours;
            data.Regions = regions;
            data.Devices = devices;
        }
    }
}