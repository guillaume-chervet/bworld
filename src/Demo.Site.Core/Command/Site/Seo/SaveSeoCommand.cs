using System;
using System.Threading.Tasks;
using Demo.Business.Command.Site.Master;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Model;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.Site.Seo
{
    public class SaveSeoCommand : Command<UserInput<SaveSeoInput>, CommandResult<dynamic>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;
        private readonly CacheProvider _cacheProvider;

        public SaveSeoCommand(IDataFactory dataFactory, UserService userService, CacheProvider cacheProvider)
        {
            _dataFactory = dataFactory;
            _userService = userService;
            _cacheProvider = cacheProvider;
        }

        protected override async Task ActionAsync()
        {
            var siteId = Input.Data.SiteId;

            await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, siteId);

            var seoItemDataModel = await GetSeoCommand.GetSeoItemDataModelAsync(_dataFactory, siteId, true);

            if (seoItemDataModel == null)
            {
                // Insert
                var masterItemDataModel = await SaveMasterCommand.GetMasterItemDataModelAsync(_dataFactory, siteId);

                seoItemDataModel = new ItemDataModel();
                seoItemDataModel.SiteId = siteId;
                seoItemDataModel.ParentId = masterItemDataModel.Id;
                seoItemDataModel.PropertyName = "Seo";
                seoItemDataModel.Module = "Seo";
                seoItemDataModel.Index = 100; // TODO

                _dataFactory.Add(seoItemDataModel);
            }

            seoItemDataModel.Data = Input.Data.Seo;

            await _dataFactory.SaveChangeAsync();
            await _cacheProvider.UpdateCacheAsync(siteId);
        }
    }
}