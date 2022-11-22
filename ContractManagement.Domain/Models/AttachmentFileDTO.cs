using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class AttachmentFileDTO : BaseDTO
    {
        public int? OutContractId { get; set; }
        public string ResourceStorage { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long Size { get; set; }
        public int FileType { get; set; }
        public string Extension { get; set; }
        public string RedirectLink { get; set; }
        public string TemporaryUrl { get; set; }
    }
}
