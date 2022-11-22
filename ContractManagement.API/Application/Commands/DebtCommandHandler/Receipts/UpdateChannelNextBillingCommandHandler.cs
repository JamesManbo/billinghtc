using ContractManagement.Domain.Commands.DebtCommand;
using ContractManagement.Infrastructure.Repositories.ContractServicePackageRepository;
using Global.Models.StateChangedResponse;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.DebtCommandHandler.Receipts
{
    public class UpdateChannelNextBillingCommandHandler : IRequestHandler<UpdateChannelNextBillingCommand, ActionResponse>
    {
        private readonly IContractSrvPckRepository _channelRepository;

        public UpdateChannelNextBillingCommandHandler(IContractSrvPckRepository channelRepository)
        {
            _channelRepository = channelRepository;
        }

        public async Task<ActionResponse> Handle(UpdateChannelNextBillingCommand request, CancellationToken cancellationToken)
        {
            await _channelRepository
                .UpdateNextBillingDate(request.ChannelId, request.OldEndingBillingDate, request.NewEndingBillingDate);
            return ActionResponse.Success;
        }
    }
}
