using System.Collections.Generic;

namespace Demo.Mvc.Core.Sites.Core.Models.Page
{
    public class ActionBusiness
    {
        #region Public Properties

        /// <summary>
        ///     Liste information sur les routes
        /// </summary>
        public IDictionary<string, string> RouteDatas { get; set; }

        public string Type { get; set; }

        public string Url { get; set; }

        #endregion
    }
}