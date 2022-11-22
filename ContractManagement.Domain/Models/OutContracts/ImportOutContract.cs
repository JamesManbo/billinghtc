using ContractManagement.Domain.AggregatesModel.ServicePackages;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ContractManagement.Domain.Models.OutContracts
{
    public class ImportOutContract
    {
        public int Id { get; set; }
        public string Culture { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int DisplayOrder { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string IdentityGuid { get; set; }
        public string ContractCode { get; set; }
        public string AgentId { get; set; }
        public string AgentCode { get; set; }
        public int? MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int? ContractTypeId { get; set; }
        public int ContractStatusId { get; set; }
        public int? ContractorId { get; set; }
        [Ignore]
        public string ContractorCode { get; set; }
        public int? ContractorHTCId { get; set; }
        public string SignedUserId { get; set; }
        public string SignedUserName { get; set; }
        public string OrganizationUnitName { get; set; }
        public int? SalesmanId { get; set; }
        public string Description { get; set; }
        public int Payment_Form { get; set; }
        public int Payment_Method { get; set; }
        public string Payment_Address { get; set; }
        public int TimeLine_RenewPeriod { get; set; }
        public int TimeLine_PaymentPeriod { get; set; }
        public DateTime? TimeLine_Expiration { get; set; }
        public DateTime? TimeLine_Liquidation { get; set; }
        public DateTime? TimeLine_Effective { get; set; }
        public DateTime? TimeLine_Signed { get; set; }
        public bool IsIncidentControl { get; set; }
        public bool IsControlUsageCapacity { get; set; }
        public int NumberBillingLimitDays { get; set; }
        public string FiberNodeInfo { get; set; }
        public string ContractNote { get; set; }
        public string AgentContractCode { get; set; }
        public string OrganizationUnitId { get; set; }
        public string CashierUserId { get; set; }
        public string CashierUserName { get; set; }
        public string CashierFullName { get; set; }
        public bool IsAutomaticGenerateReceipt { get; set; }
        public string CustomerCareStaffUserId { get; set; }
        public float TotalTaxPercent { get; set; }
        public int? ContractViolationType { get; set; }

        [Ignore]
        public List<ImportOutContractServicePackage> ServicePackages { get; set; }

        [Ignore]
        public List<ImportContractTotalByCurrency> ContractTotalByCurrencies { get; set; }
    }

    public class ImportOutContractServicePackage
    {
        public int Id { get; set; }
        public ServiceChannelType Type { get; set; }
        public int? OutContractId { get; set; }
        /// <summary>
        /// Số thứ tự của kênh tại hợp đồng bao gồm cả các giao dịch liên quan đến hợp đồng
        /// </summary>
        public int ChannelIndex { get; set; }
        public int? InContractId { get; set; }
        public int? ProjectId { get; set; }
        public string CableRoutingNumber { get; set; } // số hiệu tuyến cáp
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public int? ServicePackageId { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string PackageName { get; set; }
        public bool IsFreeStaticIp { get; set; }
        public string BandwidthLabel { get; set; }
        public float InternationalBandwidth { get; set; }
        public float DomesticBandwidth { get; set; }
        public string InternationalBandwidthUom { get; set; }
        public string DomesticBandwidthUom { get; set; }

        public int TimeLine_PrepayPeriod { get; set; }
        public int TimeLine_PaymentPeriod { get; set; }
        public DateTime? TimeLine_Effective { get; set; }
        public DateTime TimeLine_Signed { get; set; }
        public DateTime? TimeLine_StartBilling { get; set; }
        public DateTime? TimeLine_LatestBilling { get; set; }
        public DateTime? TimeLine_NextBilling { get; set; }
        public DateTime? TimeLine_SuspensionStartDate { get; set; }
        public DateTime? TimeLine_SuspensionEndDate { get; set; }
        public DateTime? TimeLine_TerminateDate { get; set; }
        public int TimeLine_DaysSuspended { get; set; }
        public int TimeLine_DaysPromotion { get; set; }
        public int TimeLine_PaymentForm { get; set; }
        public string CustomerCode { get; set; }
        public string CId { get; set; }
        public string RadiusAccount { get; set; }
        public string RadiusPassword { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal OrgPackagePrice { get; set; }
        public decimal PackagePrice { get; set; }
        public float LineQuantity { get; set; } // số lượng tuyến cáp
        public float? CableKilometers { get; set; } // số kilomet cáp
        public int? StartPointChannelId { get; set; }
        public int EndPointChannelId { get; set; }
        public decimal PromotionAmount { get; set; }
        public decimal EquipmentAmount { get; set; }
        public int StatusId { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public float TaxPercent { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public int? TransactionServicePackageId { get; set; }
        public bool IsInFirstBilling { get; set; }
        public int? RadiusServerId { get; set; }
        public int ChannelGroupId { get; set; }
        public int PaymentTargetId { get; set; }
        public int FlexiblePricingTypeId { get; set; }
        public decimal? MaxSubTotal { get; set; } // Giá dịch vụ tối đa
        public decimal? MinSubTotal { get; set; } // Giá dịch vụ tối thiểu
        public byte IsDefaultSLAByServiceId { get; set; }
        public bool IsRadiusAccountCreated { get; set; }
        public bool IsHasServicePackage { get; set; }
        public bool? IsTechnicalConfirmation { get; set; }
        public bool? IsSupplierConfirmation { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public bool HasDistinguishBandwidth { get; set; }
        [IgnoreDataMember]
        public List<ImportOutContractPackageTax> TaxValues { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        [IgnoreDataMember]
        public ImportOutputChannelPoint StartPoint { get; set; }
        [IgnoreDataMember]
        public ImportOutputChannelPoint EndPoint { get; set; }
    }

    public class ImportOutputChannelPoint
    {
        public int Id { get; set; } // Đơn vị tiền tệ
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public string InstallationAddress_Street { get; set; }
        public string InstallationAddress_District { get; set; }
        public string InstallationAddress_DistrictId { get; set; }
        public string InstallationAddress_City { get; set; }
        public string InstallationAddress_CityId { get; set; }
        public string InstallationAddress_Building { get; set; }
        public string InstallationAddress_Floor { get; set; }
        public string InstallationAddress_RoomNumber { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal MonthlyCost { get; set; }
        public decimal EquipmentAmount { get; set; }
        public string LocationId { get; set; }
    }

    public class ImportOutContractPackageTax
    {
        public string TaxCategoryName { get; set; }
        public string TaxCategoryCode { get; set; }
        public int TaxCategoryId { get; set; }
        public float TaxValue { get; set; }
        public int OutContractServicePackageId { get; set; }
    }

    public class ImportOutContractPackagePromotion
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? OutContractServicePackageId { get; set; }
        public int PromotionDetailId { get; set; }
        public decimal PromotionValue { get; set; }
        public int? PromotionValueType { get; set; }
        public bool IsApplied { get; set; }
        public int NumberMonthApplied { get; set; }
        public int PromotionId { get; set; }
        public string PromotionName { get; set; }
        public int PromotionType { get; set; }
        public string PromotionTypeName { get; set; }
    }

    public class ImportContractor
    {
        public int Id { get; set; }
        public string IdentityGuid { get; set; }
        public string ContractorFullName { get; set; }
        public string ContractorShortName { get; set; }
        public string ContractorUserName { get; set; }
        public string ContractorCode { get; set; }
        public string ContractorPhone { get; set; }
        public string ContractorEmail { get; set; }
        public string ContractorFax { get; set; }
        public string AccountingCustomerCode { get; set; }

        public string ContractorAddress { get; set; }
        public string ContractorIdNo { get; set; }
        public string ContractorTaxIdNo { get; set; }
        public bool IsEnterprise { get; set; }
        public bool IsBuyer { get; set; }
        public bool IsPartner { get; set; }
        public string UserIdentityGuid { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }
        public string Representative { get; set; }
        public string Position { get; set; }
        public string AuthorizationLetter { get; set; }
        public string ContractorCity { get; set; }
        public string ContractorCityId { get; set; }
        public string ContractorDistrict { get; set; }
        public string ContractorDistrictId { get; set; }

    }

    public class ImportContractTotalByCurrency
    {
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int? OutContractId { get; set; }
        public int? InContractId { get; set; }
        public decimal PromotionTotalAmount { get; set; }
        public decimal ServicePackageAmount { get; set; }
        public decimal TotalTaxAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal EquipmentAmount { get; set; }
        public decimal SubTotalBeforeTax { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GrandTotalBeforeTax { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
