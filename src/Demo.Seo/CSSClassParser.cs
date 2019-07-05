using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Demo.Seo
{
    public class CSSClassParser
    {
        #region Constants

        private const string _CSS_CLASS_REGEX = "class=\"[a-zA-Z./:&\\d_]+\"";

        #endregion

        #region Public Properties

        public List<string> Classes { get; set; } = new List<string>();

        #endregion

        /// <summary>
        ///     Parses the page looking for css classes that are in use.
        /// </summary>
        /// <param name="page">The page to parse.</param>
        public void ParseForCssClasses(Page page)
        {
            var matches = Regex.Matches(page.Text, _CSS_CLASS_REGEX);

            for (var i = 0; i <= matches.Count - 1; i++)
            {
                var classMatch = matches[i];
                var classesArray =
                    classMatch.Value.Substring(classMatch.Value.IndexOf('"') + 1,
                        classMatch.Value.LastIndexOf('"') - classMatch.Value.IndexOf('"') - 1)
                        .Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                foreach (var classValue in classesArray)
                {
                    if (!Classes.Contains(classValue))
                    {
                        Classes.Add(classValue);
                    }
                }
            }
        }

        #region Constructor

        #endregion

        #region Private Instance Fields

        #endregion
    }
}