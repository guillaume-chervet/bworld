﻿using System.Collections.Generic;
using Demo.Mvc.Core.Sites.Core.Command.Free.Models;

namespace Demo.Mvc.Core.Sites.Core.Command.Site
{
    public class SaveAddSiteInput
    {
        public string ModuleId { get; set; }
        public IList<SiteTemplate> Templates { get; set; }
        public string UrlConditionsGeneralesUtilisations { get; set; }
        public IList<Element> Elements { get; set; }
        public CurrentRequest Site { get; set; }
    }
}