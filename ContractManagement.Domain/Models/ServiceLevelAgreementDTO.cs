using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class ServiceLevelAgreementDTO : BaseDTO
    {
        public string Uid { get; set; }
        public string Label { get; set; }
        public string Content { get; set; }
        public int OutContractId { get; set; }
        public int? OutContractServicePackageId { get; set; }
        public int ServiceId { get; set; }
        public bool IsDefault { get; set; }
    }
}
