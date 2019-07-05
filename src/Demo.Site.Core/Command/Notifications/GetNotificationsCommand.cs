using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Business.Command.Free;
using Demo.Business.Command.News;
using Demo.Business.Command.News.Models;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Model;
using Demo.Data.Repository;
using Demo.User.Identity;

namespace Demo.Business.Command.Notifications
{
    public class GetNotificationsCommand : Command<GetNewsInput, CommandResult<GetNewsResult>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public GetNotificationsCommand(IDataFactory dataFactory, UserService userService)
        {
            _dataFactory = dataFactory;
            _userService = userService;
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }

        protected override async Task ActionAsync()
        {
            var siteId = Input.SiteId;

            var itemDataModel = await GetAsync<NotificationBusinessModel>(siteId, siteId, NotificationBusinessModule.ModuleName, "Notification");


            if (itemDataModel == null)
            {
                Result.ValidationResult.AddError("NO_DATA_FOUND");
                return;
            }

          /*  var moduleNews = (NewsBusinessModel)itemDataModel.Data;
            GetNewsCommand. UpdateElementAsync(moduleNews.Elements);
            var userInfo = await GetFreeCommand.GetUserInfoAsync(_userService, moduleNews);*/

            int nbChild = 20;

            Func<ItemDataModel, Task<GetNewsItemSummary>> getItemFunc = GetItemsAsync<GetNewsItemSummary, NotificationItemBusinessModel>;
            var newsItemResult = await GetNewsCommand.GetNewsItemSummariesAsync(_dataFactory, Input,nbChild, itemDataModel, getItemFunc);

            var result = new GetNewsResult();
            result.NumberItemPerPage = nbChild;
            /*     result.Elements = moduleNews.Elements;
                
                 result.DisplayMode = moduleNews.DisplayMode;
                 result.UserInfo = userInfo.UserInfo;
                 result.LastUpdateUserInfo = userInfo.LastUpdateUserInfo;*/
            result.CreateDate = itemDataModel.CreateDate;
            result.UpdateDate = itemDataModel.UpdateDate;
            result.GetNewsItem = newsItemResult.Items;
            result.HasNext = newsItemResult.HasNext;
            result.IdNext = newsItemResult.IdNext;
            result.HasPrevious = newsItemResult.HasPrevious;
            result.IdPrevious = newsItemResult.IdPrevious;
            result.ModuleId = itemDataModel.Id;

            Result.Data = result;
        }

        private async Task<T> GetItemsAsync<T, G>(ItemDataModel dataModel) where T : GetNewsItemSummary, new() where G : NotificationItemBusinessModel, new()
        {
            var newsItem = dataModel.Data as G;
            if (newsItem != null)
            {
                var userInfoNewsItem = await GetFreeCommand.GetUserInfoAsync(_userService, newsItem);
                var getNewsItemResult = new T();
                var elements = newsItem.Elements;

               await GetNewsCommand.UpdateElementAsync( dataModel.SiteId ,elements, _dataFactory.ItemRepository);

                getNewsItemResult.Elements = elements;

                getNewsItemResult.ModuleId = dataModel.Id;
                getNewsItemResult.CreateDate = dataModel.CreateDate;
                getNewsItemResult.UpdateDate = dataModel.UpdateDate;
                getNewsItemResult.UserInfo = userInfoNewsItem.UserInfo;
                getNewsItemResult.LastUpdateUserInfo = userInfoNewsItem.LastUpdateUserInfo;
                return getNewsItemResult;
            }
            return null;
        }

        private async Task<ItemDataModel> GetAsync<T>(string _siteId, string _parentId, string moduleName, string propertyName) where T : class, new()
        {
            var itemDataModel = (await
                    _dataFactory.ItemRepository.GetItemsAsync(_siteId,
                    new ItemFilters { ParentId = _parentId, Module = NotificationBusinessModule.ModuleName }))
                .FirstOrDefault();

            if (itemDataModel == null)
            {
                itemDataModel = await SaveFreeCommand.InitItemDataModelAsync<T>(_dataFactory,
                    moduleName, _siteId, _parentId, propertyName);
                await _dataFactory.SaveChangeAsync();
            }
            return itemDataModel;
        }
    }
}