using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.ContractContentCommand
{
    public class CUContractContentCommand : IRequest<ActionResponse<ContractContentDTO>>
    {
        public int Id { get; set; }
        public int? ContractFormId { get; set; }
        public int? OutContractId { get; set; }
        public int? InContractId { get; set; }
        public string Content { get; set; }
        public int? DigitalSignatureId { get; set; }
        public int ContractFormSignatureId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public PictureDTO DigitalSignature { get; set; }
    }
}
