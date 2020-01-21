using System;
using System.Dynamic;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Business.BusinessModule.Models;
using Demo.Business.Command.Free;
using Demo.Common.Command;
using Demo.Data;
using Demo.Routing;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.Social
{
    public class SaveSocialCommand : Command<UserInput<SaveSocialInput>, CommandResult<dynamic>>
    {
        private readonly CacheProvider _cacheProvider;
        private readonly ModuleManager _moduleManager;
        private readonly SocialBusinessModule _socialBusinessModule;
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public SaveSocialCommand(IDataFactory dataFactory, UserService userService, CacheProvider cacheProvider, ModuleManager moduleManager, SocialBusinessModule socialBusinessModule)
        {
            _dataFactory = dataFactory;
            _userService = userService;
            _cacheProvider = cacheProvider;
            _moduleManager = moduleManager;
            _socialBusinessModule = socialBusinessModule;
        }

        protected override async Task ActionAsync()
        {
            var siteId = Input.Data.Site.SiteId;

            await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, siteId);

            var itemDataModel = await SaveFreeCommand.Get<SocialBusinessModel>(Input.Data, _dataFactory, "Social");

            itemDataModel.Data = Input.Data.Data;

            await _dataFactory.SaveChangeAsync();

            await _cacheProvider.UpdateCacheAsync(siteId);

            Result.Data = new ExpandoObject();
            Result.Data.Master = await _moduleManager.GetMasterAsync(Input.Data.Site);
            Result.Data.ModuleId = itemDataModel.Id;

            var roots = _socialBusinessModule.GetRootMetadataTemp(new GetRootMetaDataInput
            {
                ItemDataModel = itemDataModel,
                DataFactory = _dataFactory
            });
            Result.Data.Url = RouteManager.GetPath(SocialBusinessModule.Url, roots);
        }
        
    }
}