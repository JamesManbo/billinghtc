using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Seed;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.ServicePackages
{
    [Table("ChannelPriceBusTables")]
    public class ChannelPriceBusTable : DeploymentChannelPackagePriceBusTable, IBind
    {
        public int? ChannelId { get; set; }
        public ChannelPriceBusTable() : base()
        {
        }

        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            if (command is CUChannelPriceBusTableCommand busTableCommand)
            {
                this.Id = busTableCommand.Id;
                this.CurrencyUnitCode = busTableCommand.CurrencyUnitCode;
                this.UsageValueFrom = busTableCommand.UsageValueFrom;
                this.UsageValueFromUomId = busTableCommand.UsageValueFromUomId;
                this.UsageValueTo = busTableCommand.UsageValueTo;
                this.UsageValueToUomId = busTableCommand.UsageValueToUomId;
                this.PriceValue = busTableCommand.PriceValue;
                this.PriceUnitUomId = busTableCommand.PriceUnitUomId;
                this.IsDomestic = busTableCommand.IsDomestic;
                this.UsageBaseUomValueFrom = busTableCommand.UsageBaseUomValueFrom;
                this.UsageBaseUomValueTo = busTableCommand.UsageBaseUomValueTo;
                this.ChannelId = busTableCommand.ChannelId;
                this.CalculateBaseValue();
            }
        }
    }
}
