using Demo.Common.Command.Validation;

namespace Demo.Common.Command
{
    public class CommandResult
    {
        public CommandResult()
        {
            ValidationResult = new ValidationResult();
        }

        /// <summary>
        ///     Indique que l'opération est en sucess ou en échec
        /// </summary>
        public bool IsSuccess
        {
            get { return ValidationResult.IsSuccess; }
        }

        public ValidationResult ValidationResult { get; }
    }

    public sealed class CommandResult<T> : CommandResult
    {
        public T Data { get; set; }
    }
}