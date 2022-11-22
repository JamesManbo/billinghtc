using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ContractModels.AppModel
{
    public class CUContractEquipmentApp
    {
        public int Id { get; set; }
        public int? OutContractPackageId { get; set; }
        public string OutContractPackageUid { get; set; }
        public int EquipmentId { get; set; }
        public int ServiceId { get; set; }
        public int ServicePackageId { get; set; }
        public string EquipmentName { get; set; }
        public float ExaminedUnit { get; set; }
        public float RealUnit { get; set; }
        public float ReclaimedUnit { get; set; }
        public MoneyDTO UnitPrice { get; set; }
        public string SerialCode { get; set; }
        public string DeviceCode { get; set; } //Mã thiết bị
        public string Manufacturer { get; set; } //Hãng sản xuất
        public string Specifications { get; set; } //Thông số kỹ thuật
        public int StatusId { get; set; }
        public bool IsFree { get; set; }
        public bool HasToReClaim { get; set; }
        public string UnitOfMeasurement { get; set; }
//        public Discount Discount { get; set; }
        public MoneyDTO SubTotal { get; private set; }
        public MoneyDTO GrandTotalBeforeTax { get; private set; }
        public MoneyDTO GrandTotal { get; private set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
