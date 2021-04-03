using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Routing;
using Demo.Mvc.Core.Sites.Core.Command.Site;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.User;
using Demo.User.Identity;

namespace Demo.Mvc.Core.Sites.Core.Command.User
{
    public class GetUserCommand : Command<UserInput<string>, CommandResult<GetUserResult>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;
        private readonly IRouteManager _routeManager;

        public GetUserCommand(IDataFactory dataFactory, UserService userService, IRouteManager routeManager)
        {
            _dataFactory = dataFactory;
            _userService = userService;
            _routeManager = routeManager;
        }
        protected override async Task ActionAsync()
        {
            if (string.IsNullOrEmpty(Input.UserId))
            {
               throw new NotAuthentifiedException();
            }

            var user = await _userService.FindApplicationUserByIdAsync(Input.UserId);

            var getSites = new List<GetSitesResult>();
            foreach (var role in user.Roles)
            {
                if (UserSecurity.IsSiteId(role))
                {
                    var itemDataModel = await _dataFactory.ItemRepository.GetItemAsync(null, role);
                    if (itemDataModel != null)
                    {
                        var sitemap = await SiteMap.SitemapUrlAsync(itemDataModel, _dataFactory, _routeManager);

                        getSites.Add(new GetSitesResult
                        {
                            SiteId = itemDataModel.Id,
                            Url = sitemap.BaseUrl
                        });
                    }
                }
            }

            var getUserResult = new GetUserResult();
            getUserResult.GetSites = getSites;

            Result.Data = getUserResult;
        }
    }
}