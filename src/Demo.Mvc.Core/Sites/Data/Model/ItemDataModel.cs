using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Demo.Mvc.Core.Sites.Data.Model
{
    public class ItemDataModel : ItemDataModelBase
    {
        public ItemDataModel()
        {
            var childs = new ObservableCollection<ItemDataModel>();
            childs.CollectionChanged += childs_CollectionChanged;
            Childs = childs;

            var files = new ObservableCollection<FileDataModel>();
            files.CollectionChanged += files_CollectionChanged;
            Files = files;

            var tags = new ObservableCollection<string>();
            tags.CollectionChanged += tags_CollectionChanged;
            Tags = tags;
        }

        private readonly Item item;

        public ItemDataModel(Item item, bool hasTracking) : this()
        {
            this.item = item;
            index = item.Index;
            isTemporary = item.IsTemporary;
            data = MemoryDatabase.GetItemData(item);
            dataSource = MemoryDatabase.GetItemData(item); 
            module = item.Module;
            _parentId = item.ParentId;
            propertyName = item.PropertyName;
            _siteId = item.SiteId;
            CreateDate = item.CreateDate;
            UpdateDate = item.UpdateDate;
            Id = item.Id;
            State = item.State;
            IsLoadedFromDatabase = true;
            if (item.Tags != null)
            {
                //Tags = item.Tags;
                //Tags.CollectionChanged -= tags_CollectionChanged;
                foreach (var itemTag in item.Tags)
                {
                    Tags.Add(itemTag);  
                }
            }
            HasTracking = hasTracking;
        }
       

        internal override bool HasChange
        {
            get
            {
                if (base.HasChange)
                {
                    return true;
                }

                return false;
            }
            set { hasCHange = value; }
        }

        public IList<ItemDataModel> Childs { get; }
        public IList<FileDataModel> Files { get; }

        private void childs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var itemDataModel in Childs)
            {
                if (itemDataModel.Parent != this)
                {
                    itemDataModel.Parent = this;
                }
            }
        }

        private void files_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (var itemDataModel in Files)
            {
                if (itemDataModel.Parent != this)
                {
                    itemDataModel.Parent = this;
                }
            }
        }

        public IList<string> Tags { get; }

        private void tags_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            HasChange = true;
        }

        ~ItemDataModel()
        {
            ((ObservableCollection<ItemDataModel>) Childs).CollectionChanged -= childs_CollectionChanged;
            ((ObservableCollection<FileDataModel>) Files).CollectionChanged -= files_CollectionChanged;
            ((ObservableCollection<string>) Tags).CollectionChanged -= tags_CollectionChanged;
        }
    }
}