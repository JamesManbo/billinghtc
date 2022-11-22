using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.BaseContractCommand
{
    public abstract class CUDeploymentEquipmentCommand
    {
        public CUDeploymentEquipmentCommand()
        {

        }

        public CUDeploymentEquipmentCommand(TransactionEquipment transEquipment)
        {
            this.OutContractPackageId = transEquipment.OutContractPackageId;
            this.OutputChannelPointId = transEquipment.OutputChannelPointId;
            this.CurrencyUnitId = transEquipment.CurrencyUnitId;
            this.CurrencyUnitCode = transEquipment.CurrencyUnitCode;

            this.EquipmentId = transEquipment.EquipmentId;
            this.EquipmentName = transEquipment.EquipmentName;
            this.ExaminedUnit = transEquipment.ExaminedUnit;
            this.RealUnit = transEquipment.RealUnit;
            this.ReclaimedUnit = transEquipment.ReclaimedUnit;
            this.UnitPrice = transEquipment.UnitPrice;
            this.SerialCode = transEquipment.SerialCode;
            this.MacAddressCode = transEquipment.MacAddressCode;
            this.DeviceCode = transEquipment.DeviceCode;
            this.Manufacturer = transEquipment.Manufacturer;
            this.Specifications = transEquipment.Specifications;
            this.StatusId = transEquipment.StatusId;
            this.IsFree = transEquipment.IsFree;
            this.HasToReClaim = transEquipment.HasToReclaim;
            this.EquipmentUom = transEquipment.EquipmentUom;
            this.CreatedBy = transEquipment.CreatedBy;
            this.UpdatedBy = transEquipment.UpdatedBy;
            this.CreatedDate = transEquipment.CreatedDate;
            this.UpdatedDate = transEquipment.UpdatedDate;
        }

        public int Id { get; set; }
        public int? OutContractPackageId { get; set; }
        public int? OutputChannelPointId { get; set; }
        public string OutContractPackageUid { get; set; }
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public float ExaminedUnit { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public float RealUnit { get; set; }
        public float ReclaimedUnit { get; set; }
        public float SupporterHoldedUnit { get; set; } // Số lượng đang tạm giữ
        public decimal UnitPrice { get; set; }
        public string SerialCode { get; set; }
        public string MacAddressCode { get; set; }
        public string DeviceCode { get; set; } //Mã thiết bị
        public string Manufacturer { get; set; } //Hãng sản xuất
        public string Specifications { get; set; } //Thông số kỹ thuật
        public int StatusId { get; set; }
        public bool IsFree { get; set; }
        public bool HasToReClaim { get; set; }
        public string EquipmentUom { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
