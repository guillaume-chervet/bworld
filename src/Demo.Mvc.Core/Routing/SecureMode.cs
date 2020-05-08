namespace Demo.Mvc.Core.Routing
{
    /// <summary>
    ///     Type de sécurisétion de la route ou du domaine
    /// </summary>
    public enum SecureMode
    {
        /// <summary>
        ///     Pas de sécurisation obligatoire, si la page en cours est sécurisé, alors le liens est sécurisé
        /// </summary>
        NoSecure = 0,

        /// <summary>
        ///     Sécurisation obligatoire
        /// </summary>
        Secure = 1,

        /// <summary>
        ///     Sécurisation si en page sécurisé sinon non
        /// </summary>
        Media = 2
    }
}