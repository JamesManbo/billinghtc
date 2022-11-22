using ContractManagement.API.Grpc.Clients;
using ContractManagement.API.Grpc.Clients.StaticResource;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.Notification;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories;
using ContractManagement.Infrastructure.Repositories.FileRepository;
using Global.Models.Notification;
using Global.Models.StateChangedResponse;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static ContractManagement.Domain.AggregatesModel.TransactionAggregate.TransactionType;
using AutoMapper;
using Global.Models.Auth;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.CreateDeployNewOutContract
{
    public class CreateDeployNewOutContractCommandHandler
        : BaseTransactionCommandHandler, IRequestHandler<CreateDeployNewOutContractCommand, ActionResponse<TransactionDTO>>
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;
        public CreateDeployNewOutContractCommandHandler(ITransactionRepository transactionRepository,
             IAttachmentFileResourceGrpcService attachmentFileService,
             IFileRepository fileRepository,
             IProjectQueries projectQueries,
             INotificationGrpcService notificationGrpcService,
             IUserGrpcService userGrpcService,
             ITransactionQueries transactionQueries,
             IMapper mapper,
             IContractorQueries contractorQueries,
             IConfiguration hostConfiguration,
             UserIdentity userIdentity)
            : base(transactionQueries,
                   projectQueries,
                   attachmentFileService,
                   fileRepository,
                   userIdentity,
                   notificationGrpcService,
                   userGrpcService,
                   contractorQueries,
                   hostConfiguration)
        {
            this._transactionRepository = transactionRepository;
            this._mapper = mapper;
        }

        public async Task<ActionResponse<TransactionDTO>> Handle(CreateDeployNewOutContractCommand request, CancellationToken cancellationToken)
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

                foreach (var transactionSrvPackage in request.TransactionServicePackages)
                {
                    transaction.AddTransServicePackage(transactionSrvPackage);
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

            var transactionDto = _mapper.Map<TransactionDTO>(savedRsp.Result);
            transactionDto.Contractor = request.Contractor;
            actionResp.SetResult(transactionDto);

            if (!actionResp.IsSuccess)
            {
                throw new ContractDomainException(actionResp.Message);
            }

            await AttachmentHandler(request, actionResp.Result.Id);
            await PushNotifyOrEmailToImplementers(actionResp.Result, request.Id > 0);
            return actionResp;
        }

    }
}
