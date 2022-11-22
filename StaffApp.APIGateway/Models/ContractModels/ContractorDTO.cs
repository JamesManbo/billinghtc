using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ContractModels
{
    public class ContractorDTO
    {
        public int Id { get; set; }
        public string IdentityGuid { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }
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
        public List<string> ContractCodes { get; set; }
    }
}
