using ContractManagement.Domain.AggregatesModel.Commons;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class TransactionEquipmentDTO
    {
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int Id { get; set; }
        public int? TransactionId { get; set; }
        public int? TransactionServicePackageId { get; private set; }
        public int OutputChannelPointId { get; set; }
        public int? ContractEquipmentId { get; set; }
        public int EquipmentId { get; set; }
        public int OldEquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string DeviceCode { get; set; } //Mã thiết bị
        public string Manufacturer { get; set; } //Hãng sản xuất
        public string Specifications { get; set; } //Thông số kỹ thuật
        public string EquipmentUom { get; set; }
        public decimal UnitPrice { get; set; }
        public bool IsFree { get; set; }
        public float ExaminedUnit { get; private set; } // Số lượng khảo sát
        public float ActivatedUnit { get; private set; } // Số lượng thiết bị đang hoạt động
        public float RealUnit { get; private set; } // Số lượng triển khai thực tế
        public float ReclaimedUnit { get; private set; } // Số lượng đã thu hồi
        public float WillBeReclaimUnit { get; set; } // Số lượng sẽ thu hồi
        public float SupporterHoldedUnit { get; set; } // Số lượng đang tạm giữ
        public float WillBeHoldUnit { get; set; } // Số lượng sẽ tạm giữ
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
        public string ChannelCId { get; set; }
        public string InstallationFullAddress { get; set; }
        public InstallationAddress InstallAddress { get; set; }
        public bool IsReplaced => this.IsOld == true && this.Id > 0;
    }
}
