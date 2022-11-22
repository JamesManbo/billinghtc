using AutoMapper;
using ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents.Payments;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Infrastructure.Repositories.ContractServicePackageRepository;
using ContractManagement.Infrastructure.Repositories.InContractRepository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.DebtCommandHandler.Payments
{
    public class PaymentVoucherCanceledIntgrnsCommandHandler : IRequestHandler<PaymentVoucherCanceledIntegrationEvent>
    {
        private readonly ILogger<PaymentVoucherCanceledIntgrnsCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IContractSrvPckRepository _contractSrvPckRepository;
        private readonly IInContractRepository _inContractQueries;

        public PaymentVoucherCanceledIntgrnsCommandHandler(ILogger<PaymentVoucherCanceledIntgrnsCommandHandler> logger,
            IMapper mapper,
            IContractSrvPckRepository contractSrvPckRepository, IInContractRepository inContractQueries)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._contractSrvPckRepository = contractSrvPckRepository;
            _inContractQueries = inContractQueries;
        }

        public async Task<Unit> Handle(PaymentVoucherCanceledIntegrationEvent request, CancellationToken cancellationToken)
        {
            if (request.VoucherDetails != null && request.VoucherDetails.Count() > 0)
            {
                var firstBillingChannels = new List<int>();
                var canceledBillingTime = new List<(int, DateTime, DateTime)>();
                foreach (var voucherDetail in request.VoucherDetails)
                {
                    if (voucherDetail.IsFirstDetailOfService)
                    {
                        firstBillingChannels.Add(voucherDetail.OutContractServicePackageId);
                    }

                    if (voucherDetail.StartBillingDate.HasValue
                        && voucherDetail.EndBillingDate.HasValue)
                    {
                        canceledBillingTime.Add(
                            (voucherDetail.OutContractServicePackageId,
                                voucherDetail.StartBillingDate.Value,
                                voucherDetail.EndBillingDate.Value)
                            );
                    }

                }

                if (canceledBillingTime.Any())
                {
                    await _contractSrvPckRepository.UpdateNextBillingDateWhenCancelVoucher(canceledBillingTime);
                }

                if (firstBillingChannels.Count > 0)
                {
                    await this._contractSrvPckRepository.UpdateFirstBillingIsFailed(firstBillingChannels.ToArray());
                }

                await _contractSrvPckRepository.SaveChangeAsync();
            }
            if (request.NextBillingDate.HasValue)
            {
                var nextBilling = request.NextBillingDate.Value;
                var ocspEntity = await _inContractQueries.GetByIdAsync(request.InContractId);

                ocspEntity.SetNextBillingDate(nextBilling);
                await _inContractQueries.SaveChangeAsync();
            }
            return await Task.FromResult(Unit.Value);
        }
    }
}
