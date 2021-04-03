using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Business.Command;
using Demo.Common.Command;
using Demo.Mvc.Core.Sites.Data;
using Demo.Mvc.Core.Sites.Data.Model;
using Demo.Mvc.Core.Sites.Data.Repository;
using Demo.Mvc.Core.User;
using Demo.User.Identity;

namespace Demo.Mvc.Core.Sites.Core.Command.Site.Seo
{
    public class GetSeoCommand : Command<UserInput<GetSeoInput>, CommandResult<SeoBusinessModel>>
    {
        private readonly IDataFactory _dataFactory;
        private readonly UserService _userService;

        public GetSeoCommand(IDataFactory dataFactory, UserService userService)
        {
            _dataFactory = dataFactory;
            _userService = userService;
        }

        protected override async Task ActionAsync()
        {
            var siteId = Input.Data.SiteId;

            if (Input.Data.IsVerifyAuthorisation)
            {
                await UserSecurity.CheckAdministratorAsync(_userService, Input.UserId, siteId);
            }

            Result.Data = await LoadSeoBusinessModelAsync(_dataFactory, siteId);
        }

        public static async Task<ItemDataModel> GetSeoItemDataModelAsync(IDataFactory datafactory, string siteId,
            bool hasTracking = false)
        {
            var itemsDb = await datafactory.ItemRepository.GetItemsAsync(siteId, new ItemFilters
            {
                Module = "Seo",
                Limit = 1,
                HasTracking = hasTracking
            });

            return itemsDb.FirstOrDefault();
        }

        public static async Task<SeoBusinessModel> LoadSeoBusinessModelAsync(IDataFactory datafactory, string siteId)
        {
            var itemDataModel = await GetSeoItemDataModelAsync(datafactory, siteId);

            SeoBusinessModel seoBusinessModel = null;
            if (itemDataModel != null && itemDataModel.Data != null)
            {
                seoBusinessModel = itemDataModel.Data as SeoBusinessModel;
            }
            else
            {
                seoBusinessModel = new SeoBusinessModel
                {
                    Disallows = new List<string>(),
                    Metas = new List<SeoValidationMeta>()
                };
                seoBusinessModel.Metas.Add(new SeoValidationMeta {Engine = SeoEngine.Google});
                seoBusinessModel.Metas.Add(new SeoValidationMeta {Engine = SeoEngine.Bing});
            }

            if (seoBusinessModel.Redirects == null)
            {
                seoBusinessModel.Redirects = new  List<SeoRedirect>();
            }

            return seoBusinessModel;
        }
    }
}