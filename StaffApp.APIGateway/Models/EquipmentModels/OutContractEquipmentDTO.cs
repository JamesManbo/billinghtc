using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.EquipmentModels
{
    public class OutContractEquipmentDTO
    {
        public int Id { get; set; }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int OutContractId { get; set; }
        public string ChannelCId { get; set; }
        public int OutputChannelPointId { get; set; }
        public string InstallationFullAddress { get; set; }
        public int OutContractPackageId { get; set; }
        public int ServicePackagePoint { get; set; }
        public string CId { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentPictureUrl { get; set; }
        public string EquipmentUom { get; set; } // đơn vị số lượng
        public MoneyDTO UnitPrice { get; set; }
        public float ExaminedUnit { get; private set; } // Số lượng khảo sát
        public float ActivatedUnit { get; private set; } // Số lượng thiết bị đang hoạt động
        public float RealUnit { get; private set; } // Số lượng triển khai thực tế
        public float ReclaimedUnit { get; private set; } // Số lượng đã thu hồi
        public bool IsInSurveyPlan { get; set; }
        public bool IsFree { get; set; }
        public bool HasToReclaim { get; set; }
        public string SerialCode { get; set; }
        public string DeviceCode { get; set; } //Mã thiết bị
        public string Manufacturer { get; set; } //Hãng sản xuất
        public string Specifications { get; set; } //Thông số kỹ thuật
        public int StatusId { get; set; }
        //public string StatusName => EquipmentStatus.From(this.StatusId)?.Name;
        public int EquipmentId { get; set; }
        public MoneyDTO SubTotal { get; set; }
        public MoneyDTO GrandTotal { get; set; }
        public MoneyDTO ExaminedSubTotal { get; set; }
        public MoneyDTO ExaminedGrandTotal { get; set; }
        public int? TransactionEquipmentId { get; set; }
    }
}
