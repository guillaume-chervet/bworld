using System;
using System.Linq;
using System.Threading.Tasks;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Repository;
using Demo.User;
using Demo.User.Identity;

namespace Demo.Business.Command.Site
{
    public class CheckAddSiteCommand : Command<CheckAddSiteInput, CommandResult<dynamic>>
    {
        private readonly IDataFactory _dataFactoryDestination;
        private readonly IDataFactory _dataFactorySource;

        public CheckAddSiteCommand(IDataFactory dataFactorySource, IDataFactory dataFactoryDestination)
        {
            _dataFactorySource = dataFactorySource;
            _dataFactoryDestination = dataFactoryDestination;
        }

        protected override async Task ActionAsync()
        {
            if (string.IsNullOrEmpty(Input.SiteName))
            {
                throw new ArgumentException("SiteName is empty");
            }

            if (string.IsNullOrEmpty(Input.ModuleId))
            {
                throw new ArgumentException("ModuleId is empty");
            }

            if (string.IsNullOrEmpty(Input.CategoryId))
            {
                throw new ArgumentException("CategoryId is empty");
            }

            var isSiteAlreadyExist = await IsSiteAlreadyExistAsync(_dataFactorySource, _dataFactoryDestination, Input);
            if (isSiteAlreadyExist)
            {
                Result.ValidationResult.AddError("SITE_ADRESS_ALREADY_EXIST", "L'adresse demandé existe déjà");
            }
        }

        protected override void Action()
        {
            throw new NotImplementedException();
        }

        internal static async Task<bool> IsSiteAlreadyExistAsync(IDataFactory dataFactorySource,
            IDataFactory dataFactoryDestination, CheckAddSiteInput checkAddSiteInput)
        {
            var addSiteItemBusinessModel =
                (AddSiteBusinessModel)
                    (await
                        dataFactorySource.ItemRepository.GetItemAsync(checkAddSiteInput.SiteId,
                            checkAddSiteInput.ModuleId)).Data;
            var template = addSiteItemBusinessModel.Templates.First(t => t.CategoryId == checkAddSiteInput.CategoryId);
            var newSiteName = template.Title + " " + checkAddSiteInput.SiteName;

            var itemDataModelCurrentSite = await TransfertSiteCommand.GetSiteAsync(template.SiteId, dataFactorySource);
            var currentSiteBusinessModel = (SiteBusinessModel) itemDataModelCurrentSite.Data;

            #region Sécurité

            // On controle si le nom de site site n'existe pas déjà pour le domaine
            {
                var siteRepository = dataFactoryDestination.ItemRepository;
                var siteDataModels = await siteRepository.GetItemsAsync(null, new ItemFilters {Module = "Site"});

                foreach (var itemDataModelSite in siteDataModels)
                {
                    var siteBusinessModelTest = (SiteBusinessModel) itemDataModelSite.Data;
                    if (siteBusinessModelTest.MasterDomainId == currentSiteBusinessModel.MasterDomainId
                        && siteBusinessModelTest.Name.ToLower() == newSiteName.ToLower())
                    {
                        return true;
                    }
                }
            }

            #endregion

            return false;
        }
    }
}