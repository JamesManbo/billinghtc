using AutoMapper;
using ContractManagement.API.Grpc.Clients;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.API.Grpc.Clients.StaticResource;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories;
using ContractManagement.Infrastructure.Repositories.FileRepository;
using Global.Models.Auth;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.RenewContract
{
    public class CURenewContractCommandHandler : BaseTransactionCommandHandler,
        IRequestHandler<CURenewContractCommand, ActionResponse<TransactionDTO>>
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;

        public CURenewContractCommandHandler(ITransactionQueries transactionQueries,
            IProjectQueries projectQueries,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IFileRepository fileRepository,
            UserIdentity userIdentity,
            INotificationGrpcService
            notificationGrpcService,
            IUserGrpcService userGrpcService,
            IContractorQueries contractorQueries,
            IConfiguration config,
            IMapper mapper,
            ITransactionRepository transactionRepository)
            : base(transactionQueries, projectQueries, attachmentFileService, fileRepository, userIdentity, notificationGrpcService, userGrpcService, contractorQueries, config)
        {
            _mapper = mapper;
            _transactionRepository = transactionRepository;
        }

        public async Task<ActionResponse<TransactionDTO>> Handle(CURenewContractCommand request, CancellationToken cancellationToken)
        {
            var actionResp = base.PreHandler(request);
            ActionResponse<Transaction> savedRsp;
            if (!actionResp.IsSuccess)
            {
                return actionResp;
            }

            Transaction transaction;

            if (request.Id == 0)
            {
                transaction = new Transaction(request);
                savedRsp = await _transactionRepository.CreateAndSave(transaction);
            }
            else
            {
                transaction = await _transactionRepository.GetByIdAsync(request.Id);
                transaction.Binding(request);
                savedRsp = await _transactionRepository.UpdateAndSave(transaction);
            }

            actionResp.CombineResponse(savedRsp);
            actionResp.SetResult(this._mapper.Map<TransactionDTO>(savedRsp.Result));


            if (!actionResp.IsSuccess)
            {
                throw new ContractDomainException(actionResp.Message);
            }

            await AttachmentHandler(request, actionResp.Result.Id);
            return actionResp;
        }
    }
}
