using DebtManagement.API.Grpc.Clients;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.IntegrationEventCommand;
using DebtManagement.Domain.Exceptions;
using DebtManagement.Infrastructure.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.Commands.IntegrationEventCommandHandler
{
    public class TerminateServicePackagesIntegrationEventCommandHandler : IRequestHandler<TerminateServicePackagesIntegrationEventCommand, ActionResponse>
    {
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;

        public TerminateServicePackagesIntegrationEventCommandHandler(IReceiptVoucherRepository receiptVoucherRepository)
        {
            _receiptVoucherRepository = receiptVoucherRepository;
        }

        public async Task<ActionResponse> Handle(TerminateServicePackagesIntegrationEventCommand request, CancellationToken cancellationToken)
        {
            var receiptVouchers = await _receiptVoucherRepository.GetPendingByOCSPIdsGreaterNowAsync(request.OutContractServicePackageIds);

            foreach (var receiptVoucher in receiptVouchers)
            {
                if (receiptVoucher.ReceiptVoucherDetails.Count == 1)
                {
                    receiptVoucher.SetStatusId(ReceiptVoucherStatus.Canceled.Id);
                }
                else
                {
                    foreach (var receiptVoucherDetail in receiptVoucher.ReceiptVoucherDetails.Where(o => request.OutContractServicePackageIds.Contains(o.OutContractServicePackageId)))
                    {
                        receiptVoucherDetail.IsDeleted = true;
                        receiptVoucherDetail.UpdatedDate = DateTime.Now;
                    }

                    if (receiptVoucher.ReceiptVoucherDetails.All(a => a.IsDeleted))
                    {
                        receiptVoucher.SetStatusId(ReceiptVoucherStatus.Canceled.Id);
                    }
                    else
                    {
                        receiptVoucher.CalculateTotal();
                    }
                }
                var updateRsp = _receiptVoucherRepository.Update(receiptVoucher);
                if (!updateRsp.IsSuccess)
                {
                    throw new DebtDomainException(updateRsp.Message);
                }
            }

            await _receiptVoucherRepository.SaveChangeAsync();

            return new ActionResponse();
        }
    }
}
