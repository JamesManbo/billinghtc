using AutoMapper;
using ContractManagement.API.Grpc.Clients;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.API.Grpc.Clients.StaticResource;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.TransactionCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.Notification;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories;
using ContractManagement.Infrastructure.Repositories.FileRepository;
using Global.Models.Auth;
using Global.Models.Notification;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static ContractManagement.Domain.AggregatesModel.TransactionAggregate.TransactionType;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.RestoreServicePackages
{
    public class CUTransactionRestoreServicePackagesCommandHandler : BaseTransactionCommandHandler,
        IRequestHandler<CUTransactionRestoreServicePackagesCommand, ActionResponse<TransactionDTO>>
    {
        private readonly IMapper _mapper;
        private readonly ITransactionRepository _transactionRepository;

        public CUTransactionRestoreServicePackagesCommandHandler(ITransactionRepository transactionRepository,
            INotificationGrpcService notificationGrpcService,
            ITransactionQueries transactionQueries,
            IProjectQueries projectQueries,
            IUserGrpcService userGrpcService,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IFileRepository fileRepository,
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
            _transactionRepository = transactionRepository;
            this._mapper = mapper;
        }

        public async Task<ActionResponse<TransactionDTO>> Handle(CUTransactionRestoreServicePackagesCommand request, CancellationToken cancellationToken)
        {

            var actionResp = base.PreHandler(request);
            ActionResponse<Transaction> savedRsp;
            if (!actionResp.IsSuccess)
            {
                return actionResp;
            }

            foreach (var item in request.TransactionServicePackages)
            {
                item.StatusId = OutContractServicePackageStatus.Developed.Id;
            }

            Transaction transaction;
            if (request.Id == 0)
            {
                transaction = new Transaction(request);

                var restoreChannels = new List<(string ServiceName, string CId)>();
                foreach (var transSrvPackageCommand in request.TransactionServicePackages)
                {
                    transaction.AddTransServicePackage(transSrvPackageCommand);
                    restoreChannels.Add((transSrvPackageCommand.ServiceName, transSrvPackageCommand.CId));
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
            await PushNotifyOrEmailToImplementers(actionResp.Result, request.Id > 0);

            //if (transaction.IsTechnicalConfirmation == true)
            //{
            //    var notiReq = new PushNotificationRequest()
            //    {
            //        Zone = NotificationZone.Contract,
            //        Type = NotificationType.App,
            //        Category = NotificationCategory.ContractTransaction,
            //        Title = $"Khôi phục dịch vụ {string.Join(", ", restoreChannels.Select(c => c.ServiceName).Distinct())}" +
            //            $" hợp đồng số {transaction.ContractCode}",
            //        Content = $"Hợp đồng số {transaction.ContractCode}.\n" + 
            //            $"Khôi phục dịch vụ các kênh:\n" +
            //            string.Join("", restoreChannels.Select(c => $"Kênh {c.ServiceName} {c.CId}. \n")),
            //        Payload = JsonConvert.SerializeObject(new
            //        {
            //            Type = (int)TransactionTypeEnums.RestoreServicePackage,
            //            TypeName = TransactionType.GetTypeName((int)TransactionTypeEnums.RestoreServicePackage),
            //            Id = saveChangesResponse.Result.Id,
            //            Code = saveChangesResponse.Result.Code,
            //            Category = NotificationCategory.ContractTransaction
            //        }, new JsonSerializerSettings
            //        {
            //            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            //            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //        })
            //    };
            //    await _notificationGrpcService.PushNotificationByUids(notiReq, string.Join(',', SupporterIds
            //}
            return actionResp;
        }
    }
}
