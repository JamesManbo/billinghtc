using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ContractManagement.API.Grpc.Clients.StaticResource;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ContractSharingRevenueLineRepository;
using ContractManagement.Infrastructure.Repositories.ContractSharingRevenueRepository;
using ContractManagement.Infrastructure.Repositories.FileRepository;
using ContractManagement.Infrastructure.Repositories.InContractRepository;
using Global.Models.StateChangedResponse;
using MediatR;
using ContractManagement.Infrastructure.Repositories.ChangeHistoryRepository;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ContractManagement.Domain.AggregatesModel.ContractOfTaxAggregate;
using ContractManagement.Infrastructure.Repositories.InOutContractTaxRepository;
using System.Collections.Generic;
using AutoMapper;
using ContractManagement.Domain.Models.ChangeHistories;
using ContractManagement.Infrastructure.Repositories.ContactInfoRepository;
using ContractManagement.API.Grpc.StaticResource;
using ContractManagement.Infrastructure.Repositories.PictureRepository;
using ContractManagement.Domain.AggregatesModel.Commons;

namespace ContractManagement.API.Application.Commands.InContractCommandHandler
{
    public class UpdateInContractCommandHandler : IRequestHandler<UpdateInContractCommand, ActionResponse<InContractDTO>>
    {
        private readonly IInContractRepository _contractRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IAttachmentFileResourceGrpcService _attachmentFileService;
        private readonly IContractSharingRevenueLineRepository _contractSharingRevenueRepository;
        private readonly IInContractTaxRepository _inContractTaxRepository;
        private readonly IContractHistoryRepository _changeHistoryRepository;
        private readonly IContactInfoRepository _contactInfoRepository;
        private readonly IInContractQueries _inContractQueries;
        private readonly IContractSharingRevenueQueries _contractSharingRevenueQueries;
        private readonly IStaticResourceService _staticResourceService;
        private readonly IPictureRepository _pictureRepository;

        public UpdateInContractCommandHandler(
            IInContractRepository contractRepository,
            IFileRepository fileRepository,
            IContractSharingRevenueLineRepository contractSharingRevenueLineRepository,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IInContractQueries inContractQueries,
            IContractHistoryRepository changeHistoryRepository,
            IInContractTaxRepository inContractTaxRepository,
            IContactInfoRepository contactInfoRepository,
            IContractSharingRevenueQueries contractSharingRevenueQueries,
            IStaticResourceService staticResourceService,
            IPictureRepository pictureRepository)
        {
            _contractRepository = contractRepository;
            _fileRepository = fileRepository;
            _attachmentFileService = attachmentFileService;
            _contractSharingRevenueRepository = contractSharingRevenueLineRepository;
            _changeHistoryRepository = changeHistoryRepository;
            _inContractQueries = inContractQueries;
            _inContractTaxRepository = inContractTaxRepository;
            _contractSharingRevenueQueries = contractSharingRevenueQueries;
            _contactInfoRepository = contactInfoRepository;
            this._staticResourceService = staticResourceService;
            this._pictureRepository = pictureRepository;
        }

        /// <summary>
        /// Xử lý thêm mới hợp đồng
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ActionResponse<InContractDTO>> Handle(UpdateInContractCommand request,
            CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<InContractDTO>();

            #region Binding initialization properties

            // Khởi tạo thể hiện của hợp đồng
            var contractEntity = await _contractRepository.GetByIdAsync(request.Id);
            contractEntity.Update(request);

            //Tạo hợp đồng đầu vào lịch sử
            var contractHistory = new ContractHistory()
            {
                CreatedBy = request.UpdatedBy,
                DateCreated = DateTime.Now,
                ActionName = "Cập nhật",
                ContractId = contractEntity.Id,
                IsInContract = true
            };

            #endregion

            #region Add service, package & equipment

            // Thêm gói cước vào hợp đồng
            foreach (var channelCommand in request
                .ServicePackages)
            {
                if (channelCommand.Id == 0)
                {
                    channelCommand.PaymentTargetId = contractEntity.Contractor.Id;
                    contractEntity.AddServicePackage(channelCommand);
                }
                else
                {
                    contractEntity.UpdateServicePackage(channelCommand);
                }
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

            if(request.DeletedContractSharingRevenues != null &&
                request.DeletedContractSharingRevenues.Count > 0)
            {
                _contractSharingRevenueRepository.DeleteMany(request.DeletedContractSharingRevenues);
            }
            // Cập nhật các giá trị phân chia doanh thu
            if (request.ContractSharingRevenues != null)
            {
                foreach (var csr in request.ContractSharingRevenues)
                {
                    if (csr.Id == 0)
                    {
                        csr.SharingType = (int)contractEntity.ContractTypeId;
                        csr.CreatedBy = request.CreatedBy;
                        csr.CreatedDate = DateTime.UtcNow;
                        contractEntity.AddContractSharingRevenue(csr);
                    }
                    else
                    {
                        _contractSharingRevenueRepository.Update(csr);
                    }
                }
            }

            // Cập nhật danh sách thông tin liên hệ
            _contactInfoRepository.DeleteContactInfo(request.Id);
            if (request.ContactInfos != null && request.ContactInfos.Any())
            {
                foreach (var ci in request.ContactInfos)
                {
                    ci.CreatedBy = request.CreatedBy;
                    ci.CreatedDate = DateTime.Now;
                    contractEntity.AddContactInfo(ci);
                }
            }

            // Cập nhật danh mục thuế
            _inContractTaxRepository.DeleteAllByInContractId(contractEntity.Id);
            if (request.TaxCategories != null)
            {
                foreach (var inContractTax in request.TaxCategories)
                {
                    var newInContractTax = new InContractTax(contractEntity.Id, inContractTax.TaxCategoryId);
                    contractEntity.AddInContractTax(newInContractTax);
                }
            }

            // Cập nhật tỉ lệ phân chia
            //contractEntity.ClearInContractServices();
            //if (request.InContractServices != null && request.InContractServices.Any())
            //{
            //    foreach (var inContractService in request.InContractServices)
            //    {
            //        contractEntity.AddInContractServices(inContractService);
            //    }
            //}
            // Tính tổng các thành phần và giá trị cuối cùng của hợp đồng
            contractEntity.CalculateTotal();

            // Lưu bản ghi hợp đồng
            var savedContractEntityRsp = await _contractRepository.UpdateAndSave(contractEntity);
            commandResponse.CombineResponse(savedContractEntityRsp);
            if (!commandResponse.IsSuccess)
            {
                throw new ContractDomainException(commandResponse.Message);
            }

            _contractSharingRevenueRepository.MapContractSharingRevenueLineToHead();

            #region Add attachment files

            var needToPersistents = request
                .AttachmentFiles?.Where(c => !string.IsNullOrWhiteSpace(c.TemporaryUrl));
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

                    fileCommand.CreatedBy = contractEntity.CreatedBy;
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

            //Thêm lịch sử 
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            var contractDto = _inContractQueries.FindById(contractEntity.Id);
            contractDto.ContractSharingRevenues = _contractSharingRevenueQueries.GetAllByInContractId(contractEntity.Id, contractEntity.ContractTypeId.Value).ToList();
            var json = JsonConvert.SerializeObject(contractDto, serializerSettings);
            contractHistory.JsonString = json;
            _ = _changeHistoryRepository.Create(contractHistory);

            return commandResponse;
        }
    }
}
