using System;
using System.Dynamic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Demo.Business.Command.Stats.Models;
using Demo.Common;
using Demo.Common.Command;
using Demo.Data.Stat;
using Demo.Data.Stat.Models;
using Demo.Geo;
using Demo.Routing.Extentions;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.Stats
{
    public class SaveStatsCommand : Command<SaveStatsInput, CommandResult<SaveStatsResults>>
    {
        private readonly IGeocodingService _geocodingService;
        private readonly IStatService _statService;

        public SaveStatsCommand(IStatService statService, IGeocodingService geocodingService)
        {
            _statService = statService;
            _geocodingService = geocodingService;
        }

        private static string GetDeviceType(string ua)
        {
            var ret = "";
            // Check if user agent is a smart TV - http://goo.gl/FocDk
            if (Regex.IsMatch(ua, @"GoogleTV|SmartTV|Internet.TV|NetCast|NETTV|AppleTV|boxee|Kylo|Roku|DLNADOC|CE\-HTML",
                RegexOptions.IgnoreCase))
            {
                ret = "tv";
            }
            // Check if user agent is a TV Based Gaming Console
            else if (Regex.IsMatch(ua, "Xbox|PLAYSTATION.3|Wii", RegexOptions.IgnoreCase))
            {
                ret = "tv";
            }
            // Check if user agent is a Tablet
            else if ((Regex.IsMatch(ua, "iP(a|ro)d", RegexOptions.IgnoreCase) ||
                      (Regex.IsMatch(ua, "tablet", RegexOptions.IgnoreCase)) &&
                      (!Regex.IsMatch(ua, "RX-34", RegexOptions.IgnoreCase)) ||
                      (Regex.IsMatch(ua, "FOLIO", RegexOptions.IgnoreCase))))
            {
                ret = "tablet";
            }
            // Check if user agent is an Android Tablet
            else if ((Regex.IsMatch(ua, "Linux", RegexOptions.IgnoreCase)) &&
                     (Regex.IsMatch(ua, "Android", RegexOptions.IgnoreCase)) &&
                     (!Regex.IsMatch(ua, "Fennec|mobi|HTC.Magic|HTCX06HT|Nexus.One|SC-02B|fone.945",
                         RegexOptions.IgnoreCase)))
            {
                ret = "tablet";
            }
            // Check if user agent is a Kindle or Kindle Fire
            else if ((Regex.IsMatch(ua, "Kindle", RegexOptions.IgnoreCase)) ||
                     (Regex.IsMatch(ua, "Mac.OS", RegexOptions.IgnoreCase)) &&
                     (Regex.IsMatch(ua, "Silk", RegexOptions.IgnoreCase)))
            {
                ret = "tablet";
            }
            // Check if user agent is a pre Android 3.0 Tablet
            else if (
                (Regex.IsMatch(ua,
                    @"GT-P10|SC-01C|SHW-M180S|SGH-T849|SCH-I800|SHW-M180L|SPH-P100|SGH-I987|zt180|HTC(.Flyer|\\_Flyer)|Sprint.ATP51|ViewPad7|pandigital(sprnova|nova)|Ideos.S7|Dell.Streak.7|Advent.Vega|A101IT|A70BHT|MID7015|Next2|nook",
                    RegexOptions.IgnoreCase)) ||
                (Regex.IsMatch(ua, "MB511", RegexOptions.IgnoreCase)) &&
                (Regex.IsMatch(ua, "RUTEM", RegexOptions.IgnoreCase)))
            {
                ret = "tablet";
            }
            // Check if user agent is unique Mobile User Agent
            else if (
                (Regex.IsMatch(ua,
                    "BOLT|Fennec|Iris|Maemo|Minimo|Mobi|mowser|NetFront|Novarra|Prism|RX-34|Skyfire|Tear|XV6875|XV6975|Google.Wireless.Transcoder",
                    RegexOptions.IgnoreCase)))
            {
                ret = "mobile";
            }
            // Check if user agent is an odd Opera User Agent - http://goo.gl/nK90K
            else if ((Regex.IsMatch(ua, "Opera", RegexOptions.IgnoreCase)) &&
                     (Regex.IsMatch(ua, "Windows.NT.5", RegexOptions.IgnoreCase)) &&
                     (Regex.IsMatch(ua, @"HTC|Xda|Mini|Vario|SAMSUNG\-GT\-i8000|SAMSUNG\-SGH\-i9",
                         RegexOptions.IgnoreCase)))
            {
                ret = "mobile";
            }
            // Check if user agent is Windows Desktop
            else if ((Regex.IsMatch(ua, "Windows.(NT|XP|ME|9)")) &&
                     (!Regex.IsMatch(ua, "Phone", RegexOptions.IgnoreCase)) ||
                     (Regex.IsMatch(ua, "Win(9|.9|NT)", RegexOptions.IgnoreCase)))
            {
                ret = "desktop";
            }
            // Check if agent is Mac Desktop
            else if ((Regex.IsMatch(ua, "Macintosh|PowerPC", RegexOptions.IgnoreCase)) &&
                     (!Regex.IsMatch(ua, "Silk", RegexOptions.IgnoreCase)))
            {
                ret = "desktop";
            }
            // Check if user agent is a Linux Desktop
            else if ((Regex.IsMatch(ua, "Linux", RegexOptions.IgnoreCase)) &&
                     (Regex.IsMatch(ua, "X11", RegexOptions.IgnoreCase)))
            {
                ret = "desktop";
            }
            // Check if user agent is a Solaris, SunOS, BSD Desktop
            else if (
                (Regex.IsMatch(ua, "Solaris|SunOS|BSD", RegexOptions.IgnoreCase)))
            {
                ret = "desktop";
            }
            // Check if user agent is a Desktop BOT/Crawler/Spider
            else if (
                (Regex.IsMatch(ua,
                    "Bot|Crawler|Spider|Yahoo|ia_archiver|Covario-IDS|findlinks|DataparkSearch|larbin|Mediapartners-Google|NG-Search|Snappy|Teoma|Jeeves|TinEye",
                    RegexOptions.IgnoreCase)) &&
                (!Regex.IsMatch(ua, "Mobile", RegexOptions.IgnoreCase)))
            {
                ret = "desktop";
            }
            // Otherwise assume it is a Mobile Device
            else
            {
                ret = "mobile";
            }
            return ret;
        }

        protected override async Task ActionAsync()
        {
            var client = Input.Client;
            if (!string.IsNullOrEmpty(client.Url) && client.Url.ToLower().Contains("://localhost"))
            {
                return;
            }

           /* if (!string.IsNullOrEmpty(Input.UserAgent) &&
                (Input.UserAgent.Contains("bworld") || Input.UserAgent.Contains("seo user-agent")))
            {
                return;
            }*/

            var statDbModel = new StatDbModel();
            statDbModel.SiteId = client.SiteId;
            statDbModel.PageName = client.Name;
            statDbModel.PageParam = client.Action;
            statDbModel.Url = UrlHelper.RemoveLastSeparator(client.Url);
            statDbModel.Ip = Input.IpAdress;
            statDbModel.UserId = Input.UserId;
            statDbModel.UniversType = Input.Client.Type;
            //var createdDate = DateTime.Now;
            statDbModel.CreateDate = DateTime.Now;
            // new DateTime(createdDate.Year, createdDate.Month, createdDate.Day, createdDate.Hour, createdDate.Minute, createdDate.Second, DateTimeKind.Utc);
            if (string.IsNullOrEmpty(client.ClientSessionId))
            {
                statDbModel.ClientSessionId = Guid.NewGuid().ToString();
                statDbModel.IsNewClientSesssion = true;
                if (!string.IsNullOrEmpty(client.Referrer))
                {
                    statDbModel.Referrer = UrlHelper.RemoveLastSeparator(client.Referrer);
                }
                statDbModel.UserAgent = Input.UserAgent;
                statDbModel.TypeDevice = GetDeviceType(Input.UserAgent);
                var data = await _geocodingService.ReverseFromIpAsync(Input.IpAdress);
                if (data != null)
                {
                    var geoDbModel = new GeoDbModel();
                    geoDbModel.As = data.As;
                    geoDbModel.City = StringHelper.FirstLetterToUpper(data.City);
                    geoDbModel.Country = StringHelper.FirstLetterToUpper(data.Country);
                    geoDbModel.CountryCode = data.CountryCode;
                    geoDbModel.Isp = data.Isp;
                    geoDbModel.Lat = data.Lat;
                    geoDbModel.Lon = data.Lon;
                    geoDbModel.Org = data.Org;
                    geoDbModel.Query = data.Query;
                    geoDbModel.Region = data.Region;
                    geoDbModel.RegionName = StringHelper.FirstLetterToUpper(data.RegionName);
                    geoDbModel.Status = data.Status;
                    geoDbModel.Timezone = data.Timezone;
                    geoDbModel.Zip = data.Zip;

                    statDbModel.Geo = geoDbModel;
                }
            }
            else
            {
                statDbModel.ClientSessionId = client.ClientSessionId;
            }

            if (string.IsNullOrEmpty(client.CookieSessionId))
            {
                statDbModel.CookieSessionId = Guid.NewGuid().ToString();
                statDbModel.IsNewCookieSesssion = true;
            }
            else
            {
                statDbModel.CookieSessionId = client.CookieSessionId;
            }

            await _statService.AddAsync(statDbModel);

            Result.Data = new SaveStatsResults();

            Result.Data.ClientSessionId = statDbModel.ClientSessionId;
            Result.Data.CookieSessionId = statDbModel.CookieSessionId;
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }
    }
}