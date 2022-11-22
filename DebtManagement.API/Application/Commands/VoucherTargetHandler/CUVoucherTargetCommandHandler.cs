using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DebtManagement.Domain.Commands.BaseVoucherCommand;
using DebtManagement.Domain.Models;
using DebtManagement.Infrastructure.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;

namespace DebtManagement.API.Application.Commands.VoucherTargetHandler
{
    public class CUVoucherTargetCommandHandler : IRequestHandler<CUVoucherTargetCommand, ActionResponse<VoucherTargetDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IVoucherTargetRepository _voucherTargetRepository;
        private readonly IVoucherTargetPropertyRepository _voucherTargetPropertyRepository;

        public CUVoucherTargetCommandHandler(IVoucherTargetRepository voucherTargetRepository, 
            IMapper mapper, 
            IVoucherTargetPropertyRepository voucherTargetPropertyRepository)
        {
            _voucherTargetRepository = voucherTargetRepository;
            this._mapper = mapper;
            this._voucherTargetPropertyRepository = voucherTargetPropertyRepository;
        }

        public async Task<ActionResponse<VoucherTargetDTO>> Handle(CUVoucherTargetCommand request, CancellationToken cancellationToken)
        {
            var actionResponse = new ActionResponse<VoucherTargetDTO>();
            var saveChangesRsp = await _voucherTargetRepository.CreateAndSave(request);

            if (saveChangesRsp.IsSuccess)
            {
                if (request.IsPayer && !string.IsNullOrEmpty(request.ApplicationUserIdentityGuid))
                {
                    _voucherTargetPropertyRepository.SynchronizeVoucherTargetProperties(request.ApplicationUserIdentityGuid, saveChangesRsp.Result.Id);
                }
                actionResponse.SetResult(_mapper.Map<VoucherTargetDTO>(saveChangesRsp.Result));
            }

            return actionResponse;
        }
    }
}
