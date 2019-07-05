using System;
using System.Dynamic;
using Antlr4.StringTemplate;
using Newtonsoft.Json;

namespace Demo.Renderer
{
    public class StringTemplateRenderer
    {
        #region Constants

        /// <summary>
        ///     Caractère utilisé par défaut pour délimiter le début d'une variable dans le template
        /// </summary>
        public const string DefaultStartCharDelimiter = "§";

        /// <summary>
        ///     Caractère utilisé par défaut pour délimiter la fin d'une variable dans le template
        /// </summary>
        public const string DefaultStopCharDelimiter = "§";

        #endregion

        #region Ctors

        /// <summary>
        ///     Constructeur
        /// </summary>
        public StringTemplateRenderer()
            : this(DefaultStartCharDelimiter, DefaultStopCharDelimiter)
        {
        }

        /// <summary>
        ///     Constructeur
        /// </summary>
        /// <param name="delimiterStartChar">Le caractère à utiliser pour délimiter le début d'une variable dans le template</param>
        /// <param name="delimiterStopChar">Le caractère à utiliser pour délimiter la fin d'une variable dans le template</param>
        public StringTemplateRenderer(string delimiterStartChar, string delimiterStopChar)
        {
            DelimiterStartChar = delimiterStartChar;
            DelimiterStopChar = delimiterStopChar;
            EnableJsonParsingForStringModels = true;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Obtient ou définit le caractère utilisé pour délimiter le début d'une variable dans le template
        /// </summary>
        public string DelimiterStartChar { get; set; }

        /// <summary>
        ///     Obtient ou définit le caractère utilisé pour délimiter la fin d'une variable dans le template
        /// </summary>
        public string DelimiterStopChar { get; set; }

        /// <summary>
        ///     Indique si les models doivent êtres considérés comme du JSON et convertis en objets lorsqu'ils sont de type
        ///     <c>string</c>
        /// </summary>
        public bool EnableJsonParsingForStringModels { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        ///     Effectue le rendu d'un template
        /// </summary>
        /// <param name="template">Le contenu du template</param>
        /// <param name="model">Un objet contenant les données à injecter lors du rendu</param>
        public string Render(string template, object model)
        {
            template = FixTemplate(template);

            var stringTemplate = new Template(template, Convert.ToChar(DelimiterStartChar),
                Convert.ToChar(DelimiterStopChar));

            if (model is string && EnableJsonParsingForStringModels)
            {
                var sModel = (string) model;

                model = sModel.StartsWith("[")
                    ? ExpandoObjectConverter.Convert(JsonConvert.DeserializeObject<ExpandoObject[]>(sModel))
                    : ExpandoObjectConverter.Convert(JsonConvert.DeserializeObject<ExpandoObject>(sModel));
            }

            stringTemplate.Add("model", model);

            // Effectue le rendu du template et retourne le résultat
            var result = stringTemplate.Render();

            result = UnfixTemplate(result);

            return result;
        }

        /// <summary>
        ///     Re-modèle le templtae pour contourner un bug String template
        /// </summary>
        /// <param name="template"></param>
        /// <returns></returns>
        private string FixTemplate(string template)
        {
            // TODO maj String template: présence d'un bug si parenthèse présente dans un foreach
            // TODO Optimizer workaround avec un unique regex
            template = template.Replace(":{", "!!parenthese_ouvrante_stringtemplate!!");
            template = template.Replace("}" + DelimiterStopChar, "!!parenthese_fermante_stringtemplate!!");

            template = template.Replace("{", "!!parenthese_ouvrante!!");
            template = template.Replace("}", "!!parenthese_fermante!!");

            template = template.Replace("!!parenthese_ouvrante_stringtemplate!!", ":{");
            template = template.Replace("!!parenthese_fermante_stringtemplate!!", "}" + DelimiterStopChar);

            return template;
        }

        /// <summary>
        ///     Retitue le modèle d'orrigine
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private string UnfixTemplate(string result)
        {
            result = result.Replace("!!parenthese_ouvrante!!", "{");
            result = result.Replace("!!parenthese_fermante!!", "}");
            return result;
        }

        #endregion
    }
}