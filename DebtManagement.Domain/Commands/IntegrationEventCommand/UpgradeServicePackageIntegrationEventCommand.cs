using DebtManagement.Domain.Models;
using DebtManagement.Domain.Models.ContractModels;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.IntegrationEventCommand
{
    public class UpgradeServicePackageIntegrationEventCommand : IRequest<ActionResponse>
    {
        public TransactionDTO Transaction { get; set; }
        public OutContractDTO OutContract { get; set; }
        public List<OutContractServicePackageDTO> NewOutContractServicePackages { get; set; }
    }
}
