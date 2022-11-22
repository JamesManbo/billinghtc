using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.ReportModels
{
    public class ServicePackage
    {
        public ServicePackage()
        {
            InstallationAddress = new InstallationAddress();
            Equipments = new List<EquipmentForReport>();
        }
        public string Building { get; set; }
        public string Floor { get; set; }
        public string RoomNo { get; set; }
        public decimal PackagePrice { get; set; }
        public string ServiceName { get; set; }
        public string ServicePackageName { get; set; }
        public string BandwithLabel { get; set; }
        public string InternationalBandwidth { get; set; }
        public string DomesticBandwidth { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public InstallationAddress InstallationAddress { get; set; }
        public decimal InstallationFee { get; set; }
        public DateTime? TimeLineEffective { get; set; }
        public int TimeLinePaymentPeriod { get; set; }
        public DateTime? TimeLineSuspensionStartDate { get; set; }
        public DateTime? TimeLineSuspensionEndDate { get; set; }
        public DateTime? TimeLineLatestBilling { get; set; }
        public DateTime? TimeLineRenewPeriod { get; set; }
        public int TimeLinePrepayPeriod { get; set; }
        public DateTime? TimeLineNextBilling { get; set; }
        public DateTime? TimeLineNextDay { get; set; }
        public int TotalMonthUse { get; set; }
        public int PromotionDateQuantity { get; set; }
        public string Content { get; set; }
        public List<EquipmentForReport> Equipments { get; set; }

    }
    public class InstallationAddress : ValueObject
    {
        public string Building { get; set; } = "";
        public string Floor { get; set; } = "";
        public string RoomNumber { get; set; } = "";
        public string Street { get; set; } = "";
        public string District { get; set; } = "";
        public string DistrictId { get; set; } = "";
        public string City { get; set; } = "";
        public string CityId { get; set; } = "";
        public string FullAddress =>
           (string.IsNullOrWhiteSpace(RoomNumber) ? string.Empty : $"Phòng {RoomNumber}, ") +
           (string.IsNullOrWhiteSpace(Floor) ? string.Empty : $"Tầng {Floor}, ") +
           (string.IsNullOrWhiteSpace(Building) ? string.Empty : $"Tòa nhà {Building}, ") +
           (string.IsNullOrWhiteSpace(Street) ? string.Empty : $"{Street}") +
           (string.IsNullOrWhiteSpace(District) ? string.Empty : $", {District}") +
           (string.IsNullOrWhiteSpace(City) ? string.Empty : $", {City}");

        public InstallationAddress()
        {
        }

        public InstallationAddress(string street)
        {
            Street = street;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Street;
            yield return District;
            yield return DistrictId;
            yield return City;
            yield return CityId;
            yield return Building;
            yield return Floor;
            yield return RoomNumber;
        }
    }

    public class ReceiptVoucherForReport
    {
        public string Content { get; set; }
        public DateTime IssuedDate { get; set; }
        public string PaymentDate { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal PaidTotal { get; set; }

    }

    public class EquipmentForReport
    {
        public int? OutContractPackageId { get; set; }
        public int ServiceId { get; set; }
        public int ServicePackageId { get; set; }
        public string EquipmentName { get; set; }
        public string EquipmentSerial { get; set; }
        public int StatusId { get; set; }
        public int EquipmentQuantity { get; set; }
    }
}
