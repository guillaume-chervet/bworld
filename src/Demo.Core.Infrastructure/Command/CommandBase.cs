using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Demo.Common.Command
{
    /// <summary>
    ///     Représente une commande avec un résultat et une entrée Typée
    /// </summary>
    /// <typeparam name="TDInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TDataFactory">Class d'accès aux données</typeparam>
    public abstract class CommandBase<TDInput, TResult> : ICommand
        where TResult : CommandResult, new()
    {
        public CommandBase()
        {
            Result = new TResult();
        }

        public TDInput Input { protected get; set; }
        public TResult Result { get; }

        #region ICommand Members

        /// <summary>
        ///     Appel métier
        /// </summary>
        /// <exception cref="Exception"></exception>
        public virtual async Task ExecuteAsync()
        {
            try
            {
                BeforeAction();
                Validate();
                await ActionAsync();
                if (Result.IsSuccess)
                {
                    ActionSuccess();
                }
            }
            catch (Exception ex)
            {
                Result.ValidationResult.AddError("Command.Exception", "Une exception non gérée à été lancé.");
                OnException(ex);
                throw;
            }
            finally
            {
                FinallyAction();
            }
        }

        protected abstract void BeforeAction();
        protected abstract void Validate();

        protected virtual async Task ActionAsync()
        {
        }

        protected abstract void ActionSuccess();
        protected abstract void OnException(Exception exception);

        /// <summary>
        ///     Méthode appelé quoi quil arrive à la fin de l'action
        /// </summary>
        protected virtual void FinallyAction()
        {
            var fields =
                GetType().GetFields(
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var fieldInfo in fields)
            {
                var value = fieldInfo.GetValue(this);

                var disposable = value as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        #endregion
    }
}