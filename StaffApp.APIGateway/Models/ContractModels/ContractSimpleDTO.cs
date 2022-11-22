using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ContractModels
{
    public class ContractSimpleDTO
    {
        public int Id { get; set; }
        public string ContractorIdentityGuid { get; set; }
        public string ContractCode { get; set; }
        public string ContractorFullName { get; set; }
        public string ContractorCode { get; set; }
        public string ContractorPhone { get; set; }
        public string ContractorAddress { get; set; }
        public string ContractStatusName { get; set; }
        public int ContractStatusId { get; set; }
        public ContractTimeLineDTO TimeLine { get; set; }
        public List<OutContractServicePackageSimpleDTO> ServicePackages { get; set; }
    }
}
