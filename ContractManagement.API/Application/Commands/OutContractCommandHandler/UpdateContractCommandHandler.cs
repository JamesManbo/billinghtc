using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContractManagement.API.Application.Commands.ContractCommandHandler;
using ContractManagement.API.Grpc.Clients.ApplicationUser;
using ContractManagement.API.Grpc.Clients.StaticResource;
using ContractManagement.API.Grpc.StaticResource;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.Commons;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Events;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.ChangeHistories;
using ContractManagement.Domain.Seed;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ChangeHistoryRepository;
using ContractManagement.Infrastructure.Repositories.ContractSharingRevenueLineRepository;
using ContractManagement.Infrastructure.Repositories.FileRepository;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using ContractManagement.Infrastructure.Repositories.PictureRepository;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ContractManagement.API.Application.Commands.OutContractCommandHandler
{
    public class UpdateContractCommandHandler : CreateUpdateContractBaseHandler,
        IRequestHandler<UpdateContractCommand, ActionResponse<OutContractDTO>>
    {
        private readonly IMediator _mediator;
        private readonly IOutContractRepository _contractRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IAttachmentFileResourceGrpcService _attachmentFileService;
        private readonly IContractHistoryRepository _changeHistoryRepository;

        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IContractSharingRevenueLineRepository _contractSharingRevenueLineRepository;
        private readonly ISharingRevenueLineDetailRepository _sharingLineDetailRepository;
        private readonly IProjectQueries _projectQueries;
        private readonly IStaticResourceService _staticResourceService;
        private readonly IPictureRepository _pictureRepository;

        private readonly IContractorQueries _contractorQueries;
        private readonly IApplicationUserGrpcService _applicationUserGrpcService;
        private readonly IOutContractServicePackageQueries _channelQueries;


        public UpdateContractCommandHandler(IOutContractRepository contractRepository,
            IFileRepository fileRepository,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IContractHistoryRepository changeHistoryRepository,
            IWrappedConfigAndMapper configAndMapper,
            IMediator mediator,
            IContractSharingRevenueLineRepository contractSharingRevenueLineRepository,
            IProjectQueries projectQueries,
            IStaticResourceService staticResourceService,
            IPictureRepository pictureRepository,
            IContractorQueries contractorQueries,
            IApplicationUserGrpcService applicationUserGrpcService,
            ISharingRevenueLineDetailRepository sharingLineDetailRepository,
            IOutContractServicePackageQueries channelQueries)
        {
            _mediator = mediator;
            _contractRepository = contractRepository;
            _fileRepository = fileRepository;
            _attachmentFileService = attachmentFileService;
            _changeHistoryRepository = changeHistoryRepository;
            _configAndMapper = configAndMapper;
            _contractSharingRevenueLineRepository = contractSharingRevenueLineRepository;
            _projectQueries = projectQueries;
            _staticResourceService = staticResourceService;
            _pictureRepository = pictureRepository;
            _contractorQueries = contractorQueries;
            _applicationUserGrpcService = applicationUserGrpcService;
            this._sharingLineDetailRepository = sharingLineDetailRepository;
            _channelQueries = channelQueries;
        }

        /// <summary>
        /// Xử lý cập nhật hợp đồng đầu ra
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ActionResponse<OutContractDTO>> Handle(UpdateContractCommand request,
            CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<OutContractDTO>();

            foreach (var channel in request.ServicePackages.Where(a => a.IsTechnicalConfirmation == true))
            {
                if (!channel.ProjectId.HasValue)
                {
                    commandResponse.AddError($"Kênh {channel.CId} dịch vụ {channel.ServiceName} yêu cầu kỹ thuật nhưng không thuộc dự án nào", nameof(channel.ProjectId));
                    return commandResponse;
                }
            }

            //Tạo bản ghi lịch sử của hợp đồng
            var contractHistory = new ContractHistory()
            {
                CreatedBy = request.UpdatedBy,
                DateCreated = DateTime.Now,
                ActionName = "Cập nhật",
                ContractId = request.Id,
                IsInContract = false
            };

            // Lấy persistent data của hợp đồng 
            var contractEntity = await _contractRepository.GetByIdAsync(request.Id);
            var oldChannelIds = contractEntity.ServicePackages.Select(s => s.Id).ToArray();
            var hasNewChannel = false;

            #region Add/update attachment files

            var needToPersistents = request.AttachmentFiles
                    ?.Where(c => !string.IsNullOrWhiteSpace(c.TemporaryUrl));

            if (needToPersistents != null && needToPersistents.Any())
            {
                var attachmentFiles =
                    await _attachmentFileService.PersistentFiles(needToPersistents
                        .Select(c => c.TemporaryUrl)
                        .ToArray());

                if (attachmentFiles == null || !attachmentFiles.Any())
                    throw new ContractDomainException("An error has occured when save the attachment files");

                for (int i = 0; i < needToPersistents.Count(); i++)
                {
                    var fileCommand = needToPersistents.ElementAt(i);
                    var uploadedRspFile = attachmentFiles
                        .Find(e => e.TemporaryUrl.Equals(fileCommand.TemporaryUrl,
                            StringComparison.InvariantCultureIgnoreCase));

                    fileCommand.FileName = uploadedRspFile.FileName;
                    fileCommand.Name = string.IsNullOrEmpty(fileCommand.Name) ? fileCommand.FileName : fileCommand.Name;
                    fileCommand.FilePath = uploadedRspFile.FilePath;
                    fileCommand.FileType = uploadedRspFile.FileType;
                    fileCommand.Size = uploadedRspFile.Size;
                    fileCommand.Extension = uploadedRspFile.Extension;

                    fileCommand.CreatedBy = contractEntity.UpdatedBy;
                    fileCommand.OutContractId = contractEntity.Id;

                    ActionResponse<AttachmentFile> savedFileRsp;
                    if (fileCommand.Id == 0)
                    {
                        savedFileRsp = await _fileRepository.CreateAndSave(fileCommand);
                    }
                    else
                    {
                        savedFileRsp = await _fileRepository.UpdateAndSave(fileCommand);
                    }

                    if (!savedFileRsp.IsSuccess) throw new ContractDomainException(savedFileRsp.Message);
                }
            }

            if (request.DeletedAttachments != null)
            {
                foreach (var attachmentId in request.DeletedAttachments)
                {
                    _fileRepository.DeleteAndSave(attachmentId);
                }
            }

            #endregion

            #region Add/update contact info

            // Cập nhật danh sách thông tin liên hệ
            contractEntity.ClearContactInfos();
            if (request.ContactInfos != null && request.ContactInfos.Any())
            {
                foreach (var ci in request.ContactInfos)
                {
                    ci.CreatedBy = request.UpdatedBy;
                    ci.CreatedDate = DateTime.Now;
                    contractEntity.AddContactInfo(ci);
                }
            }

            #endregion

            if (ContractStatus.Liquidated.Id == contractEntity.ContractStatusId)
            {
                contractHistory.Description = $"Cập nhật trạng thái hợp đồng từ {Enumeration.FromValue<ContractStatus>(contractEntity.ContractStatusId)} " +
                    $"sang {Enumeration.FromValue<ContractStatus>(request.ContractStatusId)}";
                goto SaveChanges;
            }

            #region Binding initialization properties
            // Cập nhật các giá trị của hợp đồng
            contractEntity.Update(request);

            contractEntity.IsIncidentControl = request.IsIncidentControl;
            contractEntity.IsControlUsageCapacity = request.IsControlUsageCapacity;

            #endregion

            #region Add service, package & equipment

            // Thêm gói cước vào hợp đồng
            foreach (var addPackageCommand in request
                .ServicePackages
                .Where(s => s.Id == 0))
            {
                hasNewChannel = true;
                // Thêm gói cước vào hợp đồng
                if (addPackageCommand.PaymentTarget.ApplicationUserIdentityGuid != contractEntity.Contractor.IdentityGuid)
                {
                    var paymentTarget = _contractorQueries.FindById(addPackageCommand.PaymentTarget.ApplicationUserIdentityGuid);
                    if (paymentTarget == null)
                    {
                        // Tạo mới/cập nhật paymentTarget
                        var applicationUser = await _applicationUserGrpcService.GetApplicationUserByUid(addPackageCommand.PaymentTarget.ApplicationUserIdentityGuid);

                        CUContractorCommand contractorCre = null;
                        if (applicationUser != null)
                        {
                            contractorCre = applicationUser.MapTo<CUContractorCommand>(_configAndMapper.MapperConfig);
                        }

                        var cuContractorRsp = await _mediator.Send(contractorCre, cancellationToken);
                        if (!cuContractorRsp.IsSuccess)
                            throw new ContractDomainException(cuContractorRsp.Message);

                        addPackageCommand.PaymentTargetId = cuContractorRsp.Result.Id;
                        paymentTarget = cuContractorRsp.Result.MapTo<ContractorDTO>(_configAndMapper.MapperConfig);
                    }
                    else
                    {
                        addPackageCommand.PaymentTargetId = paymentTarget.Id;
                    }
                }
                else
                {
                    addPackageCommand.PaymentTargetId = contractEntity.Contractor.Id;
                }

                if (await this._channelQueries.IsCIdExisted(addPackageCommand.CId))
                {
                    commandResponse.AddError($"Mã CId {addPackageCommand.CId} tại kênh dịch vụ {addPackageCommand.ServiceName} đã được sử dụng", nameof(addPackageCommand.CId));
                    return commandResponse;
                }
                contractEntity.AddServicePackage(addPackageCommand);
            }

            //Cập nhật gói cước nếu đã tồn tại trong hợp đồng
            foreach (var channel in request.ServicePackages
                .Where(s => s.Id > 0))
            {
                var paymentTarget = _contractorQueries.FindById(channel.PaymentTarget.ApplicationUserIdentityGuid);
                if (paymentTarget == null)
                {
                    // Tạo mới/cập nhật paymentTarget
                    var applicationUser = await _applicationUserGrpcService.GetApplicationUserByUid(channel.PaymentTarget.ApplicationUserIdentityGuid);

                    CUContractorCommand contractorCre = null;
                    if (applicationUser != null)
                    {
                        contractorCre = applicationUser.MapTo<CUContractorCommand>(_configAndMapper.MapperConfig);
                    }

                    var cuContractorRsp = await _mediator.Send(contractorCre, cancellationToken);
                    if (!cuContractorRsp.IsSuccess)
                        throw new ContractDomainException(cuContractorRsp.Message);

                    channel.PaymentTargetId = cuContractorRsp.Result.Id;
                    paymentTarget = cuContractorRsp.Result.MapTo<ContractorDTO>(_configAndMapper.MapperConfig);
                }
                else
                {
                    channel.PaymentTargetId = paymentTarget.Id;
                }

                if (await this._channelQueries.IsCIdExisted(channel.CId, channel.Id))
                {
                    commandResponse.AddError($"Mã CId {channel.CId} tại kênh dịch vụ {channel.ServiceName} đã được sử dụng", nameof(channel.CId));
                    return commandResponse;
                }
                contractEntity.UpdateServicePackage(channel);
            }

            // Xóa dịch vụ khỏi hợp đồng
            if (request.DeletedServicePackages != null)
            {
                foreach (var deletedId in request.DeletedServicePackages)
                {
                    contractEntity.RemoveServicePackage(deletedId);
                }
            }

            #endregion

            // Tính tổng các thành phần và giá trị cuối cùng của hợp đồng
            contractEntity.CalculateTotal();

            // Cập nhật danh sách phân chia doanh thu
            #region Add/update contract sharing revenue after calculate contract amount
            //contractEntity.ClearContractSharingRevenues();
            if (request.ContractSharingRevenues != null
                && request.ContractSharingRevenues.Any())
            {
                //lấy ra các bản ghi cần xóa 
                var contractSharingRevenueIds = request.ContractSharingRevenues.Select(x => x.Id).ToArray();

                var sharingLineDetails = new List<object>();
                foreach (var sharingRevenueCommand in request.ContractSharingRevenues)
                {
                    var haveToUpdateDetails = false;
                    if (sharingRevenueCommand.Id == 0)
                    {
                        var dateTimeNow = DateTime.Now;
                        sharingRevenueCommand.Id = 0;
                        sharingRevenueCommand.SharingType = sharingRevenueCommand.SharingType;
                        sharingRevenueCommand.CreatedBy = request.UpdatedBy;
                        sharingRevenueCommand.CreatedDate = dateTimeNow;
                        contractEntity.AddContractSharingRevenue(sharingRevenueCommand);
                        haveToUpdateDetails = true;
                    }
                    else if (sharingRevenueCommand.Id > 0 && sharingRevenueCommand.IsUpdating)
                    {
                        /// Cập nhật qua repository của bảng này thay vì lấy reference qua OutContract
                        /// vì lý do hiệu năng
                        _contractSharingRevenueLineRepository.Update(sharingRevenueCommand);
                        haveToUpdateDetails = true;
                    }

                    if (haveToUpdateDetails)
                    {
                        foreach (var sharingLineDetailCmd in sharingRevenueCommand.SharingLineDetails)
                        {
                            var sharingDetail = new SharingRevenueLineDetail(sharingLineDetailCmd);
                            if (sharingLineDetailCmd.Id == 0)
                            {
                                sharingDetail.CreatedBy = request.UpdatedBy;
                                sharingDetail.CreatedDate = DateTime.Now;
                            }
                            else
                            {
                                sharingDetail.UpdatedBy = request.UpdatedBy;
                                sharingDetail.UpdatedDate = DateTime.Now;
                            }
                            sharingLineDetails.Add(sharingDetail);
                        }
                    }
                }

                if (request.DeletedContractSharingRevenues.Count > 0)
                    _contractSharingRevenueLineRepository
                        .DeleteMany(request.DeletedContractSharingRevenues);

                if (sharingLineDetails.Count > 0)
                    await _sharingLineDetailRepository
                        .InsertBulk(sharingLineDetails);
            }
            #endregion

            #region Add ContractContent

            if (request.ContractContentCommand != null)
            {
                if (!string.IsNullOrEmpty(request.ContractContentCommand?.DigitalSignature?.TemporaryUrl))
                {
                    var storedDigitalSignatureItem =
                                        await _staticResourceService.PersistentImage(request.ContractContentCommand.DigitalSignature.TemporaryUrl);
                    var addDigitalSignatureResponse = await _pictureRepository.CreateAndSave(storedDigitalSignatureItem);
                    if (!addDigitalSignatureResponse.IsSuccess)
                        throw new ContractDomainException(addDigitalSignatureResponse.Message);

                    request.ContractContentCommand.DigitalSignatureId = addDigitalSignatureResponse.Result.Id;
                }

                contractEntity.AddOrUpdateContractContent(request.ContractContentCommand);
            }
        #endregion

        SaveChanges:
            contractEntity.UpdateStatus(request.ContractStatusId);
            // Lưu bản ghi hợp đồng
            var savedContractEntityRsp = await _contractRepository.UpdateAndSave(contractEntity);
            commandResponse.CombineResponse(savedContractEntityRsp);
            if (!commandResponse.IsSuccess)
            {
                return commandResponse;
            }

            contractEntity = savedContractEntityRsp.Result;

            var outContractDTO = contractEntity.MapTo<OutContractDTO>(_configAndMapper.MapperConfig);

            // Map foreign key by guid for sharing revenue tables
            _contractSharingRevenueLineRepository.MapContractSharingRevenueLineToHead();

            /// Gọi sự kiện xử lý các tác vụ ngoài luồng cho kênh mới
            if (hasNewChannel)
            {
                var orderStartedDomainEvent
                    = new SignedContractStartedDomainEvent(outContractDTO, oldChannelIds);
                commandResponse.CombineResponse(await _mediator.Send(orderStartedDomainEvent));
            }

            // Thêm lịch sử 
            contractHistory.Description = string.IsNullOrWhiteSpace(contractHistory.Description) ? "Cập nhật hợp đồng" : contractHistory.Description;
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(contractEntity, serializerSettings);
            contractHistory.JsonString = json;
            _ = _changeHistoryRepository.Create(contractHistory);
            return commandResponse;
        }
    }
}