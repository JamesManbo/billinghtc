using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class TransactionSlaDTO : BaseDTO
    {
        public string Uid { get; set; }
        public string Label { get; set; }
        public string Content { get; set; }
        public int TransactionId { get; set; }
        public int TransactionServicePackageId { get; set; }
        public int? ContractSlaId { get; set; }
        public int ServiceId { get; set; }
        public bool IsDefault { get; set; }
    }
}
