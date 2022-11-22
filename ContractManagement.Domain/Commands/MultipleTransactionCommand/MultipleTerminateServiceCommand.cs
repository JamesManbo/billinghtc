using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.MultipleTransactionCommand
{
    public class MultipleTerminateServiceCommand
        : BaseMultipleTransactionCommand, IRequest<ActionResponse>
    {
        public MultipleTerminateServiceCommand()
        {
        }
    }
}
