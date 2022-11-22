using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using TikForNet.Objects.Ppp;

namespace ContractManagement.Domain.Commands.RadiusAndBrasCommand
{
    public class CreateNewRadiusUserCommand : IRequest<ActionResponse>
    {
        public int ContractId { get; set; }
        public string ContractCode { get; set; }
        public ContractorDTO Contractor { get; set; }
        public List<OutContractServicePackageDTO> OutContractServicePackages { get; set; }
        public List<TransactionServicePackageDTO> TransactionServicePackages { get; set; }
    }
}
