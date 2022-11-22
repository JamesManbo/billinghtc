using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class ContractFormDTO: BaseDTO
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public int ServiceId { get; set; }
        public int DigitalSignatureId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public PictureDTO DigitalSignature { get; set; }
    }
}
