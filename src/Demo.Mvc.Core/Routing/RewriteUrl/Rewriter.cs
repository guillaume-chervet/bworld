﻿using System.Collections.Generic;
using Demo.Mvc.Core.Routing.Implementation;

namespace Demo.Mvc.Core.Routing.RewriteUrl
{
    public class Rewriter
    {
        public string Map(string routePath, Route route, IDictionary<string, string> datas)
        {
            if (route != null && !string.IsNullOrEmpty(route.RewritePath))
            {
                var path = RouteManager.GetPath(route.RewritePath, datas);
                return path;
            }

            return routePath;
        }

        public string Unmap(IDictionary<string, string> map, string path)
        {
            if (map != null && map.ContainsKey(path)) return map[path];
            return path;
        }
    }
}