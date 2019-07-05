using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Business.Command.Free;
using Demo.Business.Command.Free.Models;
using Demo.Business.Command.News.Models;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Model;
using Demo.Data.Repository;
using Demo.User.Identity;

namespace Demo.Business.Command.News
{
    public class GetNewsCommand : Command<UserInput<GetNewsInput>, CommandResult<GetNewsResult>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public GetNewsCommand(IDataFactory dataFactory, UserService userService)
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
            var itemDataModel =
                await _dataFactory.ItemRepository.GetItemAsync(Input.Data.SiteId, Input.Data.ModuleId, false, false, false);

            if (itemDataModel == null)
            {
                Result.ValidationResult.AddError("NO_DATA_FOUND");
                return;
            }

            await GetNewsItemCommand.CheckAuthorisationAsync(_userService, itemDataModel, Input.UserId);

            var moduleNews = (NewsBusinessModel) itemDataModel.Data;

            await UpdateElementAsync(itemDataModel.SiteId, moduleNews.Elements, _dataFactory.ItemRepository);
            var userInfo = await GetFreeCommand.GetUserInfoAsync(_userService, moduleNews);
          
            int nbChild = moduleNews.NumberItemPerPage > 0 ? moduleNews.NumberItemPerPage: 12;

            Func<ItemDataModel, Task<GetNewsItemSummary>> getItemFunc = GetItemsAsync<GetNewsItemSummary, NewsItemBusinessModel>;
            var newsItemResult = await GetNewsItemSummariesAsync(_dataFactory, Input.Data, nbChild, itemDataModel, getItemFunc);

            var result = new GetNewsResult();
            result.Elements = moduleNews.Elements;
            result.State = itemDataModel.State;
            result.NumberItemPerPage = nbChild;
            result.DisplayMode = moduleNews.DisplayMode;
            result.UserInfo = userInfo.UserInfo;
            result.LastUpdateUserInfo = userInfo.LastUpdateUserInfo;
            result.CreateDate = itemDataModel.CreateDate;
            result.UpdateDate = itemDataModel.UpdateDate;
            result.GetNewsItem = newsItemResult.Items;
            result.HasNext = newsItemResult.HasNext;
            result.IdNext = newsItemResult.IdNext;
            result.HasPrevious = newsItemResult.HasPrevious;
            result.IdPrevious = newsItemResult.IdPrevious;

            Result.Data = result;
        }

        public static async Task<GetNewsItemSummaries<T>> GetNewsItemSummariesAsync<T>(IDataFactory data, GetNewsInput input , int nbChild, ItemDataModel itemDataModel, Func<ItemDataModel, Task<T>> getItemFunc ) where T : GetNewsItemSummary, new()
        {
            var newsItemResult = new List<T>();

            var itemFilter = new ItemFilters
            {
                Limit = nbChild,
                ParentId = itemDataModel.Id,
                SortDescending = true,
                States = input.States,
                Tags = input.Tags
            };

            if (input.FilterIndex.HasValue)
            {
                itemFilter.IndexLt = input.FilterIndex;
                itemFilter.SortDescending = true;
            }

            var childs = await data.ItemRepository.GetItemsAsync(input.SiteId, itemFilter);

            var tasks = new List<Task<T>>();
            foreach (var dataModel in childs.OrderByDescending(i => i.Index))
            {
                var task = getItemFunc(dataModel);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
            {
                var resultItem = task.Result;
                if (resultItem != null)
                {
                    newsItemResult.Add(resultItem);
                }
            }

            // recherche du next
            var nbNext = await NbNext(data, input.SiteId , childs, itemDataModel, input.States, input.Tags);

            var hasPrevious = false;
            var idPrevious = string.Empty;
            if (input.FilterIndex.HasValue)
            {
                var previousItemFilter = new ItemFilters
                {
                    Limit = nbChild,
                    ParentId = itemDataModel.Id,
                    SortAscending = true,
                    IndexGt = input.FilterIndex,
                    States = input.States,
                    Tags= input.Tags,
                };

                var previousChilds = await data.ItemRepository.GetItemsAsync(input.SiteId, previousItemFilter);
                hasPrevious = previousChilds.Count > 0;
                if (previousChilds.Count == nbChild)
                {
                    idPrevious = GetMaxIndex(previousChilds).ToString();
                }
            }
            var idNext = GetMinIndex(childs).ToString();
            var hasNext = nbNext > 0;
            var result = new GetNewsItemSummaries<T>()
            {
                HasNext = hasNext,
                HasPrevious = hasPrevious,
                IdNext = idNext,
                IdPrevious = idPrevious,
                Items = newsItemResult
            };
            return result;
        }

        private static int GetMaxIndex(IList<ItemDataModel> childs)
        {
            if (childs == null || childs.Count <= 0)
            {
                return 0;
            }
            var maxIndex = childs.Max(c => c.Index);
            return maxIndex;
        }

        private static async Task<long> NbNext(IDataFactory data, string siteId, IList<ItemDataModel> childs, ItemDataModel itemDataModel, IList<ItemState> states, IList<string> tags)
        {
            var minIndex = GetMinIndex(childs);
            var nbNext = await data.ItemRepository.CountItemsAsync(siteId, new CountItemFilters
            {
                ParentId = itemDataModel.Id,
                IndexLt = minIndex,
                States = states,
                Tags = tags,
            });
            return nbNext;
        }

        private static int GetMinIndex(IList<ItemDataModel> childs)
        {
            if (childs == null || childs.Count <= 0)
            {
                return 0;
            }
            var minIndex = childs.Min(c => c.Index);
            return minIndex;
        }

        private  async Task<T> GetItemsAsync<T,G>(ItemDataModel dataModel) where T : GetNewsItemSummary, new() where G : NewsItemBusinessModel, new()
        {
            var newsItem = dataModel.Data as G;
            if (newsItem != null)
            {
                var userInfoNewsItem = await GetFreeCommand.GetUserInfoAsync(_userService, newsItem);
                var getNewsItemResult = new T();
                var elements = newsItem.Elements;

                await UpdateElementAsync(dataModel.SiteId, elements, _dataFactory.ItemRepository);

                getNewsItemResult.Elements = elements;
                getNewsItemResult.State = dataModel.State;
                getNewsItemResult.Tags = dataModel.Tags;
                getNewsItemResult.ModuleId = dataModel.Id;
                getNewsItemResult.CreateDate = dataModel.CreateDate;
                getNewsItemResult.UpdateDate = dataModel.UpdateDate;
                getNewsItemResult.UserInfo = userInfoNewsItem.UserInfo;
                getNewsItemResult.LastUpdateUserInfo = userInfoNewsItem.LastUpdateUserInfo;
                return getNewsItemResult;
            }
            return null;
        }

        public static async Task<IList<Element>> UpdateElementAsync(string siteId, IList<Element> elements, IItemRepository itemRepository) 
        {
            //TODO Remove
            var metaKeywords = elements.FirstOrDefault(e => e.Property == "MetaKeyword");
            if (metaKeywords != null)
            {
                elements.Remove(metaKeywords);
            }
            FreeBusinessModule.UpdateContent(elements, "h1");
            FreeBusinessModule.UpdateContent(elements, "p", "Sans text");
          //  await CleanElementRecursiveAsync(siteId, elements, itemRepository);
            return elements;
        }

      /*  private static async Task CleanElementRecursiveAsync(string siteId, IList<Element> inputElements, IItemRepository itemRepository)
        {
            foreach (var element in inputElements)
            {
                var type = element.Type;
                // Pour les fichiers temporaire on les passe en sous module de ce module
                if (SaveFreeCommand.IsFileElementType(type))
                {
                   await GetElementFileAsync(siteId, element, itemRepository);
                }
                else
                {
                    if (element.Childs != null && element.Childs.Count > 0)
                    {
                        await CleanElementRecursiveAsync(siteId, element.Childs, itemRepository);
                    }
                }
            }
        }

        private static async Task<Element> GetElementFileAsync(string siteId, Element element, IItemRepository itemRepository)
        {
            var files = JsonConvert.DeserializeObject<List<DataFileInput>>(element.Data);
            var results = new List<DataFileInput>();
            foreach (var dataFile in files)
            {
                var newDataFile = dataFile;
                var module = dataFile.Type.ToLower().Contains("video")? "VideoData" : "";
                if (!string.IsNullOrEmpty(module))
                {
                    var file = (await itemRepository.DownloadsAsync(siteId, dataFile.Id, false, true)).FirstOrDefault();
                    if (file != null)
                    {
                        newDataFile = new DataFileVideoInput(dataFile, file.FileData.Url);
                    }
                }
                results.Add(newDataFile);
            }

            element.Data = JsonConvert.SerializeObject(results);
            return element;
        }
        */
    }
}