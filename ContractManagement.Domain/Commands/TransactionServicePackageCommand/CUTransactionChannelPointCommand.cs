using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Models;
using MediatR;
using System.Collections.Generic;

namespace ContractManagement.Domain.Commands.OutContractCommand
{
    public class CUTransactionChannelPointCommand
        : CUDeploymentChannelPointCommand<CUTransactionEquipmentCommand>, IBind
    {
        public int? ContractPointId { get; set; }
        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            if (command is CUOutputChannelPointCommand channelPointCommand)
            {
                this.LocationId = channelPointCommand.LocationId;
                this.CurrencyUnitId = channelPointCommand.CurrencyUnitId;
                this.CurrencyUnitCode = channelPointCommand.CurrencyUnitCode;
                this.PointType = channelPointCommand.PointType;
                this.InstallationAddress = channelPointCommand.InstallationAddress;
                this.MonthlyCost = channelPointCommand.MonthlyCost;
                this.InstallationFee = channelPointCommand.InstallationFee;
                this.OtherFee = channelPointCommand.OtherFee;
                this.EquipmentAmount = channelPointCommand.EquipmentAmount;
                this.CreatedBy = channelPointCommand.CreatedBy;
                this.UpdatedBy = channelPointCommand.UpdatedBy;
                this.ContractPointId = channelPointCommand.Id;
                this.ConnectionPoint = channelPointCommand.ConnectionPoint;

                this.Equipments = new List<CUTransactionEquipmentCommand>();
                foreach (var equipmentItem in channelPointCommand.Equipments)
                {
                    var transEquipmentCommand = new CUTransactionEquipmentCommand();
                    transEquipmentCommand.Binding(equipmentItem);
                    transEquipmentCommand.ContractEquipmentId = equipmentItem.Id;
                    this.Equipments.Add(transEquipmentCommand);
                }
            }
            else if (command is OutputChannelPointDTO channelPointDTO)
            {
                this.LocationId = channelPointDTO.LocationId;
                this.CurrencyUnitId = channelPointDTO.CurrencyUnitId;
                this.CurrencyUnitCode = channelPointDTO.CurrencyUnitCode;
                this.PointType = channelPointDTO.PointType;
                this.InstallationAddress = channelPointDTO.InstallationAddress;
                this.MonthlyCost = channelPointDTO.MonthlyCost;
                this.InstallationFee = channelPointDTO.InstallationFee;
                this.OtherFee = channelPointDTO.OtherFee;
                this.EquipmentAmount = channelPointDTO.EquipmentAmount;
                this.ContractPointId = channelPointDTO.Id;
                this.ConnectionPoint = channelPointDTO.ConnectionPoint;

                this.Equipments = new List<CUTransactionEquipmentCommand>();
                foreach (var equipmentItem in channelPointDTO.Equipments)
                {
                    var transEquipmentCommand = new CUTransactionEquipmentCommand();
                    transEquipmentCommand.Binding(equipmentItem);
                    transEquipmentCommand.ContractEquipmentId = equipmentItem.Id;
                    this.Equipments.Add(transEquipmentCommand);
                }
            }
        }
    }
}
