using ContractManagement.Domain.Models.RadiusAndBras;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.RadiusAndBrasCommand
{
    public class CuRadiusServerInfoCommand : IRequest<ActionResponse<RadiusServerInfoDTO>>
    {
        public int Id { get; set; }
        public string IP { get; set; }
        public int? MarketAreaId { get; set; }
        public int DatabasePort { get; set; }
        public string ServerName { get; set; }
        public string SSHUserName { get; set; }
        public string SSHPassword { get; set; }
        public string DatabaseUserName { get; set; }
        public string DatabasePassword { get; set; }
        public string Description { get; set; }
    }
}
