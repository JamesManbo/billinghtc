using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.TransactionsModel
{
    public class TransactionEquipmentDTO
    {
        public int Id { get; set; }
        public int? TransactionId { get; set; }
        public int? TransactionServicePackageId { get; private set; }
        public int? ContractEquipmentId { get; set; }
        public int OutputChannelPointId { get; set; }
        public int EquipmentId { get; set; }
        public int OldEquipmentId { get; set; }
        public string EquipmentName { get; set; }
        public string DeviceCode { get; set; } //Mã thiết bị
        public string Manufacturer { get; set; } //Hãng sản xuất
        public string Specifications { get; set; } //Thông số kỹ thuật
        public MoneyDTO UnitPrice { get; set; }
        public bool IsFree { get; set; }
        public float ExaminedUnit { get; private set; } // Số lượng khảo sát
        public float ActivatedUnit { get; private set; } // Số lượng thiết bị đang hoạt động
        public float RealUnit { get; private set; } // Số lượng triển khai thực tế
        public float ReclaimedUnit { get; private set; } // Số lượng đã thu hồi
        public float SupporterHoldedUnit { get; private set; } // Số lượng đang tạm giữ
        public float WillBeHoldUnit { get; set; } // Số lượng sẽ tạm giữ
        public float WillBeReclaimUnit { get; set; }
        public string EquipmentUom { get; set; }
        public string SerialCode { get; set; }
        public int StatusId { get; set; }
        public bool IsWillBeReclaim { get; set; }
        public bool? IsOld { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public MoneyDTO SubTotal { get; private set; }
        public MoneyDTO GrandTotal { get; private set; }
        public MoneyDTO ExaminedSubTotal { get; private set; }
        public MoneyDTO ExaminedGrandTotal { get; private set; }
        public InstallationAddress InstallAddress { get; set; }
        public string InstallationFullAddress { get; set; }
    }
}
