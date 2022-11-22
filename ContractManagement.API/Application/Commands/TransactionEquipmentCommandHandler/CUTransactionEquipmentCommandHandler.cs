using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionEquipmentCommandHandler
{
    public class CUTransactionEquipmentCommandHandler : IRequestHandler<CUTransactionEquipmentCommand, ActionResponse<TransactionEquipmentDTO>>
    {
        private readonly ITransactionEquipmentRepository _transactionEquipmentRepository;

        public CUTransactionEquipmentCommandHandler(ITransactionEquipmentRepository transactionEquipmentRepository)
        {
            _transactionEquipmentRepository = transactionEquipmentRepository;
        }

        public async Task<ActionResponse<TransactionEquipmentDTO>> Handle(CUTransactionEquipmentCommand request, CancellationToken cancellationToken)
        {
            var actionResp = new ActionResponse<TransactionEquipmentDTO>();
            if (request.Id > 0)
            {
                var savedRsp = await _transactionEquipmentRepository.UpdateAndSave(request);
                actionResp.CombineResponse(savedRsp);
                
            }
            else
            {
                var savedRsp = await _transactionEquipmentRepository.CreateAndSave(request);
                actionResp.CombineResponse(savedRsp);
                
            }

            return actionResp;
        }
    }
}
