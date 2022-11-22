using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionServicePackageCommandHandler
{
    public class CUTransactionServicePackageCommandHandler : IRequestHandler<CUTransactionServicePackageCommand, ActionResponse<TransactionServicePackageDTO>>
    {
        private readonly ITransactionServicePackageRepository _transactionServicePackageRepository;

        public CUTransactionServicePackageCommandHandler(ITransactionServicePackageRepository transactionServicePackageRepository)
        {
            _transactionServicePackageRepository = transactionServicePackageRepository;
        }
        public async Task<ActionResponse<TransactionServicePackageDTO>> Handle(CUTransactionServicePackageCommand request, CancellationToken cancellationToken)
        {
            var actionResp = new ActionResponse<TransactionServicePackageDTO>();
            if (request.Id > 0)
            {
                var savedRsp = await _transactionServicePackageRepository.UpdateAndSave(request);
                actionResp.CombineResponse(savedRsp);

            }
            else
            {
                var savedRsp = await _transactionServicePackageRepository.CreateAndSave(request);
                actionResp.CombineResponse(savedRsp);

            }

            return actionResp;
        }
    }
}
