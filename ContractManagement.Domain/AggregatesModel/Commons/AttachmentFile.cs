using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.Commons
{
    [Table("AttachmentFiles")]
    public class AttachmentFile : Entity
    {
        public int? InContractId { get; set; }
        public int? OutContractId { get; set; }
        public int? TransactionId { get; set; }
        public string ResourceStorage { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long Size { get; set; }
        public int FileType { get; set; }
        public string Extension { get; set; }
        public string RedirectLink { get; set; }
    }
}
