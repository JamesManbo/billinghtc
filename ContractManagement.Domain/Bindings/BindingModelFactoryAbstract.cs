using ContractManagement.Domain.AggregatesModel.BaseContract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Bindings
{
    public abstract class BindingModelFactoryAbstract<TResult, TCommand>
        where TResult : IBind
        where TCommand : IBaseRequest
    {
        public abstract TResult CreateInstance(TCommand channelCommand);
    }
    public abstract class BindingModelFactoryAbstract<TResult>
        where TResult : IBind
    {
        public abstract TResult CreateInstance<TCommand>(TCommand channelCommand) where TCommand: IBaseRequest;
    }

}
