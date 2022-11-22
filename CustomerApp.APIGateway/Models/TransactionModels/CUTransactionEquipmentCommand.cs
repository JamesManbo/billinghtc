using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.TransactionModels
{
    public class CUTransactionEquipmentCommand
    {
        public int Id { get; set; }
        public int? OutContractPackageId { get; set; }
        public string OutContractPackageUid { get; set; }
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public float ExaminedUnit { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public float RealUnit { get; set; }
        public float ReclaimedUnit { get; set; }
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
        public DateTime UpdatedDate { get; set; }


        public int? TransactionId { get; set; }
        public int? TransactionServicePackageId { get; set; }
        public int? ContractEquipmentId { get; set; }
        public int? OutContractId { get; set; }
        public int OldEquipmentId { get; set; }
        public bool? IsOld { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public bool? IsAcceptanced { get; set; }
    }
}
