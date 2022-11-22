using ContractManagement.Domain.AggregatesModel.BaseContract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Bindings
{
    public class BindingModelFactory<TResult, TCommand> : BindingModelFactoryAbstract<TResult, TCommand>
        where TResult : IBind, new()
        where TCommand : IBaseRequest
    {
        public override TResult CreateInstance(TCommand bindingCommand)
        {
            var result = new TResult();
            result.Binding(bindingCommand);
            return result;
        }
    }
    public class BindingModelFactory<TResult> : BindingModelFactoryAbstract<TResult>
        where TResult : IBind, new()
    {
        public override TResult CreateInstance<TCommand>(TCommand bindingCommand)
        {
            var result = new TResult();
            result.Binding(bindingCommand);
            return result;
        }
    }
}
