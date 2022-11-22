using AutoMapper;
using ContractManagement.API.Grpc.Clients;
using ContractManagement.API.Grpc.Clients.StaticResource;
using ContractManagement.API.Grpc.Clients.Organizations;
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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static ContractManagement.Domain.AggregatesModel.TransactionAggregate.TransactionType;
using ContractManagement.Infrastructure.Repositories.ServicePackagePriceRepository;
using Global.Models.Auth;
using Microsoft.Extensions.Configuration;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.ChangeServicePackage
{
    public class CUChangeServicePackageTransactionCommandHandler : BaseTransactionCommandHandler,
        IRequestHandler<CUChangeServicePackageTransaction, ActionResponse<TransactionDTO>>
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;

        public CUChangeServicePackageTransactionCommandHandler(ITransactionRepository transactionRepository,
             IAttachmentFileResourceGrpcService attachmentFileService,
             IFileRepository fileRepository,
             INotificationGrpcService notificationGrpcService,
             ITransactionQueries transactionQueries,
             IProjectQueries projectQueries,
             IUserGrpcService userGrpcService,
             IMapper mapper,
             IServicePackagePriceRepository packagePriceRepository,
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
        public async Task<ActionResponse<TransactionDTO>> Handle(CUChangeServicePackageTransaction request, CancellationToken cancellationToken)
        {
            var actionResp = base.PreHandler(request);
            ActionResponse<Transaction> savedRsp;
            if (!actionResp.IsSuccess) return actionResp;

            Transaction transactionEntity;
            if (request.Id == 0)
            {
                transactionEntity = new Transaction(request);

                foreach (var tranChannelCmd in request.TransactionServicePackages)
                {
                    transactionEntity.HasEquipment = (tranChannelCmd.StartPoint != null && tranChannelCmd.StartPoint.Equipments.Count() > 0)
                        || (tranChannelCmd.EndPoint != null && tranChannelCmd.EndPoint.Equipments.Count() > 0);
                    transactionEntity.AddTransServicePackage(tranChannelCmd);
                }

                savedRsp = await _transactionRepository.CreateAndSave(transactionEntity);
            }
            else
            {
                transactionEntity = await _transactionRepository.GetByIdAsync(request.Id);
                transactionEntity.Binding(request);
                foreach (var channelCmd in request.TransactionServicePackages)
                {
                    transactionEntity.UpdateServicePackage(channelCmd);
                }
                savedRsp = await _transactionRepository.UpdateAndSave(transactionEntity);
            }

            actionResp.CombineResponse(savedRsp);
            actionResp.SetResult(this._mapper.Map<TransactionDTO>(savedRsp.Result));

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
