using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.OutContractCommand;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.TransactionAggregate
{
    public class TransactionPriceBusTable : DeploymentChannelPackagePriceBusTable, IBind
    {
        public int? TransactionServicePackageId { get; set; }
        public int? ContractPbtId { get; set; }
        public TransactionPriceBusTable() : base()
        {
        }

        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            if (command is CUTransactionPriceBusTableCommand transBusTableCommand)
            {
                this.Id = transBusTableCommand.Id;
                this.CurrencyUnitCode = transBusTableCommand.CurrencyUnitCode;
                this.UsageValueFrom = transBusTableCommand.UsageValueFrom;
                this.UsageValueFromUomId = transBusTableCommand.UsageValueFromUomId;
                this.UsageValueTo = transBusTableCommand.UsageValueTo;
                this.UsageValueToUomId = transBusTableCommand.UsageValueToUomId;
                this.PriceValue = transBusTableCommand.PriceValue;
                this.PriceUnitUomId = transBusTableCommand.PriceUnitUomId;
                this.IsDomestic = transBusTableCommand.IsDomestic;
                this.UsageBaseUomValueFrom = transBusTableCommand.UsageBaseUomValueFrom;
                this.UsageBaseUomValueTo = transBusTableCommand.UsageBaseUomValueTo;
                this.ContractPbtId = transBusTableCommand.ContractPbtId;
                this.TransactionServicePackageId = transBusTableCommand.TransactionServicePackageId;
            }
            this.CalculateBaseValue();
        }

    }
}
