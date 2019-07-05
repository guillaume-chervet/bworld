using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Data.Stat.Models;

namespace Demo.Data.Stat
{
    public interface IStatService
    {
        Task AddAsync(StatDbModel messageDbModel);
        Task<IList<StatDbModel>> GetStatsync(DateTime beginDate, DateTime endDate, string siteId);
    }
}