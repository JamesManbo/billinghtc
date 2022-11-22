using DebtManagement.Domain.Commands.DebtCommand;
using DebtManagement.Infrastructure.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.Commands.DebtCommandHandler
{
    public class UpdateCurrentDebtOfTargetCommandHandler : IRequestHandler<UpdateCurrentDebtOfTargetCommand, bool>
    {
        private readonly IVoucherTargetRepository _voucherTargetRepository;

        public UpdateCurrentDebtOfTargetCommandHandler(IVoucherTargetRepository voucherTargetRepository)
        {
            this._voucherTargetRepository = voucherTargetRepository;
        }

        public async Task<bool> Handle(UpdateCurrentDebtOfTargetCommand request, CancellationToken cancellationToken)
        {
            await this._voucherTargetRepository.UpdateTargetCurrentDebt(request.TargetId, request.DebtAmount, request.Increase);
            return true;
        }
    }
}
