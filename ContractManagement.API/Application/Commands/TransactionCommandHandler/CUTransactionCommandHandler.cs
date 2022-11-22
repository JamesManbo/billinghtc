using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler
{
    public class CUTransactionCommandHandler : IRequestHandler<CUTransactionCommand, ActionResponse<TransactionDTO>>
    {
        private readonly ITransactionRepository _transactionRepository;
        public CUTransactionCommandHandler(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<ActionResponse<TransactionDTO>> Handle(CUTransactionCommand request, CancellationToken cancellationToken)
        {
            var actionResp = new ActionResponse<TransactionDTO>();
            request.StatusId = TransactionStatus.WaitAcceptanced.Id;
            if (request.Id > 0)
            {
                var savedRsp = await _transactionRepository.UpdateAndSave(request);
                actionResp.CombineResponse(savedRsp);
            }
            else
            {
                var savedRsp = await _transactionRepository.CreateAndSave(request);
                actionResp.CombineResponse(savedRsp);

            }

            return actionResp;
        }
    }
}
