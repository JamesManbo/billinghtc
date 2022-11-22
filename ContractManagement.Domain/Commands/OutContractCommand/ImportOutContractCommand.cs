using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.OutContractCommand
{
    public class ImportOutContractCommand : IRequest<ActionResponse<int>>
    {
        public ImportOutContractCommand(IFormFile formFile)
        {
            FormFileOutContract = formFile;
        }

        public IFormFile FormFileOutContract { get; set; }
    }
}
