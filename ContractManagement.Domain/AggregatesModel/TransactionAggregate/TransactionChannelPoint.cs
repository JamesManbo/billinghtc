using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ContractManagement.Domain.AggregatesModel.TransactionAggregate
{
    [Table("TransactionChannelPoints")]
    public class TransactionChannelPoint : DeploymentChannelPoint<TransactionEquipment>, IBind
    {
        public int? ContractPointId { get; set; }
        public TransactionChannelPoint() { 
        }

        public TransactionChannelPoint(CUTransactionChannelPointCommand command)
        {
            CreatedDate = DateTime.Now;
            CreatedBy = command.CreatedBy;
            Binding(command);
        }

        public void Update(CUTransactionChannelPointCommand updateCmd)
        {
            UpdatedDate = DateTime.Now;
            UpdatedBy = updateCmd.UpdatedBy;
            this.Binding(updateCmd);
        }

        public override void AddOrUpdateEquipment<TEquipmentCommand>(TEquipmentCommand equipmentCmd)
        {
            if (equipmentCmd.Id == 0)
            {
                //add validated new contract equipment
                var equipmentFactory = new BindingModelFactory<TransactionEquipment, TEquipmentCommand>();
                var equipmentItem = equipmentFactory.CreateInstance(equipmentCmd);
                equipmentItem.CreatedDate = DateTime.Now;
                equipmentItem.CreatedBy = CreatedBy;
                equipmentItem.CalculateTotal();
                _equipments.Add(equipmentItem);
            }
            else
            {
                var existEquipment = Equipments.First(e => e.Id == equipmentCmd.Id);
                existEquipment.CreatedDate = DateTime.Now;
                existEquipment.UpdatedBy = UpdatedBy;
                existEquipment.Binding(equipmentCmd);
                existEquipment.CalculateTotal();
            }
            this.CalculateTotal();
        }

        public override void CalculateTotal()
        {
            this.EquipmentAmount = this.Equipments.Sum(e => e.GrandTotal);
        }

        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            if (command is CUTransactionChannelPointCommand channelPointCommand)
            {
                this.LocationId = channelPointCommand.LocationId;
                this.ContractPointId = channelPointCommand.ContractPointId;
                this.CurrencyUnitId = channelPointCommand.CurrencyUnitId;
                this.CurrencyUnitCode = channelPointCommand.CurrencyUnitCode;
                this.PointType = channelPointCommand.PointType;
                this.InstallationAddress = channelPointCommand.InstallationAddress.GetCopy();
                this.InstallationFee = channelPointCommand.InstallationFee;
                this.OtherFee = channelPointCommand.OtherFee;
                this.MonthlyCost = channelPointCommand.MonthlyCost;
                this.EquipmentAmount = channelPointCommand.EquipmentAmount;
                this.ApplyFeeToChannel = channelPointCommand.ApplyFeeToChannel;
            }
        }
    }
}
