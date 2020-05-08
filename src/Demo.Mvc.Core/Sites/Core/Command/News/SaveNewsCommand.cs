using System.Dynamic;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Routing;
using Demo.Mvc.Core.Sites.Core.BusinessModule;
using Demo.Mvc.Core.Sites.Core.BusinessModule.Models;
using Demo.Mvc.Core.Sites.Core.Command.Free;
using Demo.Mvc.Core.Sites.Core.Command.News.Models;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.User;
using Demo.User.Identity;

namespace Demo.Mvc.Core.Sites.Core.Command.News
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
        
    }
}