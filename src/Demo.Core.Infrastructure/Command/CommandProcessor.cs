using System.Threading.Tasks;

namespace Demo.Common.Command
{
    public class CommandProcessor
    {

        public async Task<TOutput> InvokeAsync<TCommand, TInput, TOutput>(TCommand command, TInput input)
            where TOutput : CommandResult, new()
            where TCommand : CommandBase<TInput, TOutput>
        {
            command.Input = input;
            await command.ExecuteAsync();
            return command.Result;
        }
    }
}