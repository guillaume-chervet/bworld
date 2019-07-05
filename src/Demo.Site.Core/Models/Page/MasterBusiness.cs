namespace Demo.Business.Models.Page
{
    public class MasterBusiness
    {
        #region Public Properties

        public MenuItemBusiness MainMenu { get; set; }

        /// <summary>
        ///     Titre du site
        /// </summary>
        public string Title { get; set; }

        #endregion
    }
}