using CustomerApp.APIGateway.Models.CommonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.OutContract
{
    public class ContractContentDTO
    {
        public int? OutContractId { get; set; }
        public int? InContractId { get; set; }
        public string Content { get; set; }
        public int? DigitalSignatureId { get; set; }
        public int ContractFormSignatureId { get; set; }
        public PictureViewModel DigitalSignature { get; set; }
        public PictureViewModel ContractFormSignature { get; set; }
    }
}
