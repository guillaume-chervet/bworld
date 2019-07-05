using System;

namespace Demo.Data.Model
{
    public class DataModelBase : IDataModel
    {
        public DateTime CreateDate { get; internal set; }
        public DateTime? UpdateDate { get; internal set; }
        public string Id { get; set; }
        internal bool IsLoadedFromDatabase { get; set; }
    }
}