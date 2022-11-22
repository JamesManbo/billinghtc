using DebtManagement.API.Application.IntegrationEvents.Events;
using DebtManagement.API.Grpc.Clients;
using DebtManagement.Infrastructure.Repositories;
using EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.EventHandling
{
    public class NextBillingToReceiptVoucherIntegrationEventHandler : IIntegrationEventHandler<NextBillingToReceiptVoucherIntegrationEvent>
    {
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;

        public NextBillingToReceiptVoucherIntegrationEventHandler(IReceiptVoucherRepository receiptVoucherRepository)
        {
            _receiptVoucherRepository = receiptVoucherRepository;
        }

        public async Task Handle(NextBillingToReceiptVoucherIntegrationEvent @event)
        {
            if(@event == null)
            {
                return;
            }

            var receiptVoucher = await _receiptVoucherRepository.GetByReceiptVoucherDetailAsync(@event.OutContractServicePackageId, @event.OldNextBilling);
            if(receiptVoucher != null)
            {
                if(receiptVoucher.ReceiptVoucherDetails.Count == 1)
                {
                    var lineVoucher = receiptVoucher.ReceiptVoucherDetails.First();
                    receiptVoucher.UpdateReceiptVoucherDetailByNextBilling(lineVoucher.IdentityGuid, @event.NewNextBilling, @event.PaymentPeriod);
                    await _receiptVoucherRepository.UpdateAndSave(receiptVoucher);
                }
                else
                {
                    // Clone phiếu thu
                    var receiptVoucherClone = receiptVoucher.CreateCopy();
                    receiptVoucherClone.IdentityGuid = Guid.NewGuid().ToString();
                    receiptVoucherClone.ClearReceiptVoucherLines();

                    // Lấy ReceiptVoucherDetail
                    var lineVoucher = receiptVoucher.ReceiptVoucherDetails.First(c => c.OutContractServicePackageId == @event.OutContractServicePackageId);
                    lineVoucher.StartBillingDate = @event.NewNextBilling;
                    lineVoucher.EndBillingDate = lineVoucher.StartBillingDate.Value
                        .AddMonths(@event.PaymentPeriod)
                        .AddDays(-1);

                    // Xóa ReceiptVoucherDetail tại phiếu thu cũ
                    receiptVoucher.RemoveReceiptVoucherLine(lineVoucher.IdentityGuid);
                    receiptVoucher.CalculateTotal();
                    _receiptVoucherRepository.Update(receiptVoucher);

                    // Thêm ReceiptVoucherDetail vào phiếu thu mới
                    lineVoucher.IdentityGuid = Guid.NewGuid().ToString();
                    receiptVoucherClone.AddReceiptVoucherDetail(lineVoucher);
                    receiptVoucherClone.CalculateTotal();
                    await _receiptVoucherRepository.CreateAndSave(receiptVoucherClone);
                }
            }
        }
    }
}
