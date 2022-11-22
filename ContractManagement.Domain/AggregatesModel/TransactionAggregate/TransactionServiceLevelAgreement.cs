using ContractManagement.Domain.AggregatesModel.Abstractions;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.TransactionAggregate
{
    [Table("TransactionSLAs")]
    public class TransactionServiceLevelAgreement
        : ServiceLevelAgreementAbstraction, IBind
    {
        public int TransactionServicePackageId { get; set; }
        public int? ContractSlaId { get; set; }
        public TransactionServiceLevelAgreement()
        {
        }

        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            if (command is CUTransactionSLACommand transSLACommand)
            {
                if(transSLACommand.TransactionServicePackageId.HasValue)
                    this.TransactionServicePackageId = transSLACommand.TransactionServicePackageId.Value;

                if (transSLACommand.ServiceId.HasValue)
                    this.ServiceId = transSLACommand.ServiceId.Value;

                this.IsDefault = transSLACommand.IsDefault;
                this.Uid = transSLACommand.Uid;
                this.Label = transSLACommand.Label;
                this.Content = transSLACommand.Content;
                this.Id = transSLACommand.Id;
                this.ContractSlaId = transSLACommand.ContractSlaId;
            }
        }
    }
}
