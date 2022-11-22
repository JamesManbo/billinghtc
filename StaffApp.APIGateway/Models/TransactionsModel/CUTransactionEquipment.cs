using Global.Models.StateChangedResponse;
using MediatR;
using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class CUTransactionEquipment
    {
        public int Id { get; set; }
        public int? TransactionId { get; set; }
        public int? TransactionServicePackageId { get; set; }
        public int? ContractEquipmentId { get; set; }
        public int EquipmentId { get; set; }
        public int OldEquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string DeviceCode { get; set; } //Mã thiết bị
        public string Manufacturer { get; set; } //Hãng sản xuất
        public string Specifications { get; set; } //Thông số kỹ thuật
        public decimal UnitPrice { get; set; }
        public string UnitPriceString { get; set; }
        public bool IsFree { get; set; }
        public int ExaminedUnit { get; set; }
        public int RealUnit { get; set; }
        public int ReclaimedUnit { get; set; }
        public string SerialCode { get; set; }
        public int StatusId { get; set; }
        public bool HasToReclaim { get; set; }
        public bool? IsOld { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal ExaminedSubTotal { get; set; }
        public decimal ExaminedGrandTotal { get; set; }
        public InstallationAddress InstallAddress { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
