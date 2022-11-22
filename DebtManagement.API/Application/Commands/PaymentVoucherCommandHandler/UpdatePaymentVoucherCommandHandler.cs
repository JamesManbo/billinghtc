using DebtManagement.Domain.Commands.PaymentVoucherCommand;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Domain.Models;
using DebtManagement.Infrastructure.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.API.Grpc.Clients;
using DebtManagement.Domain.Events.InContractEvents;
using DebtManagement.API.Grpc.Clients.StaticResource;
using DebtManagement.Domain.AggregatesModel.Commons;
using AutoMapper;

namespace DebtManagement.API.Application.Commands.PaymentVoucherCommandHandler
{
    public class
        UpdatePaymentVoucherCommandHandler : IRequestHandler<UpdatePaymentVoucherCommand,
            ActionResponse<PaymentVoucherDTO>>
    {
        private readonly IMediator _mediator;
        private readonly IPaymentVoucherRepository _paymentVoucherRepository;
        private readonly IAttachmentFileResourceGrpcService _attachmentFileService;
        private readonly IAttachmentFileRepository _fileRepository;

        public UpdatePaymentVoucherCommandHandler(
            IMediator mediator,
            IPaymentVoucherRepository paymentVoucherRepository,
            IAttachmentFileResourceGrpcService attachmentFileService,
            IAttachmentFileRepository fileRepository)
        {
            _mediator = mediator;
            _paymentVoucherRepository = paymentVoucherRepository;
            _attachmentFileService = attachmentFileService;
            _fileRepository = fileRepository;
        }

        /// <summary>
        /// Xử lý cập nhật phiếu chi
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<ActionResponse<PaymentVoucherDTO>> Handle(UpdatePaymentVoucherCommand request,
            CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<PaymentVoucherDTO>();
            if (_paymentVoucherRepository.IsExistByCode(request.VoucherCode, request.Id))
            {
                commandResponse.AddError("Số phiếu chi đã tồn tại ", nameof(request.VoucherCode));
                return commandResponse;
            }

            #region Binding initialization properties
            var voucherEntity = await _paymentVoucherRepository.GetByIdAsync(request.Id);

            if (voucherEntity.IsLock == true)
            {
                commandResponse.AddError("Phiếu chi đang bị khóa");
                return commandResponse;
            }

            if (voucherEntity.StatusId == PaymentVoucherStatus.Canceled.Id)
            //|| voucherEntity.StatusId == PaymentVoucherStatus.Success.Id)
            {
                return commandResponse;
            }
            #endregion

            if (request.PaymentVoucherDetails != null
                && request.PaymentVoucherDetails.Any())
            {
                foreach (var updateDetailCommand in request.PaymentVoucherDetails
                    .Where(p => p.Id > 0))
                {
                    voucherEntity.UpdatePaymentVoucherDetail(updateDetailCommand);

                }

                foreach (var createDetailCommand in request.PaymentVoucherDetails
                    .Where(p => p.Id == 0))
                {
                    voucherEntity.AddPaymentVoucherDetail(createDetailCommand);
                }
                if (request.TypeId == 2 || request.TypeId == 3)
                {
                    var nextBillingDate = request.PaymentVoucherDetails.FirstOrDefault().EndBillingDate.Value.AddDays(1);
                    if (request.isUpdateNextBillingDate && request.StatusId != 6)
                    {
                        voucherEntity.UpdateNextBillingDate(request.InContractId, nextBillingDate);
                    }
                }

                for (int i = 0; i < request.PaymentVoucherDetails.Count; i++)
                {
                    var needToPersistents = request.PaymentVoucherDetails[i].AttachmentFiles
                    ?.Where(c => !string.IsNullOrWhiteSpace(c.TemporaryUrl));

                    if (request.PaymentVoucherDetails[i].AttachmentFiles != null
                    && needToPersistents != null && needToPersistents.Any())
                    {
                        var attachmentFiles =
                            await _attachmentFileService.PersistentFiles(needToPersistents.Select(c => c.TemporaryUrl)
                                .ToArray());
                        if (attachmentFiles == null || !attachmentFiles.Any())
                            throw new DebtDomainException("An error has occured when save the attachment files");

                        for (int j = 0; j < request.PaymentVoucherDetails[i].AttachmentFiles.Count; j++)
                        {
                            var fileCommand = request.PaymentVoucherDetails[i].AttachmentFiles.ElementAt(j);
                            if (attachmentFiles.Count > 0 && fileCommand.TemporaryUrl != null)
                            {
                                var uploadedRspFile = attachmentFiles
                                .Find(e => e.TemporaryUrl.Equals(fileCommand.TemporaryUrl,
                                    StringComparison.InvariantCultureIgnoreCase));

                                fileCommand.FileName = uploadedRspFile.FileName;
                                fileCommand.FilePath = uploadedRspFile.FilePath;
                                //fileCommand.Name = uploadedRspFile.Name;
                                fileCommand.FileType = uploadedRspFile.FileType;
                                fileCommand.Size = uploadedRspFile.Size;
                                fileCommand.Extension = uploadedRspFile.Extension;

                                fileCommand.CreatedBy = voucherEntity.CreatedBy;
                                fileCommand.PaymentVoucherId = voucherEntity.Id;
                                fileCommand.PaymentVoucherDetailId = request.PaymentVoucherDetails[i].Id;

                                ActionResponse<AttachmentFile> savedFileRsp;
                                if (fileCommand.Id == 0)
                                {
                                    savedFileRsp = await _fileRepository.CreateAndSave(fileCommand);
                                }
                                else
                                {
                                    savedFileRsp = await _fileRepository.UpdateAndSave(fileCommand);
                                }

                                if (!savedFileRsp.IsSuccess) throw new DebtDomainException(savedFileRsp.Message);
                            }
                        }
                    }

                    if (request.PaymentVoucherDetails[i].DeletedAttachments != null)
                    {
                        //foreach (var attachmentId in request.PaymentVoucherDetails[i].DeletedAttachments)
                        //{
                        //    _fileRepository.DeleteAndSave(attachmentId);
                        //}
                        _fileRepository.DeleteMany(request.PaymentVoucherDetails[i].DeletedAttachments.ToList());
                    }
                }
            }

            // Chỉnh sửa tiết thanh toán
            if (request.PaymentDetails != null
                && request.PaymentDetails.Any())
            {

                foreach (var paymentDetail in request.PaymentDetails.Where(p => p.Id == 0))
                {
                    bool force = request.PaymentDetails.Any(p => p.PaymentTurn == paymentDetail.PaymentTurn && p.PaidAmount > 0);
                    if (!force && paymentDetail.PaidAmount <= 0) continue;

                    paymentDetail.CreatedBy = request.CreatedBy;
                    voucherEntity.AddPaymentDetail(paymentDetail);
                }

                foreach (var paymentDetail in request.PaymentDetails.Where(p => p.Id > 0))
                {
                    paymentDetail.UpdatedBy = request.UpdatedBy;
                    voucherEntity.UpdatePaymentDetail(paymentDetail);
                }
            }

            voucherEntity.Update(request);
            voucherEntity.CalculateTotal();

            voucherEntity.SetStatusId(request.StatusId);

            var savedVoucherEntityRsp = await _paymentVoucherRepository.UpdateAndSave(voucherEntity);
            commandResponse.CombineResponse(savedVoucherEntityRsp);
            if (!commandResponse.IsSuccess)
            {
                throw new DebtDomainException(commandResponse.Message);
            }

            return commandResponse;
        }
    }
}