using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Demo.Data.Stat.Models;

namespace Demo.Mvc.Core.Stats.Data
{
    public interface IStatService
    {
        Task AddAsync(StatDbModel messageDbModel);
        Task<IList<StatDbModel>> GetStatsync(DateTime beginDate, DateTime endDate, string siteId);
    }
}