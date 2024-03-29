﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Routing;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.Sites.Data.Repository;
using Demo.Mvc.Core.User;
using Demo.User.Identity;

namespace Demo.Mvc.Core.Sites.Core.Command.Site
{
    public class GetSitesCommand : Command<UserInput<string>, CommandResult<IList<GetSitesResult>>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;
        private readonly IRouteManager _routeManager;

        public GetSitesCommand(IDataFactory dataFactory, UserService userService, IRouteManager routeManager)
        {
            _dataFactory = dataFactory;
            _userService = userService;
            _routeManager = routeManager;
        }

        protected override async Task ActionAsync()
        {
            await UserSecurity.CheckIsSuperAdministratorAsync(_userService, Input.UserId);
            

            var siteRepository = _dataFactory.ItemRepository;
            var siteDataModels = await GetSiteDataModelsAsync(siteRepository);

            Result.Data = new List<GetSitesResult>();
            foreach (var itemDataModel in siteDataModels)
            {
                var sitemap = await SiteMap.SitemapUrlAsync(itemDataModel, _dataFactory, _routeManager);

                Result.Data.Add(new GetSitesResult
                {
                    SiteId = itemDataModel.Id,
                    Url = sitemap.BaseUrl
                });
            }
        }

        public static async Task<IList<ItemDataModel>> GetSiteDataModelsAsync(IItemRepository siteRepository)
        {
            var siteDataModels = await siteRepository.GetItemsAsync(null, new ItemFilters {Module = "Site"});
            return siteDataModels;
        }

        
    }
}