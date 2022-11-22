using ContractManagement.Domain.AggregatesModel.OutContractAggregate;

namespace ContractManagement.Domain.Models.OutContracts
{
    public class ImportContractEquipment
    {
        public int Id { get; set; }
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int OutputChannelPointId { get; set; }
        public int OutContractPackageId { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentPictureUrl { get; set; }
        public string EquipmentUom { get; set; } // Đơn vị số lượng
        public decimal UnitPrice { get; set; }
        public float ExaminedUnit { get; set; } // Số lượng khảo sát
        public float ActivatedUnit { get; set; } // Số lượng thiết bị đang hoạt động
        public float RealUnit { get; set; } // Số lượng triển khai thực tế
        public float ReclaimedUnit { get; set; } // Số lượng đã thu hồi
        public float SupporterHoldedUnit { get; set; } // Số lượng đang tạm giữ
        public bool IsInSurveyPlan { get; set; }
        public bool IsFree { get; set; }
        public bool IsActive { get; set; }
        public bool HasToReclaim { get; set; }

        public string SerialCode { get; set; }
        public string MacAddressCode { get; set; }
        public string DeviceCode { get; set; } //Mã thiết bị
        public string Manufacturer { get; set; } //Hãng sản xuất
        public string Specifications { get; set; } //Thông số kỹ thuật
        public int StatusId { get; protected set; }
        public int EquipmentId { get; set; }
        public EquipmentStatus EquipmentStatus { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GrandTotal { get; set; }

        public decimal ExaminedSubTotal { get; set; }
        public decimal ExaminedGrandTotal { get; set; }

        public decimal RealSubTotal { get; set; }
        public decimal RealGrandTotal { get; set; }

        public decimal ReclaimedSubTotal { get; set; }
        public decimal ReclaimedGrandTotal { get; set; }
    }
}
