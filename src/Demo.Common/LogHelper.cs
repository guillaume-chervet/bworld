using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AF.PortailCreditor.Logging
{
    public class LogHelper
    {
        /// <summary>
        ///     Retourne les clé valeur associé au membre de l'objet
        /// </summary>
        /// <param name="poco"></param>
        /// <returns></returns>
        private static IEnumerable<KeyValuePair<string, string>> GetValues(object poco)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();

            if (poco == null)
                return dictionary;

            var type = poco.GetType();
            var propertyInfos =
                type.GetProperties(BindingFlags.SetField | BindingFlags.Instance | BindingFlags.GetField |
                                   BindingFlags.CreateInstance | BindingFlags.Public);

            foreach (var propertyInfo in propertyInfos)
            {
                // Si one to one
                var value = propertyInfo.GetValue(poco, null);
                if (value != null && value.GetType().IsValueType)
                    dictionary.Add(propertyInfo.Name, value.ToString());
                else
                    dictionary.Add(propertyInfo.Name, value as string);
            }

            return dictionary;
        }

        public static string GetObjectLog(object poco)
        {
            var dictionary = GetValues(poco);

            var stringBuilder = new StringBuilder();

            foreach (var keyValue in dictionary)
            {
                stringBuilder.AppendLine(keyValue.Key + ":" + keyValue.Value);
            }

            return stringBuilder.ToString();
        }
    }
}