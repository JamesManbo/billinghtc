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
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.ChangeLocationServicePackages
{
    public class CUChangeLocationServicePackagesCommandHandler : BaseTransactionCommandHandler,
        IRequestHandler<CUChangeLocationServicePackagesCommand, ActionResponse<TransactionDTO>>
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;

        public CUChangeLocationServicePackagesCommandHandler(ITransactionRepository transactionRepository,
            INotificationGrpcService notificationGrpcService,
            ITransactionQueries transactionQueries,
            IProjectQueries projectQueries,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IFileRepository fileRepository,
            UserIdentity userIdentity,
            IUserGrpcService userGrpcService,
            IContractorQueries contractorQueries,
            IConfiguration hostConfiguration,
            IMapper mapper) : base(transactionQueries,
                projectQueries,
                attachmentFileService,
                fileRepository,
                userIdentity,
                notificationGrpcService,
                userGrpcService,
                contractorQueries,
                hostConfiguration)
        {
            _transactionRepository = transactionRepository;
            this._mapper = mapper;
        }

        public async Task<ActionResponse<TransactionDTO>> Handle(CUChangeLocationServicePackagesCommand request, CancellationToken cancellationToken)
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
                foreach (var transactionChannelCmd in request.TransactionServicePackages)
                {
                    transaction.AddTransServicePackage(transactionChannelCmd);
                }
                savedRsp = await _transactionRepository.CreateAndSave(transaction);
            }
            else
            {
                transaction = await _transactionRepository.GetByIdAsync(request.Id);
                transaction.Binding(request);
                foreach (var channelCmd in request.TransactionServicePackages)
                {
                    transaction.UpdateServicePackage(channelCmd);
                }
                savedRsp = await _transactionRepository.UpdateAndSave(transaction);
            }

            actionResp.CombineResponse(savedRsp);
            actionResp.SetResult(this._mapper.Map<TransactionDTO>(savedRsp.Result));

            if (!actionResp.IsSuccess)
            {
                throw new ContractDomainException(actionResp.Message);
            }

            await AttachmentHandler(request, actionResp.Result.Id);
            await this.PushNotifyOrEmailToImplementers(actionResp.Result, request.Id > 0);

            return actionResp;
        }
    }
}
