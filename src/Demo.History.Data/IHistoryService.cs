using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Data.History.Models;

namespace Demo.Data.History
{
    public interface IHistoryService
    {
        Task AddAsync(string id, CheckPoint checkPoint);

        Task<HistoryDbModel> GetHistoryAsync(string id);
    }
}