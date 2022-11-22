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
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Events;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.ChangeHistories;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ChangeHistoryRepository;
using ContractManagement.Infrastructure.Repositories.ContractSharingRevenueLineRepository;
using ContractManagement.Infrastructure.Repositories.FileRepository;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using ContractManagement.Infrastructure.Repositories.PictureRepository;
using ContractManagement.Infrastructure.Repositories.ServicePackagePriceRepository;
using ContractManagement.RadiusDomain.Repositories;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ContractManagement.API.Application.Commands.OutContractCommandHandler
{
    public class CreateContractCommandHandler : CreateUpdateContractBaseHandler,
        IRequestHandler<CreateContractCommand, ActionResponse<OutContractDTO>>
    {
        private readonly IMediator _mediator;
        private readonly IOutContractRepository _contractRepository;
        private readonly IOutContractQueries _outContractQueries;
        private readonly IFileRepository _fileRepository;
        private readonly IRadiusServicePackageQueries _servicePackageQueries;
        private readonly IEquipmentTypeQueries _equipmentTypeQueries;
        private readonly IContractorQueries _contractorQueries;
        private readonly IAttachmentFileResourceGrpcService _attachmentFileService;
        private readonly IContractHistoryRepository _changeHistoryRepository;
        private readonly IApplicationUserGrpcService _applicationUserGrpcService;
        private readonly IContractSharingRevenueLineRepository _contractSharingRevenueLineRepository;
        private readonly ISharingRevenueLineDetailRepository _sharingLineDetailRepository;
        private readonly IProjectQueries _projectQueries;
        private readonly IStaticResourceService _staticResourceService;
        private readonly IPictureRepository _pictureRepository;
        private readonly IServicePackagePriceRepository _packagePriceRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IOutContractServicePackageQueries _channelQueries;

        public CreateContractCommandHandler(IMediator mediator, IOutContractRepository contractRepository,
            IOutContractQueries outContractQueries,
            IFileRepository fileRepository, IRadiusServicePackageQueries servicePackageQueries,
            IEquipmentTypeQueries equipmentTypeQueries, IContractorQueries contractorQueries,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IContractHistoryRepository changeHistoryRepository,
            IApplicationUserGrpcService applicationUserGrpcService,
            IWrappedConfigAndMapper configAndMapper,
            IContractSharingRevenueLineRepository contractSharingRevenueLineRepository,
            IRadiusManagementRepository radiusManagementUserRepository,
            IProjectQueries projectQueries,
            IStaticResourceService staticResourceService,
            IPictureRepository pictureRepository,
            IServicePackagePriceRepository packagePriceRepository,
            ISharingRevenueLineDetailRepository sharingLineDetailRepository,
            IOutContractServicePackageQueries channelQueries)
        {
            _mediator = mediator;
            _contractRepository = contractRepository;
            _outContractQueries = outContractQueries;
            _fileRepository = fileRepository;
            _servicePackageQueries = servicePackageQueries;
            _equipmentTypeQueries = equipmentTypeQueries;
            _contractorQueries = contractorQueries;
            _attachmentFileService = attachmentFileService;
            _changeHistoryRepository = changeHistoryRepository;
            _applicationUserGrpcService = applicationUserGrpcService;
            _configAndMapper = configAndMapper;
            _contractSharingRevenueLineRepository = contractSharingRevenueLineRepository;
            _projectQueries = projectQueries;
            _staticResourceService = staticResourceService;
            _pictureRepository = pictureRepository;
            _packagePriceRepository = packagePriceRepository;
            this._sharingLineDetailRepository = sharingLineDetailRepository;
            _channelQueries = channelQueries;
        }

        /// <summary>
        /// Xử lý thêm mới hợp đồng
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ActionResponse<OutContractDTO>> Handle(CreateContractCommand request,
            CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<OutContractDTO>();

            #region Checking dupplicate contract code handler

            if (_outContractQueries.IsCodeExisted(request.ContractCode, request.Id))
            {
                commandResponse.AddError("Số hợp đồng đã tồn tại", nameof(request.ContractCode));
                return commandResponse;
            }

            foreach (var channel in request.ServicePackages.Where(a => a.IsTechnicalConfirmation == true))
            {
                if (!channel.ProjectId.HasValue)
                {
                    commandResponse.AddError($"Kênh {channel.CId} dịch vụ {channel.ServiceName} yêu cầu kỹ thuật nhưng không thuộc dự án nào", nameof(channel.ProjectId));
                    return commandResponse;
                }
            }

            #endregion

            var contractHistory = new ContractHistory()
            {
                CreatedBy = request.UpdatedBy,
                DateCreated = DateTime.Now,
                ActionName = "Thêm mới",
                Description = "Không",
                IsInContract = false
            };

            #region Binding initialization properties

            // Khởi tạo thể hiện của hợp đồng
            var contractEntity = new OutContract(request);

            #endregion

            #region Binding contractor handler
            // Gán chủ thể bên cung cấp dịch vụ(bên A)
            contractEntity.SetContractorHTC(request.ContractorHTC.Id);

            //Gán/thêm mới chủ thể sử dụng dịch vụ(bên B)
            var contractorIdDictionary = new Dictionary<string, int>();
            var contractor = _contractorQueries.FindById(request.Contractor.IdentityGuid);
            if (contractor == null)
            {
                // Tạo mới/cập nhật contractor
                var applicationUser = await _applicationUserGrpcService
                    .GetApplicationUserByUid(request.Contractor.IdentityGuid);

                CUContractorCommand contractorCre = null;
                if (applicationUser != null)
                {
                    contractorCre = applicationUser
                        .MapTo<CUContractorCommand>(_configAndMapper.MapperConfig);
                    contractorCre.IsBuyer = true;
                }

                var cuContractorRsp = await _mediator.Send(contractorCre, cancellationToken);
                if (!cuContractorRsp.IsSuccess)
                    throw new ContractDomainException(cuContractorRsp.Message);

                contractEntity.SetContractor(cuContractorRsp.Result.Id);
                contractor = cuContractorRsp.Result.MapTo<ContractorDTO>(_configAndMapper.MapperConfig);

                // Tạo mới ContractorPropeties
                CUContractorPropertiesCommand contractorPropertiesCommand = new CUContractorPropertiesCommand(contractor.Id, applicationUser);
                var cuContractorProRsp = await _mediator.Send(contractorPropertiesCommand, cancellationToken);
                if (!cuContractorProRsp.IsSuccess)
                    throw new ContractDomainException(cuContractorProRsp.Message);
            }
            else
            {
                contractEntity.SetContractor(contractor.Id);
            }

            contractorIdDictionary.Add(contractor.ApplicationUserIdentityGuid, contractor.Id);

            #endregion

            #region Add service, package & equipment

            // Thêm/cập nhật danh sách gói cước vào hợp đồng
            //check payment target
            foreach (var packageCommand in request.ServicePackages)
            {
                if (contractorIdDictionary.TryGetValue(packageCommand.PaymentTarget.ApplicationUserIdentityGuid, out int contractorId))
                {
                    packageCommand.PaymentTargetId = contractorId;
                }
                else
                {
                    var paymentTarget = _contractorQueries.FindById(packageCommand.PaymentTarget.ApplicationUserIdentityGuid);
                    if (paymentTarget == null)
                    {
                        // Tạo mới/cập nhật paymentTarget
                        var applicationUser = await _applicationUserGrpcService.GetApplicationUserByUid(packageCommand.PaymentTarget.ApplicationUserIdentityGuid);

                        CUContractorCommand contractorCre = null;
                        if (applicationUser != null)
                        {
                            contractorCre = applicationUser.MapTo<CUContractorCommand>(_configAndMapper.MapperConfig);
                        }

                        var cuContractorRsp = await _mediator.Send(contractorCre, cancellationToken);
                        if (!cuContractorRsp.IsSuccess)
                            throw new ContractDomainException(cuContractorRsp.Message);

                        // Tạo mới ContractorPropeties
                        CUContractorPropertiesCommand contractorPropertiesCommand = new CUContractorPropertiesCommand(cuContractorRsp.Result.Id, applicationUser);
                        var cuContractorProRsp = await _mediator.Send(contractorPropertiesCommand, cancellationToken);
                        if (!cuContractorProRsp.IsSuccess)
                            throw new ContractDomainException(cuContractorProRsp.Message);

                        packageCommand.PaymentTargetId = cuContractorRsp.Result.Id;
                        contractorIdDictionary.Add(packageCommand.PaymentTarget.ApplicationUserIdentityGuid, packageCommand.PaymentTargetId);
                    }
                    else
                    {
                        packageCommand.PaymentTargetId = paymentTarget.Id;
                    }
                }

                // Gán lại thông tin lấy từ db
                packageCommand.CreatedBy = contractEntity.CreatedBy;

                // Thêm gói cước vào hợp đồng
                packageCommand.StatusId = OutContractServicePackageStatus.Undeveloped.Id;
                if (await this._channelQueries.IsCIdExisted(packageCommand.CId))
                {
                    commandResponse.AddError($"Mã CId {packageCommand.CId} tại kênh dịch vụ {packageCommand.ServiceName} đã được sử dụng", nameof(packageCommand.CId));
                    return commandResponse;
                }
                contractEntity.AddServicePackage(packageCommand);
            }

            #endregion

            #region Add contact infomation

            // Thêm danh sách thông tin liên hệ
            if (request.ContactInfos != null)
            {
                foreach (var contactInfo in request.ContactInfos)
                {
                    contactInfo.CreatedBy = request.CreatedBy;
                    contactInfo.CreatedDate = DateTime.Now;
                    contractEntity.AddContactInfo(contactInfo);
                }
            }

            #endregion

            #region Add ContractContent

            if (request.ContractContentCommand != null)
            {
                if (request.ContractContentCommand.DigitalSignature != null
                    && !string.IsNullOrEmpty(request.ContractContentCommand.DigitalSignature.TemporaryUrl))
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

            #region Add/update contract sharing revenue after calculate contract amount
            var sharingRvnDetails = new List<object>();
            if (request.ContractSharingRevenues != null && request.ContractSharingRevenues.Any())
            {
                foreach (var sharingRvnCommand in request.ContractSharingRevenues)
                {
                    contractEntity.AddContractSharingRevenue(sharingRvnCommand);

                    foreach (var sharingLineDetailCmd in sharingRvnCommand.SharingLineDetails)
                    {
                        var sharingLineDetail = new SharingRevenueLineDetail(sharingLineDetailCmd)
                        {
                            CreatedBy = request.CreatedBy,
                            CreatedDate = sharingRvnCommand.CreatedDate
                        };

                        sharingRvnDetails.Add(sharingLineDetail);
                    }
                }
            }

            #region Save changes

            // Tính tổng các thành phần và giá trị cuối cùng của hợp đồng
            contractEntity.CalculateTotal();

            // Lưu bản ghi hợp đồng
            var savedContractEntityRsp = await _contractRepository.CreateAndSave(contractEntity);
            commandResponse.CombineResponse(savedContractEntityRsp);
            if (!commandResponse.IsSuccess)
            {
                throw new ContractDomainException(commandResponse.Message);
            }

            contractEntity = savedContractEntityRsp.Result;

            contractEntity.UpdateStatus(request.ContractStatusId);
            commandResponse.CombineResponse(await _contractRepository.UpdateAndSave(contractEntity));
            if (!commandResponse.IsSuccess)
            {
                throw new ContractDomainException(commandResponse.Message);
            }

            /// Thêm mới hàng loạt các chi tiết giá trị phân chia với hđ đầu vào
            if (sharingRvnDetails.Count > 0)
            {
                foreach (dynamic item in sharingRvnDetails)
                {
                    item.OutContractId = contractEntity.Id;
                    item.OutContractCode = contractEntity.ContractCode;
                }
                await _sharingLineDetailRepository.InsertBulk(sharingRvnDetails);
            }

            _contractSharingRevenueLineRepository.MapContractSharingRevenueLineToHead();
            #endregion

            //Mapping hợp đồng đầu ra lịch sử
            contractHistory.ContractId = contractEntity.Id;

            #endregion

            var outContractDTO = contractEntity.MapTo<OutContractDTO>(_configAndMapper.MapperConfig);
            outContractDTO.Contractor = contractor;

            var orderStartedDomainEvent = new SignedContractStartedDomainEvent(outContractDTO);
            commandResponse.CombineResponse(await _mediator.Send(orderStartedDomainEvent));

            if (!commandResponse.IsSuccess)
            {
                return commandResponse;
            }

            #region Add attachment files

            var needToPersistentAttachments = request.AttachmentFiles
                    ?.Where(c => !string.IsNullOrWhiteSpace(c.TemporaryUrl))
                    ?.Select(c => c.TemporaryUrl)
                    ?.ToArray();

            if (needToPersistentAttachments != null && needToPersistentAttachments.Any())
            {
                var attachmentFiles = await _attachmentFileService.PersistentFiles(needToPersistentAttachments);
                if (attachmentFiles == null || !attachmentFiles.Any())
                    throw new ContractDomainException("An error has occured when save the attachments");

                foreach (var fileCommand in attachmentFiles)
                {
                    fileCommand.Name = request.AttachmentFiles
                           .Find(c => c.TemporaryUrl.Equals(fileCommand.TemporaryUrl, StringComparison.OrdinalIgnoreCase))
                           ?.Name;
                    fileCommand.Name = string.IsNullOrEmpty(fileCommand.Name) ? fileCommand.FileName : fileCommand.Name;
                    fileCommand.CreatedBy = contractEntity.CreatedBy;
                    fileCommand.OutContractId = contractEntity.Id;
                    var savedFileRsp = await _fileRepository.CreateAndSave(fileCommand);
                    if (!savedFileRsp.IsSuccess) throw new ContractDomainException(savedFileRsp.Message);
                }
            }

            #endregion            

            #region Add changing history

            //Thêm lịch sử
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var json = JsonConvert.SerializeObject(outContractDTO, serializerSettings);
            contractHistory.JsonString = json;
            // Fire and forget
            _ = _changeHistoryRepository.Create(contractHistory);

            #endregion

            return commandResponse;
        }
    }
}