using StaffApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ContractFormModels
{
    public class ContractFormDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int DigitalSignatureId { get; set; }
        public int ServiceId { get; set; }
        public PictureViewDTO DigitalSignature { get; set; }
    }
}
