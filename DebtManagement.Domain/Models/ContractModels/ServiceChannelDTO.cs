using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ContractModels
{
    public class ServiceChannelDTO
    {
        public int Id { get; set; }
        public int? InContractId { get; set; }
        public int? OutContractId { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public string Code { get; set; }
        public int Units { get; set; }
        public int Type { get; set; }
        public int UnitOfMeasurementId { get; set; }
        public string CableRoutingNumber { get; set; }
        public string InstallationAddress { get; set; }
        public int InternationalBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public int InternationalBandwidthUnitId { get; set; } // Đơn vị tính băng thông quốc tế
        public int DomesticBandwidth { get; set; }
        public string DomesticBandwidthUom { get; set; }
        public int DomesticBandwidthUnitId { get; set; } // Đơn vị tính băng thông trong nước
        public decimal RentalCharge { get; set; }
    }
}