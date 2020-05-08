using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Mvc.Core.Sites.Core.BusinessModule;
using Demo.Mvc.Core.Sites.Data;

namespace Demo.Mvc.Core.Sites.Core.Command.Site.Cache
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

        protected override async Task ActionAsync()
        {
              Result.Data = await (new ModuleManager(_dataFactory, _cacheProvider)).GetMasterAsync(Input.Site);
        }
    }
}