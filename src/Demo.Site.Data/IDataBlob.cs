using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Data.Model;

namespace Demo.Data
{   
    public interface IDataBlob
    {
        Task<IList<FileDataModel>> DownloadsAsync(string siteId = null, string parentId = null, bool isGetStream = true, bool isGetUrl = false);
        Task<FileDataModel> DownloadAsync(string siteId, string parentId, string propertyName, string module, bool isGetStream = true, bool isGetUrl = false);
        Task<FileDataModel> DownloadAsync(string siteId, string id, bool isGetStream = true, bool isGetUrl = false);
        Task UploadAsync(FileDataModel fileDataModel, bool isUploadOnly = false);
        Task DeleteAsync(string id);

    }
}