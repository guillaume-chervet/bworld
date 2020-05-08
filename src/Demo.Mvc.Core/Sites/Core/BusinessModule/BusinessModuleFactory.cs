using System;
using System.Collections.Generic;
using System.Linq;

namespace Demo.Mvc.Core.Sites.Core.BusinessModule
{
    public class BusinessModuleFactory
    {
        private readonly IServiceProvider _unityContainer;

        public BusinessModuleFactory(IServiceProvider unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public IBusinessModule GetModule(string moduleName)
        {
            var assembly = GetType().Assembly;

            foreach (var type in assembly.GetTypes())
            {
                if (type.Name == moduleName + "BusinessModule")
                {
                    return _unityContainer.GetService(type) as IBusinessModule;
                }
            }

            return null;
        }

        public IBusinessModuleCreate GetModuleCreate(string moduleName)
        {
            var assembly = GetType().Assembly;

            foreach (var type in assembly.GetTypes())
            {
                if (type.Name == moduleName + "BusinessModule")
                {
                    return _unityContainer.GetService(type) as IBusinessModuleCreate;
                }
            }

            return null;
        }

        public IList<IBusinessModule> GetModules()
        {
            var assembly = GetType().Assembly;

            var businessModuleType = typeof(IBusinessModule);

            IList<IBusinessModule> list = new List<IBusinessModule>();
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetInterfaces().Contains(businessModuleType) && !type.IsAbstract)
                {
                    list.Add((IBusinessModule) _unityContainer.GetService(type));
                }
            }

            return list;
        }
    }
}