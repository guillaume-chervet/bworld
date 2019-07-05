using System;
using System.Dynamic;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Business.BusinessModule.Models;
using Demo.Business.Command.Free;
using Demo.Business.Command.News.Models;
using Demo.Common.Command;
using Demo.Data;
using Demo.Routing;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.News
{
    public class SaveNewsCommand : Command<UserInput<SaveNewsInput>, CommandResult<dynamic>>
    {
        private readonly CacheProvider _cacheProvider;
        private readonly ModuleManager _moduleManager;
        private readonly NewsBusinessModule _newsBusinessModule;
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public SaveNewsCommand(IDataFactory dataFactory, UserService userService, CacheProvider cacheProvider, ModuleManager moduleManager, NewsBusinessModule newsBusinessModule)
        {
            _dataFactory = dataFactory;
            _userService = userService;
            _cacheProvider = cacheProvider;
            _moduleManager = moduleManager;
            _newsBusinessModule = newsBusinessModule;
        }

        protected override async Task ActionAsync()
        {
            var siteId = Input.Data.Site.SiteId;

            await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, siteId);

            var itemDataModel = await SaveFreeCommand.SaveItemDataModelAsync<NewsBusinessModel>(_dataFactory, Input.Data, Input.UserId, "News");
            var freeBusinessModel = (NewsBusinessModel)itemDataModel.Data;
            freeBusinessModel.DisplayMode = Input.Data.DisplayMode;
            freeBusinessModel.NumberItemPerPage = Input.Data.NumberItemPerPage;

            await _dataFactory.SaveChangeAsync();

            await _cacheProvider.UpdateCacheAsync(siteId);

            Result.Data = new ExpandoObject();
            Result.Data.Master = await _moduleManager.GetMasterAsync(Input.Data.Site);

            var roots = _newsBusinessModule.GetRootMetadata(new GetRootMetaDataInput
            {
                ItemDataModel = itemDataModel,
                DataFactory = _dataFactory
            });
            Result.Data.Url = RouteManager.GetPath(NewsBusinessModule.Url, roots);
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }
    }
}