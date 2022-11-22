using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.API.Grpc.Clients.StaticResource;
using ContractManagement.API.Grpc.StaticResource;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.ContractOfTaxAggregate;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.ChangeHistories;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ChangeHistoryRepository;
using ContractManagement.Infrastructure.Repositories.ContractSharingRevenueLineRepository;
using ContractManagement.Infrastructure.Repositories.FileRepository;
using ContractManagement.Infrastructure.Repositories.InContractRepository;
using ContractManagement.Infrastructure.Repositories.InOutContractTaxRepository;
using ContractManagement.Infrastructure.Repositories.PictureRepository;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ContractManagement.API.Application.Commands.InContractCommandHandler
{
    public class
        CreateInContractCommandHandler : IRequestHandler<CreateInContractCommand, ActionResponse<InContractDTO>>
    {
        private readonly IMediator _mediator;
        private readonly IInContractRepository _contractRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IContractorQueries _contractorQueries;
        private readonly IAttachmentFileResourceGrpcService _attachmentFileService;
        private readonly IContractSharingRevenueLineRepository _contractSharingRevenueLineRepository;
        private readonly IInContractTaxRepository _inContractTaxRepository;
        private readonly IContractHistoryRepository _changeHistoryRepository;
        private readonly IInContractQueries _inContractQueries;
        private readonly IUserGrpcService _userGrpcService;
        private readonly IStaticResourceService _staticResourceService;
        private readonly IPictureRepository _pictureRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;

        public CreateInContractCommandHandler(
            IInContractRepository contractRepository,
            IContractSharingRevenueLineRepository contractSharingRevenueLineRepository,
            IFileRepository fileRepository,
            IContractorQueries contractorQueries,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IMediator mediator,
            IInContractQueries inContractQueries,
            IInContractTaxRepository inContractTaxRepository,
            IContractHistoryRepository changeHistoryRepository,
            IContractSharingRevenueQueries contractSharingRevenueQueries,
            IUserGrpcService userGrpcService,
            IWrappedConfigAndMapper configAndMapper,
            IStaticResourceService staticResourceService,
            IPictureRepository pictureRepository)
        {
            this._contractRepository = contractRepository;
            this._fileRepository = fileRepository;
            this._contractorQueries = contractorQueries;
            this._attachmentFileService = attachmentFileService;
            this._mediator = mediator;
            this._contractSharingRevenueLineRepository = contractSharingRevenueLineRepository;
            this._inContractTaxRepository = inContractTaxRepository;
            this._changeHistoryRepository = changeHistoryRepository;
            this._inContractQueries = inContractQueries;
            this._userGrpcService = userGrpcService;
            this._configAndMapper = configAndMapper;
            this._staticResourceService = staticResourceService;
            this._pictureRepository = pictureRepository;
        }

        /// <summary>
        /// Xử lý thêm mới hợp đồng
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ActionResponse<InContractDTO>> Handle(CreateInContractCommand request,
            CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<InContractDTO>();

            if (_inContractQueries.IsCodeExisted(request.ContractCode, request.Id))
            {
                commandResponse.AddError("Số hợp đồng đã tồn tại ", nameof(request.ContractCode));
                return commandResponse;
            }

            var contractHistory = new ContractHistory()
            {
                CreatedBy = request.CreatedBy,
                DateCreated = DateTime.Now,
                ActionName = "Thêm mới",
                Description = "Không",
                IsInContract = true
            };

            #region Binding initialization properties

            // Khởi tạo thể hiện của hợp đồng
            var contractEntity = new InContract(request);

            #endregion

            #region Binding contractor handler

            var contractor = _contractorQueries.FindById(request.Contractor.IdentityGuid);
            if (contractor == null)
            {
                // Tạo mới/cập nhật contractor
                var user = await _userGrpcService.GetUserByUid(request.Contractor.IdentityGuid);
                CUContractorCommand contractorCre = null;
                if (user != null)
                {
                    contractorCre = user
                        .MapTo<CUContractorCommand>(_configAndMapper.MapperConfig);
                }

                var cuContractorRsp = await _mediator.Send(contractorCre, cancellationToken);
                if (!cuContractorRsp.IsSuccess)
                    throw new ContractDomainException(cuContractorRsp.Message);

                contractEntity.SetContractor(cuContractorRsp.Result.Id);
                contractor = cuContractorRsp.Result.MapTo<ContractorDTO>(_configAndMapper.MapperConfig);
            }
            else
            {
                contractEntity.SetContractor(contractor.Id);
            }
            #endregion

            foreach (var createChannelCmd in request.ServicePackages)
            {
                createChannelCmd.PaymentTargetId = contractor.Id;

                // Gán lại thông tin lấy từ db
                createChannelCmd.CreatedBy = contractEntity.CreatedBy;
                // Thêm gói cước vào hợp đồng
                createChannelCmd.StatusId = OutContractServicePackageStatus.Undeveloped.Id;
                contractEntity.AddServicePackage(createChannelCmd);
            }

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

            // Thêm các giá trị phân chia doanh thu
            if (request.ContractSharingRevenues != null)
            //    || contractEntity.ContractTypeId == InContractType.InMaintenance.Id))
            {
                foreach (var csr in request.ContractSharingRevenues)
                {
                    contractEntity.AddContractSharingRevenue(csr);
                }
                contractEntity.TimeLine.StartBillingDate = contractEntity.TimeLine.Signed;
            }

            // Thêm danh sách thông tin liên hệ
            if (request.ContactInfos != null)
            {
                foreach (var ci in request.ContactInfos)
                {
                    ci.CreatedBy = request.CreatedBy;
                    ci.CreatedDate = DateTime.Now;
                    contractEntity.AddContactInfo(ci);
                }
            }

            // Tính tổng các thành phần và giá trị cuối cùng của hợp đồng
            contractEntity.CalculateTotal();
            // Lưu bản ghi hợp đồng
            var savedContractEntityRsp = await _contractRepository.CreateAndSave(contractEntity);
            commandResponse.CombineResponse(savedContractEntityRsp);
            if (!commandResponse.IsSuccess)
            {
                throw new ContractDomainException(commandResponse.Message);
            }

            contractEntity.IdentityGuid = savedContractEntityRsp.Result.IdentityGuid;

            _contractSharingRevenueLineRepository.MapContractSharingRevenueLineToHead();

            //Mapping hợp đồng đầu vào lịch sử
            contractHistory.ContractId = contractEntity.Id;

            #region Add attachment files

            var needToPersistentAttachments = request
                        .AttachmentFiles
                        ?.Where(c => !string.IsNullOrEmpty(c.TemporaryUrl))
                        ?.Select(c => c.TemporaryUrl)
                        ?.ToArray();

            if (needToPersistentAttachments != null && needToPersistentAttachments.Length > 0)
            {
                var attachmentFiles =
                    await _attachmentFileService.PersistentFiles(needToPersistentAttachments);
                if (attachmentFiles == null || !attachmentFiles.Any())
                    throw new ContractDomainException("An error has occured when save the attachment files");

                foreach (var fileCommand in attachmentFiles)
                {
                    fileCommand.Name = request.AttachmentFiles
                        .Find(c => c.TemporaryUrl.Equals(fileCommand.TemporaryUrl, StringComparison.OrdinalIgnoreCase))
                        ?.Name;
                    fileCommand.Name = string.IsNullOrEmpty(fileCommand.Name) ? fileCommand.FileName : fileCommand.Name;
                    fileCommand.CreatedBy = contractEntity.CreatedBy;
                    fileCommand.InContractId = contractEntity.Id;
                    var savedFileRsp = await _fileRepository.CreateAndSave(fileCommand);
                    if (!savedFileRsp.IsSuccess) throw new ContractDomainException(savedFileRsp.Message);
                }
            }

            #endregion

            //Thêm thuế vào hợp đồng trong bảng InContractTax
            if (request.TaxCategories != null)
            {
                foreach (var inContractTax in request.TaxCategories)
                {
                    var newInContractTax = new InContractTax
                    {
                        InContractId = savedContractEntityRsp.Result.Id,
                        TaxCategoryId = inContractTax.TaxCategoryId
                    };
                    await _inContractTaxRepository.CreateAndSave(newInContractTax);
                }
            }

            //Thêm lịch sử 
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var outContractDTO = contractEntity.MapTo<InContractDTO>(_configAndMapper.MapperConfig);
            outContractDTO.Contractor = contractor;
            var json = JsonConvert.SerializeObject(outContractDTO, serializerSettings);
            contractHistory.JsonString = json;
            _ = _changeHistoryRepository.Create(contractHistory);

            return commandResponse;
        }
    }
}