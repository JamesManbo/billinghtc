using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.Commands.Abstraction;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionServicePackageCommand
{
    public class CUTransactionPromotionForContractCommand
        : AppliedPromotionCommandAbstraction, IBaseRequest
    {
        public int TransactionServicePackageId { get; set; }
        public int? OutContractServicePackageId { get; set; }
    }
}
