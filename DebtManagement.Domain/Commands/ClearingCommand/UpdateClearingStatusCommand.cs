using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.ClearingCommand
{
    public class UpdateClearingStatusCommand : IRequest<ActionResponse>
    {
        public string Id { get; set; }
        public int StatusId { get; set; }
        public string UpdatedBy { get; set; }
    }
}
