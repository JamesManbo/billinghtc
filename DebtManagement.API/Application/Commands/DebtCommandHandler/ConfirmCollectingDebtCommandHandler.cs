using DebtManagement.Domain.Commands.DebtCommand;
using DebtManagement.Infrastructure.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.Commands.DebtCommandHandler
{
    public class ConfirmCollectingDebtCommandHandler : IRequestHandler<ConfirmCollectingDebtCommand, ActionResponse>
    {
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;

        public ConfirmCollectingDebtCommandHandler(IReceiptVoucherRepository receiptVoucherRepository)
        {
            _receiptVoucherRepository = receiptVoucherRepository;
        }

        public async Task<ActionResponse> Handle(ConfirmCollectingDebtCommand request, CancellationToken cancellationToken)
        {
            await _receiptVoucherRepository.ConfirmCollectionOnBehalfDebt(request.ReceiptVoucherIds, request.ApprovedUserId, request.ConfirmationDate);
            await _receiptVoucherRepository.SaveChangeAsync();
            return ActionResponse.Success;
        }
    }
}
