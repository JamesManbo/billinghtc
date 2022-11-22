using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.InContractModels
{
    public class CUContractSharingRevenue
    {
        public string Uid { get; set; }
        public int Id { get; set; }
        public int InContractId { get; set; }
        public string InContractCode { get; set; }
        public int? OutContractId { get; set; }
        public string OutContractCode { get; set; }
        public int SharingType { get; set; }
        public List<CUContractSharingRevenueLine> ContractSharingRevenueLines { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
