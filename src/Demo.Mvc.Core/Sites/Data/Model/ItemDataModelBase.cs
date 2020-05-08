using KellermanSoftware.CompareNetObjects;

namespace Demo.Mvc.Core.Sites.Data.Model
{
    public abstract class ItemDataModelBase : DataModelBase
    {
        protected string _parentId;
        protected string _siteId;
        protected object data;
        protected object dataSource;
        protected bool hasCHange;
        protected int index;
        protected bool isTemporary;
        protected ItemState state = ItemState.Published;
        protected string module;
        protected ItemDataModel parent;
        protected string propertyName;
        protected ItemDataModel site;

        internal bool HasTracking { get; set; }

        internal virtual bool HasChange
        {
            get
            {
                if (hasCHange)
                {
                    return true;
                }

                if (dataSource != null)
                {
                    var compareLogic = new CompareLogic();
                    var result = compareLogic.Compare(data, dataSource);
                    return !result.AreEqual;
                }

                return false;
            }
            set { hasCHange = value; }
        }

        public string PropertyName
        {
            get { return propertyName; }
            set
            {
                if (propertyName != value)
                {
                    propertyName = value;
                    HasChange = true;
                }
            }
        }

        public string Module
        {
            get { return module; }
            set
            {
                if (module != value)
                {
                    module = value;
                    HasChange = true;
                }
            }
        }

        public object Data
        {
            get { return data; }
            set
            {
                if (dataSource == null && HasTracking)
                {
                    dataSource = value;
                }

                data = value;
            }
        }

        public PropertyType PropertyType
        {
            get
            {
                if (string.IsNullOrEmpty(PropertyName))
                {
                    return PropertyType.None;
                }
                if (PropertyName.EndsWith("s"))
                {
                    return PropertyType.List;
                }
                return PropertyType.Property;
            }
        }

        public string SiteId
        {
            get
            {
                if (Site != null)
                {
                    return Site.Id;
                }
                return _siteId;
            }
            set
            {
                if (Site != null && Site.Id == value)
                {
                    return;
                }
                Site = null;

                if (_siteId != value)
                {
                    HasChange = true;
                    _siteId = value;
                }
            }
        }

        public ItemDataModel Site
        {
            get { return site; }
            set
            {
                if (site != value)
                {
                    HasChange = true;
                    site = value;
                }
            }
        }

        public ItemDataModel Parent
        {
            get { return parent; }
            set
            {
                if (parent != value)
                {
                    HasChange = true;
                    parent = value;
                    if (parent != null)
                    {
                        if (this is ItemDataModel)
                        {
                            var self = (ItemDataModel) this;
                            if (self != null && !parent.Childs.Contains(self))
                            {
                                parent.Childs.Add(self);
                            }
                        }

                        if (this is FileDataModel)
                        {
                            var self = (FileDataModel) this;
                            if (self != null && !parent.Files.Contains(self))
                            {
                                parent.Files.Add(self);
                            }
                        }
                    }
                }
            }
        }

        public string ParentId
        {
            get
            {
                if (Parent != null)
                {
                    return Parent.Id;
                }
                return _parentId;
            }
            set
            {
                if (Parent != null && Parent.Id == value)
                {
                    return;
                }
                Parent = null;

                if (_parentId != value)
                {
                    HasChange = true;
                    _parentId = value;
                }
            }
        }

        public int Index
        {
            get { return index; }
            set
            {
                if (index != value)
                {
                    HasChange = true;
                    index = value;
                }
            }
        }

        public bool IsTemporary
        {
            get { return isTemporary; }
            set
            {
                if (isTemporary != value)
                {
                    HasChange = true;
                    isTemporary = value;
                }
            }
        }

        public ItemState State
        {
            get { return state; }
            set
            {
                if (state != value)
                {
                    HasChange = true;
                    state = value;
                }
            }
        }
    }
}