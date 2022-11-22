using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Bindings;
using ContractManagement.Domain.Commands.BaseContractCommand;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.TransactionEquipmentCommand
{
    public class CUTransactionEquipmentCommand
        : CUDeploymentEquipmentCommand
        , IRequest<ActionResponse<TransactionEquipmentDTO>>, IBind
    {
        public CUTransactionEquipmentCommand() { }

        public int? TransactionId { get; set; }
        public int? TransactionServicePackageId { get; set; }
        public int? ContractEquipmentId { get; set; }
        public int? OutContractId { get; set; }
        public int OldEquipmentId { get; set; }
        public bool? IsOld { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public bool? IsAcceptanced { get; set; }
        public float WillBeReclaimUnit { get; set; }
        public float WillBeHoldUnit { get; set; }

        public void Binding(IBaseRequest command, bool isUpdate = false)
        {
            if (command is CUContractEquipmentCommand contractEquipCommand)
            {
                this.IsAcceptanced = false;
                this.IsOld = true;

                this.Id = contractEquipCommand.Id;
                this.OutContractPackageId = contractEquipCommand.OutContractPackageId;
                this.OutContractPackageUid = contractEquipCommand.OutContractPackageUid;
                this.EquipmentId = contractEquipCommand.EquipmentId;
                this.EquipmentName = contractEquipCommand.EquipmentName;
                this.ExaminedUnit = contractEquipCommand.ExaminedUnit;
                this.RealUnit = contractEquipCommand.RealUnit;
                this.ReclaimedUnit = contractEquipCommand.ReclaimedUnit;
                this.CurrencyUnitId = contractEquipCommand.CurrencyUnitId;
                this.CurrencyUnitCode = contractEquipCommand.CurrencyUnitCode;
                this.SupporterHoldedUnit = contractEquipCommand.SupporterHoldedUnit;
                this.UnitPrice = contractEquipCommand.UnitPrice;
                this.SerialCode = contractEquipCommand.SerialCode;
                this.MacAddressCode = contractEquipCommand.MacAddressCode;
                this.DeviceCode = contractEquipCommand.DeviceCode;
                this.Manufacturer = contractEquipCommand.Manufacturer;
                this.Specifications = contractEquipCommand.Specifications;
                this.StatusId = contractEquipCommand.StatusId;
                this.IsFree = contractEquipCommand.IsFree;
                this.HasToReClaim = contractEquipCommand.HasToReClaim;
                this.EquipmentUom = contractEquipCommand.EquipmentUom;
                this.CreatedBy = contractEquipCommand.CreatedBy;
                this.UpdatedBy = contractEquipCommand.UpdatedBy;
                this.CreatedDate = contractEquipCommand.CreatedDate;
                this.UpdatedDate = contractEquipCommand.UpdatedDate;
                this.UpdatedDate = contractEquipCommand.UpdatedDate;
            }
            else if (command is ContractEquipmentDTO equipmentDTO)
            {
                this.IsAcceptanced = false;
                this.IsOld = true;

                this.OutContractPackageId = equipmentDTO.OutContractPackageId;
                this.EquipmentId = equipmentDTO.EquipmentId;
                this.EquipmentName = equipmentDTO.EquipmentName;
                this.ExaminedUnit = equipmentDTO.ExaminedUnit;
                this.CurrencyUnitId = equipmentDTO.CurrencyUnitId;
                this.CurrencyUnitCode = equipmentDTO.CurrencyUnitCode;
                this.RealUnit = equipmentDTO.RealUnit;
                this.ReclaimedUnit = equipmentDTO.ReclaimedUnit;
                this.SupporterHoldedUnit = equipmentDTO.SupporterHoldedUnit;
                this.UnitPrice = equipmentDTO.UnitPrice;
                this.SerialCode = equipmentDTO.SerialCode;
                this.MacAddressCode = equipmentDTO.MacAddressCode;
                this.DeviceCode = equipmentDTO.DeviceCode;
                this.Manufacturer = equipmentDTO.Manufacturer;
                this.Specifications = equipmentDTO.Specifications;
                this.StatusId = equipmentDTO.StatusId;
                this.IsFree = equipmentDTO.IsFree;
                this.HasToReClaim = equipmentDTO.HasToReclaim;
                this.EquipmentUom = equipmentDTO.EquipmentUom;
            }
        }
    }
}
