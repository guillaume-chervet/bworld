using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Sites.Core.Command.Free;
using Demo.Mvc.Core.Sites.Core.Command.Free.Models;
using Demo.Mvc.Core.Sites.Core.Command.News;
using Demo.Mvc.Core.Sites.Core.Command.News.Models;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.Sites.Data.Repository;
using Demo.User.Identity;

namespace Demo.Mvc.Core.Sites.Core.Command.Notifications
{
    public class GetNotificationsItemCommand : Command<UserInput<GetModuleInput>, CommandResult<GetNewsItemResult>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public GetNotificationsItemCommand(IDataFactory dataFactory, UserService userService)
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

            var result = await GetNewsItemResult(_dataFactory, _userService, itemDataModel, Input.UserId);
            Result.Data = result;
        }

        public static async Task<GetNewsItemResult> GetNewsItemResult(IDataFactory dataFactory, UserService userService
           , ItemDataModel itemDataModel, string userId)
        {
            var result = await GetNewsItemResultAsync<NotificationItemBusinessModel, GetNewsItemResult>(dataFactory, userService, itemDataModel);
           
            return result;
        }

        public static async Task<G> GetNewsItemResultAsync<T, G>(IDataFactory dataFactory, UserService userService
           , ItemDataModel itemDataModel) where T : FreeBusinessModel where G: GetNewsItemBase, new()
        {
            var result = await GetFreeResultAsync<T, G>(userService, itemDataModel, dataFactory.ItemRepository);

            var itemDataModelParent =
               await dataFactory.ItemRepository.GetItemAsync(itemDataModel.SiteId, itemDataModel.ParentId);
            var dataParent = (NotificationBusinessModel)itemDataModelParent.Data;
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
            result.Elements =  await GetNewsCommand.UpdateElementAsync(itemDataModel.SiteId , moduleFree.Elements, itemRepository);

            result.UserInfo = userInfo.UserInfo;
            result.LastUpdateUserInfo = userInfo.LastUpdateUserInfo;
            result.CreateDate = itemDataModel.CreateDate;
            result.UpdateDate = itemDataModel.UpdateDate;
            return result;
        }
    }
}