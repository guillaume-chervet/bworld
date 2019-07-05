using System;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Common.Command;
using Demo.Data;

namespace Demo.Business.Command.Site.Cache
{
    public class ResetSiteCacheCommand : Command<ResetSiteCacheInput, CommandResult<dynamic>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly CacheProvider _cacheProvider;

        public ResetSiteCacheCommand(IDataFactory dataFactory, CacheProvider cacheProvider)
        {
            _dataFactory = dataFactory;
            _cacheProvider = cacheProvider;
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }

        protected override async Task ActionAsync()
        {
              Result.Data = await (new ModuleManager(_dataFactory, _cacheProvider)).GetMasterAsync(Input.Site);
        }
    }
}