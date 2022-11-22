using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.ContractContentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.BaseContractModels
{
    public abstract class CreateUpdateContractBase
    {
        protected CreateUpdateContractBase()
        {
        }

        public int Id { get; set; }
        //Đại lý
        public string AgentId { get; set; }
        public string AgentCode { get; set; }
        public int ContractTypeId { get; set; }
        //Vùng thị trường
        public int MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        //Phòng ban phụ trách
        public string OrganizationUnitId { get; set; }
        public string OrganizationUnitCode { get; set; }
        public string OrganizationUnitName { get; set; }
        public string SignedUserName { get; set; }
        public string ContractCode { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public int? ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ContractStatusId { get; set; }
        public string ContractorIdentityGuid { get; set; }
        public int? ContractorId { get; set; }
        public string SalesmanIdentityGuid { get; set; }
        //Nhân viên thu ngân
        public string CashierUserId { get; set; }
        public string Description { get; set; }
        //Nhân viên ký hợp đồng
        public string SignedUserId { get; set; }
        public int? SalesmanId { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal OtherFee { get; set; }
        public string ContractNote { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string CreatorUserId { get; set; }
        public List<CreateUpdateContractTax> TaxCategories { get; set; }
        //Ngày và kỳ hạn
        public ContractTimeLine TimeLine { get; set; }
        //Hình thức thanh toán
        public PaymentMethod Payment { get; set; }
        public CUContractor Contractor { get; set; }
        public CUContractor ContractorHTC { get; set; }
        //Đối soát sự cố
        public bool IsIncidentControl { get; set; }
        //Đối soát dung lượng
        public bool IsControlUsageCapacity { get; set; }
        public int NumberBillingLimitDays { get; set; }
        public CUContractContent ContractContentCommand { get; set; }
    }
}
