using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Sites.Core.Command.Free;
using Demo.Mvc.Core.Sites.Core.Command.Free.Models;
using Demo.Mvc.Core.Sites.Core.Command.News.Models;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.Sites.Data.Repository;
using Demo.Mvc.Core.User;
using Demo.User.Identity;
using Demo.User.Site;

namespace Demo.Mvc.Core.Sites.Core.Command.News
{
    public class GetNewsItemCommand : Command<UserInput<GetModuleInput>, CommandResult<GetNewsItemResult>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public GetNewsItemCommand(IDataFactory dataFactory, UserService userService)
        {
            _dataFactory = dataFactory;
            _userService = userService;
        }

        protected override async Task ActionAsync()
        {
            var itemDataModel = await _dataFactory.ItemRepository.GetItemAsync(Input.Data.SiteId, Input.Data.ModuleId);

            if (itemDataModel == null)
            {
                Result.ValidationResult.AddError("NO_DATA_FOUND");
                return;
            }

            string userId = Input.UserId;
          

            var result = await GetNewsItemResult(_dataFactory, _userService, itemDataModel, userId);
            Result.Data = result;
        }

        public static async Task CheckAuthorisationAsync(UserService userService, IItemRepository itemRepository, string siteId, string moduleId,
            string userId)
        {
            var itemDataModel = await itemRepository.GetItemAsync(siteId, moduleId);
            await CheckAuthorisationAsync(userService, itemDataModel, userId);
        }

        public static async Task CheckAuthorisationAsync(UserService userService,ItemDataModel itemDataModel, string userId)
        {
            var siteId = itemDataModel.SiteId;
            var roles = new List<SiteUserRole>();
            if (itemDataModel.State == ItemState.Draft)
            {
                roles.Add(SiteUserRole.Administrator);
            }
            else if (NewsItemBusinessModule.IsPrivate(itemDataModel.PropertyName))
            {
                roles.Add(SiteUserRole.PrivateUser);
            }
            var nbRole = roles.Count;
            if (nbRole > 0 && string.IsNullOrEmpty(userId))
            {
                throw new NotAuthentifiedException();
            }

            if (nbRole > 0)
            {
                var canGetData = await UserSecurity.HasRolesAsync(userService, userId, siteId, true, roles.ToArray());

                if (!canGetData)
                {
                    throw new NotAuthentifiedException();
                }
            }
        }

        public static async Task<GetNewsItemResult> GetNewsItemResult(IDataFactory dataFactory, UserService userService
           , ItemDataModel itemDataModel, string userId)
        {
            var result = await GetNewsItemResultAsync<FreeBusinessModel, GetNewsItemResult>(dataFactory, userService, itemDataModel, userId);

            return result;
        }

        public static async Task<G> GetNewsItemResultAsync<T, G>(IDataFactory dataFactory, UserService userService
           , ItemDataModel itemDataModel, string userId) where T : FreeBusinessModel where G: GetNewsItemBase, new()
        {
            await CheckAuthorisationAsync(userService, itemDataModel, userId);

            var result = await GetFreeResultAsync<T, G>(userService, itemDataModel, dataFactory.ItemRepository);
            var itemDataModelParent = await dataFactory.ItemRepository.GetItemAsync(itemDataModel.SiteId, itemDataModel.ParentId);

            await CheckAuthorisationAsync(userService, itemDataModelParent, userId);

            var dataParent = (T)itemDataModelParent.Data;
            var parentTitle = FreeBusinessModule.GetTitle(dataParent.Elements);

            result.ParentModuleId = itemDataModel.ParentId;
            result.ModuleId = itemDataModel.Id;
            result.ParentTitle = parentTitle;

            return result;
        }

        public static async Task<G> GetFreeResultAsync<T, G>(UserService userService, ItemDataModel itemDataModel, IItemRepository itemRepository)
            where T : FreeBusinessModel where G : GetFreeResult, new()
        {
            var moduleFree = (T) itemDataModel.Data;
            var userInfo = await GetFreeCommand.GetUserInfoAsync<T>(userService, moduleFree);

            var result = new G();
            result.Elements = await GetNewsCommand.UpdateElementAsync(itemDataModel.SiteId, moduleFree.Elements, itemRepository);
            result.IsDisplayAuthor = moduleFree.IsDisplayAuthor;
            result.IsDisplaySocial = moduleFree.IsDisplaySocial;
            result.Icon = moduleFree.Icon;

            result.UserInfo = userInfo.UserInfo;
            result.State = itemDataModel.State;
            result.Tags = itemDataModel.Tags;
            result.LastUpdateUserInfo = userInfo.LastUpdateUserInfo;
            result.CreateDate = itemDataModel.CreateDate;
            result.UpdateDate = itemDataModel.UpdateDate;
            return result;
        }
    }
}