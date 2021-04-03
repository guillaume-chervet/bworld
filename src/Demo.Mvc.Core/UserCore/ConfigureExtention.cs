using System;
using System.Collections.Generic;
using System.Reflection;
using Demo.Mvc.Core.User;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Mvc.Core.UserCore
{
    public static class ConfigureExtention
    {
        private static IList<Type> ListCommands(Assembly assembly)
        {
            var commands = new List<Type>();

            foreach (var type in assembly.GetTypes())
            {
                if (type.Name.EndsWith("Command") && type.Namespace.Contains("UserCore"))
                {
                    commands.Add(type);
                }
            }

            return commands;
        }

        
        public static void ConfigureUserCore(this IServiceCollection services)
        {
            var type = typeof(ConfigureExtention);
            var assembly = type.Assembly;
            var commands = ListCommands(assembly);
            foreach (var command in commands)
            {
                services.AddTransient(command, command);
            }
            services.ConfigureUserData();
        }
    }
}