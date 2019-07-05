using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Log
{
    public interface ILogService
    {
        Task<IList<Log>> GetLogs(GetLogsInput getLogsInput = null);
        Task ClearLogsAsync();
    }
}