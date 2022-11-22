using System;
using System.Collections.Generic;
using System.Text;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Models.OutContracts;

namespace ContractManagement.Domain.Models
{
    public class OutContractSimpleDTO : BaseDTO
    {
        public OutContractSimpleDTO()
        {
            ServicePackages = new List<ContractPackageSimpleDTO>();
            ContractTotalByCurrencies = new List<ContractTotalByCurrencyDTO>();
            Direction = "out";
        }
        public readonly string Direction;
        public ContractorSimpleDTO Contractor { get; set; }
        public string ContractCode { get; set; }
        public string AgentId { get; set; }
        public string AgentCode { get; set; }
        public int? ContractTypeId { get; set; }
        public int ContractorId { get; set; }
        public string ContractorFullName { get; set; }
        public int? MarketAreaId { get; protected set; }
        public string MarketAreaName { get; protected set; }
        public int? ProjectId { get; protected set; }
        public string ProjectName { get; protected set; }
        public string OrganizationUnitId { get; set; }
        public string OrganizationUnitName { get; protected set; }
        public int ContractStatusId { get; set; }
        public string ContractStatusName => ContractStatus.From(ContractStatusId).Name ?? "";
        public string Description { get; set; }
        public string SignedUserId { get; protected set; }
        public string SignedUserName { get; protected set; }
        public int? SalesmanId { get; protected set; }
        public string ContractNote { get; set; }
        public string ContractorIdentityGuid { get; set; }
        public ContractTimeLine TimeLine { get; set; }
        public bool HasUploadedFiles { get; set; }
        public List<ContractPackageSimpleDTO> ServicePackages { get; set; }
        public List<ContractTotalByCurrencyDTO> ContractTotalByCurrencies { get; set; }

    }

    public class ContractPackageSimpleDTO : BaseDTO
    {
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int ServicePackageId { get; set; }
        public string PackageName { get; set; }
        public decimal PackagePrice { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal PromotionAmount { get; set; }
        public decimal OtherFee { get; set; }
        public decimal EquipmentAmount { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal PromotionTotalAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string BandwidthLabel { get; set; }
        public bool HasStartAndEndPoint { get; set; }
        public OutputChannelPointSimpleDTO StartPoint { get; set; }
        public OutputChannelPointSimpleDTO EndPoint { get; set; }
        public BillingTimeLine TimeLine { get; set; }
    }

    public class OutputChannelPointSimpleDTO: BaseDTO
    {
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
        public OutputChannelPointTypeEnum PointType { get; set; }
        public InstallationAddress InstallationAddress { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public decimal MonthlyCost { get; set; }
        public decimal EquipmentAmount { get; set; }
    }

    public class ContractorSimpleDTO : BaseDTO
    {
        public string ContractorFullName { get; set; }
        public string ContractorPhone { get; set; }
        public string ContractorCode { get; set; }
    }
}
