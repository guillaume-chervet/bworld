using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Demo.Routing.Extentions
{
    public static class UrlHelper
    {
        public static string Concat(string path1, string path2)
        {
            if (string.IsNullOrEmpty(path1) && string.IsNullOrEmpty(path2))
                return null;

            if (string.IsNullOrEmpty(path1))
                path1 = "";

            if (string.IsNullOrEmpty(path2))
                path2 = "";

            path1 = path1.TrimEnd('/');
            path2 = path2.TrimStart('/');

            return string.Format("{0}/{1}", path1, path2);
        }

        public static string Concat(params string[] parameters)
        {
            if (parameters == null) return string.Empty;
            var lenght = parameters.Length;

            if (lenght == 1) return parameters[0];


            var result = parameters[0];
            for (var i = 1; i < lenght; i++)
            {
                result = Concat(result, parameters[i]);
            }

            return result;
        }

        public static string RemoveSeparator(string path1)
        {
            if (string.IsNullOrEmpty(path1))
            {
                return path1;
            }

            return path1.TrimStart('/').TrimEnd('/');
        }

        public static string RemoveFirstSeparator(string path1)
        {
            if (string.IsNullOrEmpty(path1))
                return path1;

            return path1.TrimStart('/');
        }

        public static string RemoveLastSeparator(string path1)
        {
            if (string.IsNullOrEmpty(path1))
                return path1;

            return path1.TrimEnd('/');
        }

        /// <summary>
        ///     Retourne les clé valeur associé au membre de l'objet
        /// </summary>
        /// <param name="poco"></param>
        /// <returns></returns>
        public static IDictionary<string, string> GetValues(object poco)
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

        public static string NormalizeTextForUrl(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.ToLower();
            text = text.Replace(" ", "-");
            text = text.Replace("/", "-");
            text = text.Replace("\\", "-");
            text = text.Trim();
            text = text.Replace("é", "e");
            text = text.Replace("è", "e");
            text = text.Replace("ê", "e");
            text = text.Replace("ç", "c");

            text = text.Replace("à", "a");

            text = text.Replace("'", "-");
            text = text.Replace("\"", "");
            text = text.Replace("<", "");
            text = text.Replace(">", "");
            text = text.Replace(")", "");
            text = text.Replace("(", "");
            text = text.Replace(":", "-");
            text = text.Replace("«", "-");
            text = text.Replace("»", "-");
            text = text.Replace("&", "-");
            text = text.Replace(",", "-");

            text = text.Replace("-----", "-");
            text = text.Replace("----", "-");
            text = text.Replace("---", "-");
            text = text.Replace("--", "-");

            text = RemoveDiacritics(text);

            return text;
        }

        public static string NormalizeTextForHtml(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.Trim();

            text = text.Replace("'", "");
            text = text.Replace("\"", "");
            text = text.Replace("<", "");
            text = text.Replace(">", "");
            text = text.Replace(")", "");
            text = text.Replace("(", "");

            text = RemoveDiacritics(text);

            return text;
        }

        /// <summary>
        ///     Supprime les accents dans la chaine
        /// </summary>
        /// <param name="value">La chaine à traiter</param>
        /// <returns>La chaine sans accents</returns>
        public static string RemoveDiacritics(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var normalizedString = value.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }

            return stringBuilder.ToString();
        }

        public static string Protocol(bool isSecure, string protocolHttps)
        {
            return isSecure ? protocolHttps : "http";
        }
    }
}