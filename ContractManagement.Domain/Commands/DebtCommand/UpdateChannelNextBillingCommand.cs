using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Commands.DebtCommand
{
    public class UpdateChannelNextBillingCommand : IRequest<ActionResponse>
    {
        public UpdateChannelNextBillingCommand()
        {
        }

        public int ChannelId { get; set; }
        public DateTime OldEndingBillingDate { get; set; }
        public DateTime NewEndingBillingDate { get; set; }
    }
}
