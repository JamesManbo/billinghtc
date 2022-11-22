using ContractManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.BusinessBlockCommandHandler
{
    public class BusinessBlockCommand : IRequest<ActionResponse<BusinessBlockDTO>>
    {
        public int Id { get; set; }
        public string BusinessBlockName { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
