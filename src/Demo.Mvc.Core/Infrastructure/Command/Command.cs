using System;

namespace Demo.Common.Command
{
    /// <summary>
    ///     Représente une commande avec un résultat et une entrée Typée
    /// </summary>
    /// <typeparam name="TDInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TDataFactory">Class d'accès aux données</typeparam>
    public abstract class Command<TDInput, TResult> : CommandBase<TDInput, TResult>
        where TResult : CommandResult, new()
    {
        protected override void BeforeAction()
        {
        }

        protected override void ActionSuccess()
        {
        }

        protected override void OnException(Exception exception)
        {
        }

        protected override void Validate()
        {
        }
    }
}