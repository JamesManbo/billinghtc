using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.IntegrationEventCommand
{
    public class TerminateServicePackagesIntegrationEventCommand : IRequest<ActionResponse>
    {
        public List<int> OutContractServicePackageIds { get; set; }
        public TerminateServicePackagesIntegrationEventCommand()
        {
            OutContractServicePackageIds = new List<int>();
        }
    }
}
