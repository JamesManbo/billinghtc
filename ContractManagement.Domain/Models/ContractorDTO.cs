using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class ContractorDTO : BaseDTO
    {
        public ContractorDTO()
        {
            ContractCodes = new List<string>();
        }
        public string IdentityGuid { get; set; }
        public string ContractorUserName { get; set; }
        public string ContractorFullName { get; set; }
        public string ContractorCode { get; set; }
        public string ContractorPhone { get; set; }
        public string ContractorEmail { get; set; }
        public string ContractorFax { get; set; }
        public string ContractorAddress { get; set; }
        public string ContractorIdNo { get; set; }
        public string ContractorTaxIdNo { get; set; }
        public bool IsEnterprise { get; set; }
        public bool IsBuyer { get; set; }
        public bool IsHTC { get; set; }
        public List<string> ContractCodes { get; set; }
        public bool IsPartner { get; set; }
        public string UserIdentityGuid { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }
        public string AccountingCustomerCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string Representative { get; set; }
        public string Position { get; set; }
        public string AuthorizationLetter { get; set; }
        public string ContractorShortName { get; set; }
        public string ContractorCity { get; set; }
        public string ContractorCityId { get; set; }
        public string ContractorDistrict { get; set; }
        public string ContractorDistrictId { get; set; }
    }
}
