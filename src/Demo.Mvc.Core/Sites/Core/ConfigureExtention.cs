﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Demo.Mvc.Core.Routing;
using Demo.Mvc.Core.Sites.Core.BusinessModule;
using Demo.Mvc.Core.Sites.Core.Routing;
using Demo.Mvc.Core.Sites.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Mvc.Core.Sites.Core
{
    public static class ConfigureExtention
    {
        private static IList<Type> ListCommands(Assembly assembly)
        {
            var commands = new List<Type>();

            foreach (var type in assembly.GetTypes())
            {
                if (type.Name.EndsWith("Command"))
                {
                if (type.IsAbstract || type.IsInterface)
                                {
                                    continue;
                                }
                    commands.Add(type);
                }
            }

            return commands;
        }

        private static IList<Type> ListModules(Assembly assembly)
        {
            var commands = new List<Type>();

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsAbstract || type.IsInterface)
                {
                    continue;
                }
                var interfaces = type.GetInterfaces().ToList();
                if (interfaces.Contains(typeof(IBusinessModule)) || interfaces.Contains(typeof(IBusinessModuleCreate)))
                {
                    
                    commands.Add(type);
                }
            }

            return commands;
        }
        
        public static void ConfigureSiteCore(this IServiceCollection services, IConfiguration Configuration)
        {
            var type = typeof(ConfigureExtention);
            var assembly = type.Assembly;
             var commands = ListCommands(assembly);
            foreach (var command in commands)
            {
                services.AddTransient(command, command);
            }
            
            var modules = ListModules(assembly);
            foreach (var module in modules)
            {
                services.AddTransient(module, module);
            }
            
            services.AddTransient<CacheProvider, CacheProvider>();
            services.AddTransient<IRouteProvider, RouteProvider>();
            services.AddTransient<IRouteManager, RouteManager>();
            services.AddTransient<UrlProvider, UrlProvider>();
            services.AddTransient<BusinessModuleFactory, BusinessModuleFactory>();
            services.AddTransient<BusinessFactory, BusinessFactory>();
            services.AddTransient<ModuleManager, ModuleManager>();
            services.ConfigureSiteData(Configuration);
        }
    }
}