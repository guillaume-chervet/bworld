using System;
using System.Threading.Tasks;
using Demo.Business.BusinessModule;
using Demo.Business.Routing;
using Demo.Common.Command;
using Demo.Routing;
using Demo.Routing.Interfaces;
using Demo.Data;

namespace Demo.Business
{
    public class  BusinessFactory
    {
        public TOutput Invoke<TCommand, TInput, TOutput>(TCommand command, TInput input)
            where TOutput : CommandResult, new()
            where TCommand : CommandBase<TInput, TOutput>
        {
            return new CommandProcessor().Invoke<TCommand, TInput, TOutput>(command, input);
        }

        public Task<TOutput> InvokeAsync<TCommand, TInput, TOutput>(TCommand command, TInput input)
            where TOutput : CommandResult, new()
            where TCommand : CommandBase<TInput, TOutput>
        {
            return new CommandProcessor().InvokeAsync<TCommand, TInput, TOutput>(command, input);
        }
    }
}