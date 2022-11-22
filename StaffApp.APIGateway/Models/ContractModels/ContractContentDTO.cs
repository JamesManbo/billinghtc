

using StaffApp.APIGateway.Models.CommonModels;

namespace StaffApp.APIGateway.Models.ContractModels
{
    public class ContractContentDTO
    {
        public int? OutContractId { get; set; }
        public int? InContractId { get; set; }
        public string Content { get; set; }
        public int? DigitalSignatureId { get; set; }
        public int ContractFormSignatureId { get; set; }
        public PictureViewDTO DigitalSignature { get; set; }
        public PictureViewDTO ContractFormSignature { get; set; }
    }
}
