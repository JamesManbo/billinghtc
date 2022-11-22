using ContractManagement.Domain.AggregatesModel.Abstractions;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.TransactionAggregate
{
    [Table("TransactionChannelTaxes")]
    public class TransactionChannelTax : TaxValueAbstraction, IBind
    {
        public int TransactionId { get; set; }
        public int? ContractChannelTaxId { get; set; }
        public int? TransactionServicePackageId { get; private set; }

        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            if (command is CUTransactionTaxCommand taxValueCommand)
            {
                this.TaxCategoryId = taxValueCommand.TaxCategoryId;
                this.TransactionServicePackageId = taxValueCommand.TransactionServicePackageId;
                this.TaxCategoryName = taxValueCommand.TaxCategoryName;
                this.TaxCategoryCode = taxValueCommand.TaxCategoryCode;
                this.TaxValue = taxValueCommand.TaxValue;
                this.TransactionId = taxValueCommand.TransactionId;
                this.ContractChannelTaxId = taxValueCommand.ContractChannelTaxId;
            }
        }
    }
}
