using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models
{
    public class ContractSharingRevenueDTO
    {
        public ContractSharingRevenueDTO()
        {
            ContractSharingRevenueLines = new List<ContractSharingRevenueLineDTO>();
        }

        public int Id { get; set; }
        public string Uid { get; set; }
        public int InContractId { get; set; }
        public int? OutContractId { get; set; }
        public string InContractCode { get; set; }
        public string OutContractCode { get; set; }
        public int SharingType { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public List<ContractSharingRevenueLineDTO> ContractSharingRevenueLines { get; set; }
    }
}
