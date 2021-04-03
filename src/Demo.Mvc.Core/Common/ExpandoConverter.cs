using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace Demo.Mvc.Core.Common
{
    public static class ExpandoConverter
    {
        /// <summary>
        ///     Converti un object Exando en objet utilisable par StringTemplate
        /// </summary>
        /// <param name="currentValue"></param>
        /// <returns></returns>
        public static object Convert(object currentValue)
        {
            if (currentValue == null) return null;

            var type = currentValue.GetType();

            if (currentValue is ExpandoObject)
            {
                IDictionary<string, object> dico = new Dictionary<string, object>();
                foreach (var keyValuePair in (IEnumerable<KeyValuePair<string, object>>) currentValue)
                {
                    dico.Add(keyValuePair.Key, Convert(keyValuePair.Value));
                }
                return dico;
            }
            if (type.FullName.Contains("System.Collections.Generic.List"))
            {
                IList<object> list = new List<object>();
                foreach (var element in (IEnumerable) currentValue)
                {
                    list.Add(Convert(element));
                }
                return list;
            }

            return currentValue;
        }
    }
}