using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.Domain.Commands.ContractFormCommand
{
    public class CUContractFormCommand : IRequest<ActionResponse<ContractFormDTO>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ServiceId { get; set; }
        public string Content { get; set; }
        public int? DigitalSignatureId { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public PictureDTO DigitalSignature { get; set; }
    }
}
