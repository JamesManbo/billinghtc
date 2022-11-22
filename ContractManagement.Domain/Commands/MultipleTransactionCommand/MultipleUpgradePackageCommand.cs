using ContractManagement.Domain.AggregatesModel.Commons;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.MultipleTransactionCommand
{
    public class MultipleUpgradePackageCommand : BaseMultipleTransactionCommand, IRequest<ActionResponse>
    {
        public MultipleUpgradePackageCommand()
        {
        }

        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public decimal PackagePrice { get; set; }
    }
}
