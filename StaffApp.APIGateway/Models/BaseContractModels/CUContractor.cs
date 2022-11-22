using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.BaseContractModels
{
    public class CUContractor
    {
        public int Id { get; set; }
        public string IdentityGuid { get; set; }
        public string ContractorShortName { get; set; }
        public string ContractorFullName { get; set; }
        public string ContractorCode { get; set; }
        public string ContractorPhone { get; set; }
        public string ContractorEmail { get; set; }
        public string ContractorFax { get; set; }
        public string AccountingCustomerCode { get; set; }

        public string ContractorCity { get; set; }
        public string ContractorCityId { get; set; }
        public string ContractorDistrict { get; set; }
        public string ContractorDistrictId { get; set; }
        public string ContractorAddress { get; set; }
        public string ContractorIdNo { get; set; }
        public string ContractorTaxIdNo { get; set; }

        public bool IsEnterprise { get; set; }
        public bool IsBuyer { get; set; }
    }
}
