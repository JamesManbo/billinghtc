using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.OutContractCommand
{
    public class ImportContractEquipmentCommand : IRequest<ActionResponse<int>>
    {
        public IFormFile FormFileOutContract { get; set; }
        public ImportContractEquipmentCommand(IFormFile formFile)
        {
            FormFileOutContract = formFile;
        }
    }
}
