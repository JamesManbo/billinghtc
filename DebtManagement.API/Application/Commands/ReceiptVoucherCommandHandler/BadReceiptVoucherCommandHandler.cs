using DebtManagement.API.Grpc.Clients;
using DebtManagement.API.Grpc.Clients.StaticResource;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Infrastructure.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.Commands.ReceiptVoucherCommandHandler
{
    public class BadReceiptVoucherCommandHandler : IRequestHandler<BadReceiptVoucherCommand, ActionResponse>
    {
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;
        private readonly IAttachmentFileRepository _attachmentFileRepository;
        private readonly IAttachmentFileResourceGrpcService _attachmentFileResourceGrpcService;

        public BadReceiptVoucherCommandHandler(
            IReceiptVoucherRepository receiptVoucherRepository,
            IAttachmentFileRepository attachmentFileRepository,
            IAttachmentFileResourceGrpcService attachmentFileResourceGrpcService)
        {
            _receiptVoucherRepository = receiptVoucherRepository;
            _attachmentFileRepository = attachmentFileRepository;
            _attachmentFileResourceGrpcService = attachmentFileResourceGrpcService;
        }

        public async Task<ActionResponse> Handle(BadReceiptVoucherCommand request, CancellationToken cancellationToken)
        {
            var actionResponse = new ActionResponse();
            var receiptVoucherEntities = await _receiptVoucherRepository.GetByIdsAsync(request.Ids.ToArray());
            var attachmentFileCommands = await _attachmentFileResourceGrpcService.PersistentFiles(request.AttachmentFiles.Select(e => e.TemporaryUrl).ToArray());

            foreach (var receiptVoucherEntity in receiptVoucherEntities)
            {
                if (!ReceiptVoucherStatus.UnpaidStatuses().Contains(receiptVoucherEntity.StatusId))
                {
                    continue;
                }

                receiptVoucherEntity.BadDebtApprovalContent = request.BadDebtApprovalContent;
                receiptVoucherEntity.SetStatusId(ReceiptVoucherStatus.PayingBadDebt.Id);
                receiptVoucherEntity.UpdatedBy = request.UpdatedBy;
                receiptVoucherEntity.UpdatedDate = DateTime.Now;
                foreach(var rvDetail in receiptVoucherEntity.ValidReceiptVoucherDetails)
                {
                    rvDetail.ReductionFee = rvDetail.GrandTotal;
                }
                receiptVoucherEntity.CalculateTotal();
                actionResponse.CombineResponse(_receiptVoucherRepository.Update(receiptVoucherEntity));

                foreach (var attFile in attachmentFileCommands)
                {
                    //attFile.Id = Guid.NewGuid().ToString();
                    //attFile.ReceiptVoucherId = receiptVoucherEntity.IdentityGuid;
                    attFile.ReceiptVoucherId = receiptVoucherEntity.Id;
                    attFile.CreatedBy = request.UpdatedBy;
                    attFile.CreatedDate = DateTime.Now;
                    actionResponse.CombineResponse(_attachmentFileRepository.Create(attFile));
                }
            }

            if (!actionResponse.IsSuccess)
            {
                throw new DebtDomainException(actionResponse.Message);
            }

            await _receiptVoucherRepository.SaveChangeAsync();
            await _attachmentFileRepository.SaveChangeAsync();
            return actionResponse;
        }
    }
}
