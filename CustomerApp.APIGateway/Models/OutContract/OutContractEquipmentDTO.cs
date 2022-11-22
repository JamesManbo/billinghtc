using CustomerApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.OutContract
{
    public class OutContractEquipmentDTO
    {
        public int Id { get; set; }
        public string ChannelCId { get; set; }
        public int OutContractId { get; set; }
        public int OutputChannelPointId { get; set; }
        public int OutContractPackageId { get; set; }
        public string InstallationFullAddress { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentPictureUrl { get; set; }
        public string EquipmentUom { get; set; }
        public MoneyDTO UnitPrice { get; set; }
        public float ExaminedUnit { get; private set; } // Số lượng khảo sát
        public float ActivatedUnit { get; private set; } // Số lượng thiết bị đang hoạt động
        public float RealUnit { get; private set; } // Số lượng triển khai thực tế
        public float ReclaimedUnit { get; private set; } // Số lượng đã thu hồi
        public DiscountDTO Discount { get; set; }
        public bool IsInSurveyPlan { get; set; }
        public bool IsFree { get; set; }
        public bool HasToReclaim { get; set; }
        public string SerialCode { get; set; }
        public string DeviceCode { get; set; }
        public string Manufacturer { get; set; }
        public string Specifications { get; set; }
        public int StatusId { get; set; }
        public int EquipmentId { get; set; }
        public MoneyDTO GrandTotal { get; set; }
        public MoneyDTO UnitPriceBeforeTax { get; set; }
    }
}
