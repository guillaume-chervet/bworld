using System.Collections.Generic;

namespace Demo.Common.Command.Validation
{
    public class ValidationResult
    {
        public ValidationResult()
        {
            Errors = new Dictionary<string, Error>();
        }

        /// <summary>
        ///     Liste des erreur associé générales
        /// </summary>
        public IDictionary<string, Error> Errors { get; }

        /// <summary>
        ///     Indique s'il y a une erreur
        /// </summary>
        public bool IsSuccess
        {
            get { return Errors.Count <= 0; }
        }

        /// <summary>
        ///     Ajoute une erreur de validation
        /// </summary>
        /// <param name="code">Code de l'erreur</param>
        /// <param name="message">Message de l'erreur (optionnel)</param>
        /// <param name="errorDetails">Detail potentiellement dynamique (règle métier) de l'erreur (optionnel)</param>
        public void AddError(string code, string message = null, IDictionary<string, string> errorDetails = null)
        {
            Errors.Add(code, new Error {Message = message, ErrorDetails = errorDetails});
        }

        /// <summary>
        ///     Ajoute des erreurs de validation
        /// </summary>
        /// <param name="newErrors">Dictionnaire d'erreur</param>
        public void AddErrors(IDictionary<string, Error> newErrors)
        {
            if (newErrors != null)
            {
                foreach (var newError in newErrors)
                {
                    Errors.Add(newError);
                }
            }
        }

        public bool ContainsKey(string key)
        {
            return Errors.ContainsKey(key);
        }
    }
}