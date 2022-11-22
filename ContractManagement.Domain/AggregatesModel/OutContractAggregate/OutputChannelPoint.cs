using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.OutContractCommand;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ContractManagement.Domain.AggregatesModel.OutContractAggregate
{
    [Table("OutputChannelPoints")]
    public class OutputChannelPoint : DeploymentChannelPoint<ContractEquipment>, IBind
    {
        public OutputChannelPoint()
        {
        }

        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            if (command is CUOutputChannelPointCommand channelCommand)
            {
                if (string.IsNullOrWhiteSpace(channelCommand.LocationId))
                {
                    this.LocationId = Guid.NewGuid().ToString();
                }
                else
                {
                    this.LocationId = channelCommand.LocationId;
                }

                this.DerivedBinding(channelCommand);
            }

            if (command is CUTransactionChannelPointCommand transactionPointCmd)
            {
                this.LocationId = transactionPointCmd.LocationId;
                this.DerivedBinding(transactionPointCmd);
            }
        }

        private void DerivedBinding(CUDeploymentChannelPointCommand baseCommand)
        {
            this.CurrencyUnitId = baseCommand.CurrencyUnitId;
            this.CurrencyUnitCode = baseCommand.CurrencyUnitCode;
            this.PointType = baseCommand.PointType;
            this.InstallationAddress = baseCommand.InstallationAddress.GetCopy();
            this.InstallationFee = baseCommand.InstallationFee;
            this.OtherFee = baseCommand.OtherFee;
            this.MonthlyCost = baseCommand.MonthlyCost;
            this.EquipmentAmount = baseCommand.EquipmentAmount;
            this.ApplyFeeToChannel = baseCommand.ApplyFeeToChannel;
            this.ConnectionPoint = baseCommand.ConnectionPoint;
        }

        public override void CalculateTotal()
        {
            this.EquipmentAmount = this.Equipments.Sum(e => e.GrandTotal);
        }
    }
}
