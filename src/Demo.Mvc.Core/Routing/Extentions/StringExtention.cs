using System;

namespace Demo.Mvc.Core.Routing.Extentions
{
    public static class StringExtention
    {
        public static string ReplaceFirstIgnoreCase(this string source, string search, string replace)
        {
            var pos = source.IndexOf(search, StringComparison.InvariantCultureIgnoreCase);

            if (pos < 0) return source;

            return source.Substring(0, pos) + replace + source.Substring(pos + search.Length);
        }

        /// <summary>
        ///     Permet de décteter une chaine contenu dans la chaîne en étant abstrait des majuscules minuscule
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ContainsIgnoreCase(this string source, string value)
        {
            var results = source.IndexOf(value, StringComparison.CurrentCultureIgnoreCase);

            return results != -1;
        }

        public static string ReplaceIgnoreCase(this string originalString, string oldValue, string newValue)
        {
            return Replace(originalString, oldValue, newValue, StringComparison.CurrentCultureIgnoreCase);
        }

        public static string Replace(this string originalString, string oldValue, string newValue,
            StringComparison comparisonType)
        {
            var startIndex = 0;
            while (true)
            {
                startIndex = originalString.IndexOf(oldValue, startIndex, comparisonType);
                if (startIndex == -1)
                    break;

                originalString = originalString.Substring(0, startIndex) + newValue +
                                 originalString.Substring(startIndex + oldValue.Length);

                startIndex += newValue.Length;
            }

            return originalString;
        }
    }
}