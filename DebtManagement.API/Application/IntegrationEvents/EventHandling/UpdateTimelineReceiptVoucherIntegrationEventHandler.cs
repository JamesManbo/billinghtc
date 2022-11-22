using DebtManagement.API.Application.IntegrationEvents.Events;
using DebtManagement.Infrastructure.Repositories;
using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.EventHandling
{
    public class UpdateTimelineReceiptVoucherIntegrationEventHandler
        : IIntegrationEventHandler<UpdateTimelineReceiptVoucherIntegrationEvent>
    {
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;
        private readonly IReceiptVoucherDetailRepository _receiptVoucherDetailRepository;

        public UpdateTimelineReceiptVoucherIntegrationEventHandler(IReceiptVoucherDetailRepository receiptVoucherDetailRepository,
            IReceiptVoucherRepository receiptVoucherRepository)
        {
            _receiptVoucherDetailRepository = receiptVoucherDetailRepository;
            _receiptVoucherRepository = receiptVoucherRepository;
        }

        public async Task Handle(UpdateTimelineReceiptVoucherIntegrationEvent @event)
        {
            #region Update billing start, end date of receipt voucher detail

            var receiptVoucherDetail = await _receiptVoucherDetailRepository.FindFirstReceiptDetail(@event.OutServicePackageId);
            if (receiptVoucherDetail != null)
            {
                receiptVoucherDetail.StartBillingDate = @event.TimeLine.StartBilling;
                receiptVoucherDetail.EndBillingDate = @event.TimeLine.NextBilling;
                receiptVoucherDetail.UpdatedBy = "Hệ thống";
                receiptVoucherDetail.UpdatedDate = DateTime.Now;
                await _receiptVoucherDetailRepository.UpdateAndSave(receiptVoucherDetail);
            }

            #endregion

            #region Update issued date of next billing receipt voucher

            var issuedDateNoteValidVch = await _receiptVoucherRepository.GetIssuedDateNotValidVchr(@event.OutServicePackageId);
            if (issuedDateNoteValidVch != null && @event.TimeLine.NextBilling.HasValue)
            {
                issuedDateNoteValidVch.IssuedDate = @event.TimeLine.NextBilling.Value;
                issuedDateNoteValidVch.InvalidIssuedDate = false;
                issuedDateNoteValidVch.UpdatedBy = "Hệ thống";
                issuedDateNoteValidVch.UpdatedDate = DateTime.Now;
                await _receiptVoucherRepository.UpdateAndSave(issuedDateNoteValidVch);
            }

            #endregion


        }
    }
}
