using System;
using System.Collections.Generic;
using Demo.Mvc.Core.Routing.Implementation;
using Demo.Mvc.Core.Sites.Data.Model;

namespace Demo.Mvc.Core.Sites.Core.Models.Page
{
    public class MenuItemBusiness
    {
        public string RoutePath { get; set; }
        public Route Route { get; set; }
        public string RoutePathWithoutHomePage { get; set; }
        public string ModuleName { get; set; }
        public IList<MenuItemBusiness> Childs { get; set; }
        public ItemState State { get; set; }
        public Object Data { get; set; }

        /// <summary>
        ///     Liste des action diponible sur la page
        /// </summary>
        public IDictionary<string, ActionBusiness> Actions { get; set; }

        /// <summary>
        ///     Module Id
        /// </summary>
        public string ModuleId { get; set; }

        /// <summary>
        ///     Liste information sur les routes
        /// </summary>
        public IDictionary<string, string> RouteDatas { get; set; }

        public SeoBusiness Seo { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public TypeMenuItem? TypeMenuItem { get; set; }
        public string Icon { get; set; }
    }
}