using DebtManagement.API.Application.IntegrationEvents.Events;
using DebtManagement.API.Grpc.Clients;
using DebtManagement.Domain.Commands.ContractEventsCommand;
using DebtManagement.Domain.Events.ContractEvents;
using DebtManagement.Infrastructure.Repositories;
using EventBus.Abstractions;
using MediatR;
using OfficeOpenXml.ConditionalFormatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.IntegrationEvents.EventHandling
{
    public class RestoreOrSuspendServicesIntegrationEventHandler : IIntegrationEventHandler<RestoreOrSuspendServicesIntegrationEvent>
    {
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;
        private readonly ITaxCategoryGrpcService _taxCategoryGrpcService;
        private readonly IMediator _mediator;

        public RestoreOrSuspendServicesIntegrationEventHandler(IReceiptVoucherRepository receiptVoucherRepository, 
            ITaxCategoryGrpcService taxCategoryGrpcService,
            IMediator mediator)
        {
            _receiptVoucherRepository = receiptVoucherRepository;
            _taxCategoryGrpcService = taxCategoryGrpcService;
            _mediator = mediator;
        }

        public async Task Handle(RestoreOrSuspendServicesIntegrationEvent @event)
        {
            if (!@event.OutContractServicePackageEvents.Any())
            {
                return;
            }

            if (@event.IsActive)
            {
                var servicePackageSuspensionTimeEvents = new List<ServicePackageSuspensionTimeEvent>();
                for (int i = 0; i < @event.OutContractServicePackageEvents.Count; i++)
                {
                    if (@event.OutContractServicePackageEvents[i] != null)
                    {
                        var receiptVoucher = await _receiptVoucherRepository.GetByOutContractServicePackageAsync(@event.OutContractServicePackageEvents[i]);
                        if (receiptVoucher != null)
                        {
                            receiptVoucher.IsActive = @event.IsActive;
                            var receiptVoucherDetail = receiptVoucher.ReceiptVoucherDetails.FirstOrDefault(o => o.OutContractServicePackageId == @event.OutContractServicePackageEvents[i].Id);
                            if (receiptVoucherDetail != null)
                            {
                                receiptVoucherDetail.IsActive = @event.IsActive;

                                if(receiptVoucherDetail.StartBillingDate < @event.OutContractServicePackageEvents[i].SuspensionEndDate)
                                {
                                    receiptVoucherDetail.StartBillingDate = @event.OutContractServicePackageEvents[i].SuspensionEndDate;
                                    receiptVoucherDetail.EndBillingDate = @event.OutContractServicePackageEvents[i].SuspensionEndDate.Value.AddMonths(@event.OutContractServicePackageEvents[i].PaymentPeriod);
                                }

                                if (@event.OutContractServicePackageEvents[i].RemainingAmount > receiptVoucherDetail.GrandTotal)
                                {
                                    servicePackageSuspensionTimeEvents.Add(new ServicePackageSuspensionTimeEvent()
                                    {
                                        Id = (int)@event.OutContractServicePackageEvents[i].ServicePackageSuspensionTimeId,
                                        RemainingAmount = (int)@event.OutContractServicePackageEvents[i].RemainingAmount - receiptVoucherDetail.GrandTotal
                                    });
                                    receiptVoucherDetail.DiscountAmountSuspend += receiptVoucherDetail.GrandTotal;
                                    receiptVoucherDetail.GrandTotal = 0;
                                }
                                else
                                {
                                    servicePackageSuspensionTimeEvents.Add(new ServicePackageSuspensionTimeEvent()
                                    {
                                        Id = (int)@event.OutContractServicePackageEvents[i].ServicePackageSuspensionTimeId,
                                        RemainingAmount = 0
                                    });
                                    receiptVoucherDetail.DiscountAmountSuspend += @event.OutContractServicePackageEvents[i].RemainingAmount ?? 0;
                                    receiptVoucherDetail.GrandTotal -= @event.OutContractServicePackageEvents[i].RemainingAmount ?? 0;
                                }
                            }
                            receiptVoucher.CalculateTotal();
                            _receiptVoucherRepository.Update(receiptVoucher);
                        }
                    }
                }
                if (servicePackageSuspensionTimeEvents.Any())
                {
                    var uSPSTIECommand = new UpdateSPSuspensionTimesIntegrationEventCommand(servicePackageSuspensionTimeEvents);
                    await _mediator.Send(uSPSTIECommand);
                }
            }
            else
            {
                for (int i = 0; i < @event.OutContractServicePackageEvents.Count; i++)
                {
                    if (@event.OutContractServicePackageEvents[i] != null)
                    {
                        var receiptVoucher = await _receiptVoucherRepository.GetByOutContractServicePackageAsync(@event.OutContractServicePackageEvents[i]);
                        if (receiptVoucher != null)
                        {
                            var receiptVoucherDetail = receiptVoucher.ReceiptVoucherDetails.FirstOrDefault(o => o.OutContractServicePackageId == @event.OutContractServicePackageEvents[i].Id);
                            if (receiptVoucherDetail != null)
                            {
                                receiptVoucherDetail.IsActive = @event.IsActive;
                            }

                            receiptVoucher.IsActive = !receiptVoucher.ReceiptVoucherDetails.All(a => !a.IsActive);
                            receiptVoucher.CalculateTotal();
                            _receiptVoucherRepository.Update(receiptVoucher);
                        }
                    }
                }
            }

            await _receiptVoucherRepository.SaveChangeAsync();

        }
    }
}
