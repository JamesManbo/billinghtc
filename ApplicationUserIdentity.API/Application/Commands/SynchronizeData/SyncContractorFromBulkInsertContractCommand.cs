using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Application.Commands.SynchronizeData
{
    public class SyncContractorFromBulkInsertContractCommand : IRequest<ActionResponse>
    {
        public int FromContractorId { get; set; }

        public SyncContractorFromBulkInsertContractCommand(int fromContractorId)
        {
            FromContractorId = fromContractorId;
        }
    }
}
