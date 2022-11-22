using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class ContractContentDTO: BaseDTO
    {
        public int? OutContractId { get; set; }
        public int? InContractId { get; set; }
        public int? ContractFormId { get; set; }
        public string Content { get; set; }
        public int? DigitalSignatureId { get; set; }
        public int ContractFormSignatureId { get; set; }
        public PictureDTO DigitalSignature { get; set; }
        public PictureDTO ContractFormSignature { get; set; }
    }
}
