using AutoMapper;
using ContractManagement.API.Application.IntegrationEvents.Events.DebtEvents.Payments;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ContractServicePackageRepository;
using ContractManagement.Infrastructure.Repositories.InContractRepository;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.DebtCommandHandler.Payments
{
    public class PaymentVoucherCreatedIntgrnsCommandHandler : IRequestHandler<PaymentVoucherCreatedIntegrationEvent>
    {
        private readonly ILogger<PaymentVoucherCreatedIntgrnsCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IContractSrvPckRepository _contractSrvPckRepository;
        private readonly IInContractRepository _inContractQueries;

        public PaymentVoucherCreatedIntgrnsCommandHandler(ILogger<PaymentVoucherCreatedIntgrnsCommandHandler> logger,
            IMapper mapper,
            IContractSrvPckRepository contractSrvPckRepository, IInContractRepository inContractQueries)
        {
            this._logger = logger;
            this._mapper = mapper;
            this._contractSrvPckRepository = contractSrvPckRepository;
            _inContractQueries = inContractQueries;
        }

        public async Task<Unit> Handle(PaymentVoucherCreatedIntegrationEvent request, CancellationToken cancellationToken)
        {
            if(request.VoucherDetails != null && request.VoucherDetails.Count() > 0)
            {
                foreach (var voucherLine in request.VoucherDetails)
                {
                    var channel = await _contractSrvPckRepository.GetByIdAsync(voucherLine.OutContractServicePackageId);
                    if (voucherLine.EndBillingDate.HasValue)
                    {
                        if (channel.TimeLine.PaymentForm == (int)PaymentMethodForm.Prepaid)
                        {
                            channel.SetNextBillingDate(voucherLine.EndBillingDate.Value.AddDays(1));
                        }
                        else
                        {
                            channel.SetNextBillingDate(voucherLine.EndBillingDate.Value
                                .AddDays(1)
                                .AddMonths(channel.TimeLine.PaymentPeriod));
                        }
                    }

                    if (voucherLine.IsFirstDetailOfService)
                    {
                        channel.SetIsPaidTheFirstBilling();
                    }

                    var updatedResponse = _contractSrvPckRepository.Update(channel);
                    if (!updatedResponse.IsSuccess)
                    {
                        throw new ContractDomainException(updatedResponse.Message);
                    }
                }

                await _contractSrvPckRepository.SaveChangeAsync();
            }

            if(request.NextBillingDate != null && request.NextBillingDate.HasValue)
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
