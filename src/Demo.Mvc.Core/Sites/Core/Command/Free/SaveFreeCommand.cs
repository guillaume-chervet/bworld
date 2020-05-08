using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Routing;
using Demo.Mvc.Core.Sites.Core.BusinessModule;
using Demo.Mvc.Core.Sites.Core.BusinessModule.Models;
using Demo.Mvc.Core.Sites.Core.Command.Free.Models;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.Sites.Data.Repository;
using Demo.Mvc.Core.User;
using Demo.User.Identity;
using Newtonsoft.Json;

namespace Demo.Mvc.Core.Sites.Core.Command.Free
{
    public class SaveFreeCommand : Command<UserInput<SaveFreeInput>, CommandResult<dynamic>>
    {
        private readonly CacheProvider _cacheProvider;
        private readonly ModuleManager _moduleManager;
        private readonly FreeBusinessModule _freeBusinessModule;
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public SaveFreeCommand(IDataFactory dataFactory, UserService userService, CacheProvider cacheProvider, ModuleManager moduleManager, FreeBusinessModule freeBusinessModule)
        {
            _dataFactory = dataFactory;
            _userService = userService;
            _cacheProvider = cacheProvider;
            _moduleManager = moduleManager;
            _freeBusinessModule = freeBusinessModule;
        }

        protected override async Task ActionAsync()
        {
            var siteId = Input.Data.Site.SiteId;

            await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, siteId);

            var itemDataModel = await SaveItemDataModelAsync<FreeBusinessModel>(_dataFactory, Input.Data, Input.UserId, FreeBusinessModule.ModuleName);

            await _dataFactory.SaveChangeAsync();

            await _cacheProvider.UpdateCacheAsync(siteId);

            Result.Data = new ExpandoObject();
            Result.Data.Master = await _moduleManager.GetMasterAsync(Input.Data.Site);

            var roots = _freeBusinessModule.GetRootMetadata(new GetRootMetaDataInput
            {
                ItemDataModel = itemDataModel,
                DataFactory = _dataFactory
            });
            Result.Data.Url = RouteManager.GetPath(FreeBusinessModule.Url, roots);
        }

       public static async Task<ItemDataModel> SaveItemDataModelAsync<T>(IDataFactory dataFactory, SaveFreeInput input, string userId, string module) where T : FreeBusinessModel, new()
        {
            var itemDataModel = await Get<T>(input, dataFactory, module);
            var freeBusinessModel = (T) itemDataModel.Data;
            SetAuthor(itemDataModel, freeBusinessModel, userId);

            var elements =
                await GetElementsAsync(dataFactory, itemDataModel, input.Elements);
            // On enregistre l'element
            freeBusinessModel.Elements = elements;
            freeBusinessModel.IsDisplayAuthor = input.IsDisplayAuthor;
            freeBusinessModel.IsDisplaySocial = input.IsDisplaySocial;
            freeBusinessModel.Icon = input.Icon;

            itemDataModel.State = input.State;
            if (input.Tags != null)
            {
                foreach (var tagId in input.Tags)
                {
                    if (!itemDataModel.Tags.Contains(tagId))
                    {
                        itemDataModel.Tags.Add(tagId);
                    }   
                }
                var toRemove = itemDataModel.Tags.Where(tagId => !input.Tags.Contains(tagId)).ToList();
                foreach (var tagId in toRemove)
                {
                    itemDataModel.Tags.Remove(tagId);
                }
            }

            return itemDataModel;
        }

        public static void SetAuthor(ItemDataModel itemDataModel, FreeBusinessModel freeBusinessModel, string userId)
        {
            var isNew = string.IsNullOrEmpty(itemDataModel.Id);

            if (isNew)
            {
                freeBusinessModel.AuthorUserId = userId;
            }
            else
            {
                freeBusinessModel.LastUpdateAuthorUserId = userId;
            }
        }

        public static async Task<ItemDataModel> Get<T>(SaveModuleInputBase input, IDataFactory data, string moduleName)
            where T : class, new()
        {
            var siteId = input.Site.SiteId;
            var parentId = input.ParentId;
            if (string.IsNullOrEmpty(parentId))
            {
                parentId = siteId;
            }

            ItemDataModel itemDataModel;
            if (string.IsNullOrEmpty(input.ModuleId))
            {
                itemDataModel = await InitItemDataModelAsync<T>(data, moduleName, siteId, parentId, input.PropertyName);
            }
            else
            {
                itemDataModel = await data.ItemRepository.GetItemAsync(input.Site.SiteId, input.ModuleId, false, true);
                SavePropertyName(itemDataModel, input.PropertyName);
            }
            return itemDataModel;
        }

        public static async Task<ItemDataModel> InitItemDataModelAsync<T>(IDataFactory data, string moduleName,
             string siteId, string parentId=null, string propertyName=null) where T : class, new()
        {

            if (string.IsNullOrEmpty(parentId))
            {
                parentId = siteId;
            }

            ItemDataModel itemDataModel;
            var maxIndex = await data.ItemRepository.GetMaxChildIndexAsync(siteId, parentId);

            itemDataModel = new ItemDataModel();
            itemDataModel.SiteId = siteId;
            itemDataModel.ParentId = parentId;
            SavePropertyName(itemDataModel, propertyName);
            itemDataModel.Module = moduleName;
            itemDataModel.Index = maxIndex + 1;
            data.Add(itemDataModel);

            var freeBusinessModel = new T();
            itemDataModel.Data = freeBusinessModel;
            return itemDataModel;
        }

        public static void SavePropertyName(ItemDataModel itemDataModel, string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                if (string.IsNullOrEmpty(itemDataModel.PropertyName))
                {
                    itemDataModel.PropertyName = "MenuItems";
                }
            }
            else
            {
                if (itemDataModel.PropertyName != propertyName)
                {
                    itemDataModel.PropertyName = propertyName;
                }
            }
        }

        public static async Task<IList<Element>> GetElementsAsync(IDataFactory data, ItemDataModel itemDataModel,
 IList<Element> inputElements)
        {
            string siteId = itemDataModel.SiteId;
            string moduleId = itemDataModel.Id;
            var elements = new List<Element>();

            // On charge toutes les images pour éliminer au fur et a mesure
            var itemFilesToDelete =
                await data.ItemRepository.GetItemsAsync(siteId, new ItemFilters {Module = "Image", ParentId = moduleId, IsTemporary = false});
            // Ici les images temporaires
            var items =
                await
                    data.ItemRepository.GetItemsAsync(siteId,
                        new ItemFilters { Module = "Image", ParentId = siteId, IsTemporary = true});
            foreach (var dataModel in items)
            {
                if (dataModel.CreateDate.AddHours(72) <= DateTime.Now &&
                    itemFilesToDelete.Count(d => d.Id == dataModel.Id) <= 0)
                {
                    itemFilesToDelete.Add(dataModel);
                }
            }

            // On gère le cas particulié des images
            var tempDataRepository = data.ItemRepository;
            await CleanElementRecursive(itemDataModel, inputElements, tempDataRepository, itemFilesToDelete, elements);

            // On efface les images non utilisées
          /*  foreach (var dataModel in itemFilesToDelete)
            {
                await data.DeleteAsync(dataModel);
            }*/
            // TODO
            return elements;
        }

        private static async Task CleanElementRecursive(ItemDataModel itemDataModel, IList<Element> inputElements,
            IItemRepository tempDataRepository, IList<ItemDataModel> itemFilesToDelete, List<Element> elements)
        {
            foreach (var element in inputElements)
            {
                var type = element.Type;
                // Pour les fichiers temporaire on les passe en sous module de ce module
                if (IsFileElementType(type))
                {
                    var elementFile =
                        await GetElementFileAsync(element, tempDataRepository, itemDataModel, itemFilesToDelete);
                    elements.Add(elementFile);
                }
                else
                {
                    if (element.Childs != null && element.Childs.Count > 0)
                    {
                        var destElements = new List<Element>();
                        await CleanElementRecursive(itemDataModel, element.Childs, tempDataRepository, itemFilesToDelete, destElements);
                        element.Childs = destElements;
                    }

                    elements.Add(element);
                }
            }
        }

        public static bool IsFileElementType(string type)
        {
            return type == "file" || type == "image" || type == "carousel";
        }

        private static async Task<Element> GetElementFileAsync(Element element,
            IItemRepository tempDataRepository, ItemDataModel itemDataModel, IList<ItemDataModel> itemFilesToDelete)
        {
            var files = JsonConvert.DeserializeObject<List<DataFileInput>>(element.Data);
            var fileDatas = new List<FileData>();
            foreach (var dataFile in files)
            {
                ItemDataModel item;
                if (!string.IsNullOrEmpty(dataFile.Id))
                {
                    item = itemFilesToDelete.FirstOrDefault(i => i.Id == dataFile.Id);
                }
                else
                {
                    item = itemFilesToDelete.FirstOrDefault(i => i.PropertyName == dataFile.PropertyName);
                }

                if (item != null && itemFilesToDelete.Contains(item))
                {
                    itemFilesToDelete.Remove(item);
                }

                if (dataFile.IsTemporary)
                {
                    ItemDataModel tempItemData = null;
                    if (!string.IsNullOrEmpty(dataFile.Id))
                    {
                        // TODO a effecer ancien system
                        tempItemData =
                            await tempDataRepository.GetItemAsync(itemDataModel.SiteId, dataFile.Id, false, true);
                    }
                    else if (!string.IsNullOrEmpty(dataFile.PropertyName))
                    {
                        tempItemData = (await tempDataRepository.GetItemsAsync(itemDataModel.SiteId, new ItemFilters
                        {
                            ParentId = itemDataModel.ParentId,
                            PropertyName = dataFile.PropertyName,
                            Module = "Image",
                            HasTracking = true
                        })).First();
                    }

                    if (tempItemData != null)
                    {
                        tempItemData.IsTemporary = false;
                        tempItemData.Parent = itemDataModel;
                    }
                }

                var fileData = new FileData();
                fileData.Id = dataFile.Id;
                fileData.PropertyName = dataFile.PropertyName;
                fileData.Name = dataFile.Name;
                fileData.Size = dataFile.Size;
                fileData.Width = dataFile.Witdh;
                fileData.Height = dataFile.Height;
                fileData.Type = dataFile.Type;
                fileData.Title = dataFile.Title;
                fileData.Description = dataFile.Description;
                fileData.ThumbDisplayMode = dataFile.ThumbDisplayMode;
                fileData.Behavior = dataFile.Behavior;
                fileData.Link = dataFile.Link;
                fileDatas.Add(fileData);
            }

            var elementFile = new Element();
            elementFile.Property = element.Property;
            elementFile.Type = element.Type;
            elementFile.Data = JsonConvert.SerializeObject(fileDatas);
            return elementFile;
        }
        
    }
}