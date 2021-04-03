using System.Threading.Tasks;
using Demo.Common.Command;

namespace Demo.Mvc.Core.Sites.Core
{
    public class  BusinessFactory
    {

        public Task<TOutput> InvokeAsync<TCommand, TInput, TOutput>(TCommand command, TInput input)
            where TOutput : CommandResult, new()
            where TCommand : CommandBase<TInput, TOutput>
        {
            return new CommandProcessor().InvokeAsync<TCommand, TInput, TOutput>(command, input);
        }
    }
}