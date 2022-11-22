using AutoMapper;
using ContractManagement.API.Grpc.Clients;
using ContractManagement.API.Grpc.Clients.StaticResource;
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
using ContractManagement.API.Grpc.Clients.ApplicationUser;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Infrastructure.Repositories.ServicePackagePriceRepository;
using Global.Models.Auth;
using Microsoft.Extensions.Configuration;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.Domain.Commands.RadiusAndBrasCommand;
using System.Collections.Generic;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.AddNewServicePackage
{

    public class CUAddNewServicePackageTransactionCommandHandler : BaseTransactionCommandHandler,
        IRequestHandler<CUAddNewServicePackageTransaction, ActionResponse<TransactionDTO>>
    {
        private readonly IMediator _mediator;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly IApplicationUserGrpcService _applicationUserGrpcService;
        private readonly IConfiguration _configuration;

        public CUAddNewServicePackageTransactionCommandHandler(
             ITransactionRepository transactionRepository,
             IAttachmentFileResourceGrpcService attachmentFileService,
             IFileRepository fileRepository,
             IMapper mapper,
             INotificationGrpcService notificationGrpcService,
             ITransactionQueries transactionQueries,
             IProjectQueries projectQueries,
             IContractorQueries contractorQueries,
             IApplicationUserGrpcService applicationUserGrpcService,
             IMediator mediator,
             IConfiguration configuration,
             IUserGrpcService userGrpcService,
             IConfiguration hostConfiguration,
             UserIdentity userIdentity) : base(transactionQueries,
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
            this._applicationUserGrpcService = applicationUserGrpcService;
            this._mediator = mediator;
            _configuration = configuration;
        }

        /// <summary>
        /// Xử lý thêm mới dịch vụ vào transaction, status = 1
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ActionResponse<TransactionDTO>> Handle(CUAddNewServicePackageTransaction request, CancellationToken cancellationToken)
        {
            ActionResponse<Transaction> saveChangeActResponse;
            var actionResp = base.PreHandler(request);
            if (!actionResp.IsSuccess)
            {
                return actionResp;
            }

            request.Type = TransactionType.AddNewServicePackage.Id;

            Transaction transactionEntity;
            if (request.Id == 0)
            {
                transactionEntity = new Transaction(request);
                foreach (var tranChannelCmd in request.TransactionServicePackages)
                {
                    if (request.OutContractId.HasValue && request.OutContractId > 0)
                    {
                        var paymentTarget = _contractorQueries.FindById(tranChannelCmd.PaymentTarget.ApplicationUserIdentityGuid);
                        if (paymentTarget == null)
                        {
                            // Tạo mới/cập nhật paymentTarget
                            var applicationUser = await _applicationUserGrpcService
                                .GetApplicationUserByUid(tranChannelCmd.PaymentTarget.ApplicationUserIdentityGuid);

                            if (applicationUser == null)
                            {
                                throw new ContractDomainException($"Could not find the application user whose ID is '{tranChannelCmd.PaymentTarget.ApplicationUserIdentityGuid}'.");
                            }

                            var createContractorCmd = _mapper.Map<CUContractorCommand>(applicationUser);
                            var cuContractorRsp = await _mediator.Send(createContractorCmd, cancellationToken);
                            if (!cuContractorRsp.IsSuccess)
                            {
                                actionResp.CombineResponse(cuContractorRsp);
                                return actionResp;
                            }
                            tranChannelCmd.PaymentTargetId = cuContractorRsp.Result.Id;
                        }
                        else
                        {
                            tranChannelCmd.PaymentTargetId = paymentTarget.Id;
                        }
                    }
                    else
                    {
                        tranChannelCmd.PaymentTargetId = request.ContractorId;
                    }

                    transactionEntity.AddTransServicePackage(tranChannelCmd);
                }

                saveChangeActResponse = await _transactionRepository.CreateAndSave(transactionEntity);

                if (saveChangeActResponse.IsSuccess
                    && transactionEntity.OutContractId.HasValue)
                {
                    var newTranChannel = transactionEntity.TransactionServicePackages.First(c => c.IsOld != true);
                    if (!string.IsNullOrWhiteSpace(newTranChannel.RadiusAccount))
                    {
                        /// Tạo tài khoản người dùng trên hệ thống Radius
                        var createRadiusAccountEvent = new CreateNewRadiusUserCommand()
                        {
                            ContractCode = transactionEntity.ContractCode,
                            ContractId = transactionEntity.OutContractId.Value,
                            TransactionServicePackages = new List<TransactionServicePackageDTO>() { _mapper.Map<TransactionServicePackageDTO>(newTranChannel) }
                        };

                        actionResp.CombineResponse(await _mediator.Send(createRadiusAccountEvent));
                        if (!actionResp.IsSuccess)
                        {
                            return actionResp;
                        }

                        /// Cập nhật lại trạng thái của kênh vừa tạo là đã tạo tài khoản Radius
                        newTranChannel.IsRadiusAccountCreated = true;
                    }
                }

                await _transactionRepository.SaveChangeAsync();
            }
            else
            {
                transactionEntity = await _transactionRepository.GetByIdAsync(request.Id);
                transactionEntity.Binding(request);
                foreach (var updateCommand in request.TransactionServicePackages)
                {
                    transactionEntity.UpdateServicePackage(updateCommand);
                }

                saveChangeActResponse = await _transactionRepository.UpdateAndSave(transactionEntity);
            }
            actionResp.CombineResponse(saveChangeActResponse);
            actionResp.SetResult(this._mapper.Map<TransactionDTO>(saveChangeActResponse.Result));

            if (!actionResp.IsSuccess)
            {
                return actionResp;
            }

            await AttachmentHandler(request, actionResp.Result.Id);

            if (request.IsFromCustomer)
            {
                var saleRole = _configuration.GetValue<string>("RoleCodeSale");
                var customerCareRole = _configuration.GetValue<string>("RoleCodeCSKH");

                var newChannel = request.TransactionServicePackages.First();
                var notiReq = new PushNotificationRequest()
                {
                    Zone = NotificationZone.Contract,
                    Type = NotificationType.Private,
                    Category = NotificationCategory.ContractTransaction,
                    Title = $"Hợp đồng số {transactionEntity.ContractCode} đăng ký dịch vụ {newChannel.ServiceName} mới",
                    Content = $"Hợp đồng số {actionResp.Result.ContractCode}." +
                        $"\nĐăng ký gói cước {newChannel.PackageName}, dịch vụ {newChannel.ServiceName}.",
                    Payload = JsonConvert.SerializeObject(new
                    {
                        Type = transactionEntity.Type,
                        TypeName = TransactionType.GetTypeName(transactionEntity.Type),
                        Id = saveChangeActResponse.Result.Id,
                        Code = saveChangeActResponse.Result.Code,
                        Category = NotificationCategory.ContractTransaction
                    },
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })
                };

                if (!string.IsNullOrEmpty(saleRole))
                    await _notificationGrpcService.PushNotificationByRole(notiReq, saleRole);

                if (!string.IsNullOrEmpty(customerCareRole))
                    await _notificationGrpcService.PushNotificationByRole(notiReq, customerCareRole);
            }
            else
            {
                await this.PushNotifyOrEmailToImplementers(actionResp.Result, request.Id > 0);
            }

            return actionResp;
        }
    }
}
