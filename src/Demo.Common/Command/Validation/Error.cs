using System.Collections.Generic;

namespace Demo.Common.Command.Validation
{
    /// <summary>
    ///     Représente une erreur
    /// </summary>
    public class Error
    {
        /// <summary>
        ///     Optionnel: Message associé à l'erreur
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Fourni des informations plus détailler à propos de l'erreur
        /// </summary>
        public IDictionary<string, string> ErrorDetails { get; set; }
    }
}