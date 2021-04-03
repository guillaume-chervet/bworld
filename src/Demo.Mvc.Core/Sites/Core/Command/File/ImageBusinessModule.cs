using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Mvc.Core.Common;
using Demo.Mvc.Core.Sites.Core.BusinessModule;
using Demo.Mvc.Core.Sites.Core.Command.Site;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.Sites.Data.Repository;

namespace Demo.Mvc.Core.Sites.Core.Command.File
{
    public class ImageBusinessModule : IBusinessModuleCreate
    {
        public async Task<ItemDataModel> CreateFromAsync(IDataFactory dataFactorySource,
            IDataFactory dataFactoryDestination,
            ItemDataModel itemDataModelSource,
            ItemDataModel itemDataModelDestinationParent, bool isTransfert, object data = null)
        {
            var itemDataModelDestination = await AddSiteCommand.DuplicateItemAsync(dataFactoryDestination,
                itemDataModelSource, itemDataModelDestinationParent, isTransfert, data);

            if (string.IsNullOrEmpty(itemDataModelDestination.Id))
            {
                // Pour les image on  a besoin de forcer l'id
                itemDataModelDestination.Id = Guid.NewGuid().ToString();
            }

            // TODO a supprimer hiostorique image
            {
                var items =
                    await
                        dataFactorySource.ItemRepository.GetItemsAsync(itemDataModelSource.SiteId,
                            new ItemFilters {ParentId = itemDataModelSource.Id});
                if (items != null)
                {
                    var allLists = items.OrderBy(i => i.Index);
                    foreach (var item in allLists)
                    {
                        await
                            AddSiteCommand.DuplicateItemAsync(dataFactoryDestination, item, itemDataModelDestination,
                                isTransfert,
                                data);
                    }
                }
            }

            // Nouvelles images stream
            {
                var files =
                    await
                        dataFactorySource.ItemRepository.DownloadsAsync(itemDataModelSource.SiteId,
                            itemDataModelSource.Id, true);
                if (files != null)
                {
                    var allLists = files.OrderBy(i => i.Index);
                    foreach (var file in allLists)
                    {
                        await
                            DuplicateFileAsync(dataFactoryDestination, file, itemDataModelDestination,
                                isTransfert,
                                data);
                    }
                }
            }


            return itemDataModelDestination;
        }

        public async Task DeleteAsync(IDataFactory dataFactory, ICacheRepository cacheRepository, ItemDataModel itemDataModel)
        {

            var files =
                   await
                       dataFactory.ItemRepository.DownloadsAsync(itemDataModel.SiteId,
                           itemDataModel.Id);

            foreach (var fileDataModel in files)
            {
                await dataFactory.DeleteFileAsync(fileDataModel.Id);
            }

            await dataFactory.DeleteAsync(itemDataModel);
        }

        public static async Task<FileDataModel> DuplicateFileAsync(IDataFactory dataFactoryDestination,
            FileDataModel file,
            ItemDataModel parentItemDestination, bool isTransfert, object data)
        {
            FileDataModel itemDestination = null;

            // Dans le cas d'un transfert de données
            if (isTransfert)
            {
                if ((parentItemDestination == null || !string.IsNullOrEmpty(parentItemDestination.Id)) &&
                    !string.IsNullOrEmpty(file.Id))
                {
                    itemDestination = await dataFactoryDestination.ItemRepository.DownloadAsync(file.SiteId, file.Id);
                }

                if (itemDestination == null)
                {
                    itemDestination = new FileDataModel();
                    itemDestination.Id = file.Id;
                }
            }
            else
            {
                itemDestination = new FileDataModel();
            }

            itemDestination.Index = file.Index;
            if (file.Data != null)
            {
                itemDestination.Data = CloneHelper.DeepCopy(file.Data);
            }
            itemDestination.Module = file.Module;
            itemDestination.PropertyName = file.PropertyName;
            if (parentItemDestination != null)
            {
                if (parentItemDestination.Site == null)
                {
                    itemDestination.Site = parentItemDestination;
                }
                else
                {
                    itemDestination.Site = parentItemDestination.Site;
                }
                itemDestination.Parent = parentItemDestination;
            }

            itemDestination.FileData.ContentType = file.FileData.ContentType;
            itemDestination.FileData.FileName = file.FileData.FileName;
            itemDestination.FileData.Length = file.FileData.Length;
            itemDestination.FileData.Stream = file.FileData.Stream;

            dataFactoryDestination.Add(itemDestination);

            return itemDestination;
        }
    }
}