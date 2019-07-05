using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Demo.Routing;
using Demo.Routing.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Business
{
    public static class ConfigureExtention{
   
        public static void ConfigureRouting(this IServiceCollection services)
        {
            services.AddTransient<IRouteManager, RouteManager>();
        }
    }
}