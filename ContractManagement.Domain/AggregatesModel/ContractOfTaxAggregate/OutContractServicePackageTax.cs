using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.TaxAggreagate;
using System.ComponentModel.DataAnnotations;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Abstractions;
using ContractManagement.Domain.Bindings;
using MediatR;
using ContractManagement.Domain.Commands.ServicePackageCommand;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;

namespace ContractManagement.Domain.AggregatesModel.ContractOfTaxAggregate
{
    [Table("OutContractServicePackageTaxes")]
    public class OutContractServicePackageTax : TaxValueAbstraction, IBind
    {
        public int OutContractServicePackageId { get; set; }

        public OutContractServicePackageTax()
        {
        }

        public OutContractServicePackageTax(int outContractServicePackageId, int taxCategoryId, string taxCategoryName, string taxCategoryCode, float taxValue)
        {
            this.OutContractServicePackageId = outContractServicePackageId;
            this.TaxCategoryId = taxCategoryId;
            this.TaxValue = taxValue;
            this.TaxCategoryName = taxCategoryName;
            this.TaxCategoryCode = taxCategoryCode;
        }

        public virtual OutContractServicePackage OutContractServicePackage { get; set; }

        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            if (command is CUOutContractServicePackageTaxCommand taxValueCommand)
            {
                if (!isUpdate)
                {
                    this.TaxCategoryName = taxValueCommand.TaxCategoryName;
                    this.TaxCategoryCode = taxValueCommand.TaxCategoryCode;
                    this.TaxCategoryId = taxValueCommand.TaxCategoryId;
                    this.OutContractServicePackageId = taxValueCommand.OutContractServicePackageId;
                }
                this.TaxValue = taxValueCommand.TaxValue;
            }
        }
    }

}
