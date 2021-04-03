using System;
using System.IO;
using System.Reflection;

namespace Demo.Mvc.Core.Common
{
    public class ResourcesLoader
    {
        
        private static string GetAssemblyDirectory()
        {
            var codeBase = Assembly.GetExecutingAssembly().CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
        
        /// <summary>
        /// Retour toutes les règles désérialisé
        /// </summary>
        /// <returns>Liste de règle</returns>
        public static string Load(string localPath)
        {
                var jsonPath = Path.Combine(GetAssemblyDirectory(), localPath); 
                return File.ReadAllText(jsonPath);
        }
    }
}