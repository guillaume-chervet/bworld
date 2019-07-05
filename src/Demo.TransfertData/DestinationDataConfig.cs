using System;
using System.Configuration;
using Demo.Data;
using Demo.Data.Mongo;

namespace Demo.TransfertData
{
    public class DestinationDataConfig : IDataConfig, IAzureConfig
    {

        public string MongoConnectionString { get; }
        public string MongoDatabaseName { get; }
        public string StorageConnectionString { get; }

        /// <summary>
        ///     Lit un paramètres de type string dans le fichier de configuration
        /// </summary>
        /// <param name="key">Clé du fichier Web.config</param>
        /// <returns>Valeure associée à la clé</returns>
        private static string ReadString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }

            var value = ConfigurationManager.AppSettings[key];
            if (value == null)
            {
                throw new ConfigurationErrorsException("la clef n'existe pas dans le fichier de configuration : " + key);
            }

            return value;
        }

    }
}