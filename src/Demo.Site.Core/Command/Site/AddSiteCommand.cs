using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Business.Renderers;
using Demo.Business.Routing;
using Demo.Common;
using Demo.Common.Command;
using Demo.Data;
using Demo.Data.Model;
using Demo.Email;
using Demo.Renderer;
using Demo.Routing;
using Demo.Routing.Extentions;
using Demo.Routing.Models;
using Demo.User;
using Demo.Data.Repository;
using Demo.User.Identity;
using Demo.User.Site;
using Demo.Business.Command.Administration.User;
using Demo.Routing.Interfaces;

namespace Demo.Business.Command.Site
{
    /// <summary>
    ///     Command qui créer un site depuis l'interface utilisateur
    /// </summary>
    public class AddSiteCommand : Command<UserInput<AddSiteInput>, CommandResult<dynamic>>
    {
        private readonly CacheProvider _cacheProvider;
        private readonly SiteUserService _siteUserService;
        private readonly BusinessModuleFactory _businessModuleFactory;
        private readonly IRouteManager _routeManager;
        private readonly IRouteProvider _routeProvider;
        private readonly IDataFactory _dataFactorySource;
        private readonly IDataFactory _dataFactoryDestination;
        private readonly IEmailService _emailService;
        private readonly UserService _userService;

        public AddSiteCommand(IDataFactory dataFactorySource, IDataFactory dataFactoryDestination,
            UserService userService, IEmailService emailService, CacheProvider cacheProvider, SiteUserService siteUserService, BusinessModuleFactory businessModuleFactory, IRouteManager routeManager, IRouteProvider routeProvider)
        {
            _dataFactorySource = dataFactorySource;
            _dataFactoryDestination = dataFactoryDestination;
            _userService = userService;
            _emailService = emailService;
            _cacheProvider = cacheProvider;
            _siteUserService = siteUserService;
            _businessModuleFactory = businessModuleFactory;
            _routeManager = routeManager;
            _routeProvider = routeProvider;
        }

        protected override async Task ActionAsync()
        {
            if (string.IsNullOrEmpty(Input.Data.SiteName))
            {
                throw new ArgumentException("SiteName is empty");
            }

            if (string.IsNullOrEmpty(Input.Data.ModuleId))
            {
                throw new ArgumentException("ModuleId is empty");
            }

            if (string.IsNullOrEmpty(Input.Data.CategoryId))
            {
                throw new ArgumentException("CategoryId is empty");
            }

            var checkAddSiteInput = new CheckAddSiteInput
            {
                CategoryId = Input.Data.CategoryId,
                ModuleId = Input.Data.ModuleId,
                SiteId = Input.Data.SiteId,
                SiteName = Input.Data.SiteName
            };

            if (await CheckAddSiteCommand.IsSiteAlreadyExistAsync(_dataFactorySource, _dataFactoryDestination, checkAddSiteInput))
            {
                Result.ValidationResult.AddError("SITE_ADRESS_ALREADY_EXIST", "L'adresse demandé existe déjà");
            }

            var addSiteItemBusinessModel =
                (AddSiteBusinessModel)
                    (await _dataFactorySource.ItemRepository.GetItemAsync(Input.Data.Site.SiteId, Input.Data.ModuleId)).Data;
            var template = addSiteItemBusinessModel.Templates.First(t => t.CategoryId == Input.Data.CategoryId);
            var newSiteName = template.Title + " " + Input.Data.SiteName;

            var itemDataModelCurrentSite = await TransfertSiteCommand.GetSiteAsync(template.SiteId, _dataFactorySource);
            var currentSiteBusinessModel = (SiteBusinessModel) itemDataModelCurrentSite.Data;

            #region duplication du site

            var createFromSiteModel = new CreateFromSiteModel();
            createFromSiteModel.CategoryId = template.CategoryId;
            createFromSiteModel.SiteName = newSiteName;
            createFromSiteModel.Title = Input.Data.SiteName;
            createFromSiteModel.UserId = Input.UserId;

            var moduleSite = _businessModuleFactory.GetModuleCreate(itemDataModelCurrentSite.Module);
            var siteItemDataModel = await moduleSite.CreateFromAsync(_dataFactorySource, _dataFactoryDestination,
                itemDataModelCurrentSite, null, false, createFromSiteModel);

            #endregion

            await _dataFactoryDestination.SaveChangeAsync();
            await _cacheProvider.UpdateCacheAsync(siteItemDataModel.Id);

            #region Association Site/User

            await SaveSiteUserCommand.CreateNewSiteUserAsync(_siteUserService, _userService, siteItemDataModel.Id, Input.UserId);

            #endregion

            #region Generation Url nouveaux site

            var siteBusinessModel = (SiteBusinessModel) siteItemDataModel.Data;

            var input = new FindPathInput();
            input.DomainDatas = new Dictionary<string, string>();
            input.DomainDatas.Add("site", UrlHelper.NormalizeTextForUrl(newSiteName));

            if (Input.Data.Port.HasValue)
            {
                input.Port = Input.Data.Port.ToString();
            }
            //TODO règle temporaire pour le développement
            var isLocalhost = Input.Data.Site.DomainDatas.ContainsKey("domain") &&
                              Input.Data.Site.DomainDatas["domain"] == "localhost";
            if (isLocalhost)
            {
                input.DomainDatas.Add("domain", Input.Data.Site.DomainDatas["domain"]);
            }
            input.MasterDomainId = siteBusinessModel.MasterDomainId;

            input.IsSecure = null;


            input.Datas = await _routeProvider.GetRootMetadataAsync(siteItemDataModel.Id);

            var result = await _routeManager.FindDomainPathAsync(input);

            #endregion

            // On reconstitue l'Url
            var protocole = Input.Data.IsSecure ? _routeProvider.ProtocolSecure : _routeProvider.ProtocolDefault;
            var port = string.Empty;

            if (Input.Data.Port.HasValue && Input.Data.Port.Value != 80 && Input.Data.Port.Value != 443)
            {
                port = string.Concat(":", Input.Data.Port.Value.ToString(CultureInfo.InvariantCulture));
            }
            var siteUrl = string.Concat(protocole, "://",
                UrlHelper.Concat(string.Concat(result.RequestDomain, port), result.Path));
            await SendEmailAsync(newSiteName, siteUrl, Input.UserId);
            Result.Data = UrlHelper.Concat(siteUrl, "/site/creation/confirmation?dm=false");
        }

        public async Task SendEmailAsync(string siteName, string siteUrl, string userId)
        {
            var user = await _userService.FindApplicationUserByIdAsync(userId);

            // TODO Mettre un template
            var identityMessage = new MailMessage();

            identityMessage.Subject = string.Concat("[bworld] Création de votre site ", siteName);

            var createSiteMailModel = new CreateSiteMailModel();
            createSiteMailModel.SiteName = siteName;
            createSiteMailModel.SiteUrl = siteUrl;
            createSiteMailModel.SiteUrlAdmin = UrlHelper.Concat(siteUrl, "administration");
            createSiteMailModel.UserName = user.FullName;
            var createSiteTempate = ResourcesLoader.Load(Path.Combine("Renderers", "CreateSite.st"));
            identityMessage.Body = new StringTemplateRenderer().Render(
                createSiteTempate, createSiteMailModel);

            identityMessage.Destination = user.Email;

            await _emailService.SendAsync(identityMessage);
        }

        public static async Task<ItemDataModel> DuplicateItemAsync(IDataFactory dataFactoryDestination,
            ItemDataModel item,
            ItemDataModel parentItemDestination, bool isTransfert, object data)
        {
            ItemDataModel itemDestination = null;

            // Dans le cas d'un transfert de données
            if (isTransfert)
            {
                if ((parentItemDestination == null || !string.IsNullOrEmpty(parentItemDestination.Id)) &&
                    !string.IsNullOrEmpty(item.Id))
                {
                    itemDestination = await dataFactoryDestination.ItemRepository.GetItemAsync(item.SiteId, item.Id);
                }

                if (itemDestination == null)
                {
                    itemDestination = new ItemDataModel();
                    itemDestination.Id = item.Id;
                }
            }

            else
            {
                itemDestination = new ItemDataModel();
            }

            itemDestination.Index = item.Index;
            if (item.Data != null)
            {
                itemDestination.Data = CloneHelper.DeepCopy(item.Data);
            }
            itemDestination.Module = item.Module;
            itemDestination.PropertyName = item.PropertyName;
            if (parentItemDestination != null)
            {
                if (parentItemDestination.Site == null)
                {
                    itemDestination.Site = parentItemDestination;
                }
                else
                {
                    itemDestination.Site = parentItemDestination.Site;
                }
                itemDestination.Parent = parentItemDestination;
            }
            dataFactoryDestination.Add(itemDestination);

            return itemDestination;
        }
    }
}