using ContractManagement.Domain.Models.RadiusAndBras;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.RadiusAndBrasCommand
{
    public class CuBrasInfoCommand : IRequest<ActionResponse<BrasInfoDTO>>
    {
        public int Id { get; set; }
        public string IP { get; set; }
        public int ManualAPIPort { get; set; }
        public int? SSHPort { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? ProjectId { get; set; }
        public string Description { get; set; }
    }
}
