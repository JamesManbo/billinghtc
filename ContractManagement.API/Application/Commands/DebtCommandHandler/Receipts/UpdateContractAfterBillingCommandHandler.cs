using AutoMapper;
using ContractManagement.API.Application.IntegrationEvents.EventHandling.DebtEventHandlers;
using ContractManagement.Domain.AggregatesModel.ContractorAggregate;
using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using ContractManagement.Domain.Commands.DebtCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Infrastructure.Repositories.ContractServicePackageRepository;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using ContractManagement.Infrastructure.Repositories.PromotionRepository;
using ContractManagement.Utility;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.DebtCommandHandler
{
    public class UpdateContractAfterBillingCommandHandler : IRequestHandler<UpdateContractAfterBillingCommand, ActionResponse>
    {
        private readonly ILogger<BillingPaymentPendingIntegrationEventHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IContractSrvPckRepository _contractSrvPckRepository;
        private readonly IServicePackageSuspensionTimeRepository _servicePackageSuspensionTimeRepository;
        private readonly IPromotionForContractRepository _promotionForContractRepository;


        public UpdateContractAfterBillingCommandHandler(ILogger<BillingPaymentPendingIntegrationEventHandler> logger,
            IMapper mapper,
            IServicePackageSuspensionTimeRepository servicePackageSuspensionTimeRepository,
            IContractSrvPckRepository contractSrvPckRepository,
            IPromotionForContractRepository promotionForContractRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _servicePackageSuspensionTimeRepository = servicePackageSuspensionTimeRepository;
            _contractSrvPckRepository = contractSrvPckRepository;
            _promotionForContractRepository = promotionForContractRepository;
        }

        public async Task<ActionResponse> Handle(UpdateContractAfterBillingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.VoucherDetails == null || request.VoucherDetails.Count == 0)
                    return ActionResponse.Success;

                foreach (var rcptVoucherDetail in request.VoucherDetails)
                {
                    if (request.IsActiveSPST)
                    {
                        var spstIds = rcptVoucherDetail.SPSuspensionTimeIds.SplitToInt(',');
                        var spSuspensionTimes = await _servicePackageSuspensionTimeRepository.GetByIdsAsync(spstIds);
                        foreach (var suspendTimeItem in spSuspensionTimes)
                        {
                            suspendTimeItem.UpdatedDate = DateTime.Now;
                            suspendTimeItem.IsActive = request.IsActiveSPST;
                            var updatedSrvPckSuspendTimeStatus =
                                _servicePackageSuspensionTimeRepository.Update(suspendTimeItem);

                            if (!updatedSrvPckSuspendTimeStatus.IsSuccess)
                            {
                                throw new ContractDomainException(updatedSrvPckSuspendTimeStatus.Message);
                            }
                        }
                    }

                    var channel = await _contractSrvPckRepository.GetByIdAsync(rcptVoucherDetail.OutContractServicePackageId);

                    /// Cập nhật ngày tính cước tiếp theo của kênh được thanh toán
                    if (rcptVoucherDetail.EndBillingDate.HasValue)
                    {
                        DateTime channelNextBilling;
                        if (channel.TimeLine.PaymentForm == (int)PaymentMethodForm.Prepaid)
                        {
                            channelNextBilling = rcptVoucherDetail.EndBillingDate.Value.AddDays(1);
                        }
                        else
                        {
                            channelNextBilling = rcptVoucherDetail.EndBillingDate.Value
                                .AddDays(1).AddMonths(channel.TimeLine.PaymentPeriod);
                        }

                        if (request.Promotions != null && request.Promotions.Count > 0)
                        {
                            var usingMonthPromotion = request.Promotions.Where(c => c.OutContractServicePackageId == channel.Id
                            && c.PromotionType == PromotionType.GiveAwayTimeToUse.Id);
                            foreach (var promotionDetail in usingMonthPromotion)
                            {
                                if(promotionDetail.PromotionValueType == PromotionValueType.UseTimeDay.Id)
                                {
                                    channelNextBilling.AddDays((int)promotionDetail.PromotionValue);
                                }

                                if(promotionDetail.PromotionValueType == PromotionValueType.UseTimeMonth.Id)
                                {
                                    channelNextBilling.AddMonths((int)promotionDetail.PromotionValue);
                                }
                            }
                        }

                        channel.SetNextBillingDate(channelNextBilling);
                    }

                    /// Đánh dấu là kênh đang được thực hiện thanh toán lần đầu
                    if (channel.IsInFirstBilling)
                    {
                        channel.SetIsPaidTheFirstBilling();
                    }

                    var updatedResponse = _contractSrvPckRepository.Update(channel);
                    if (!updatedResponse.IsSuccess)
                    {
                        throw new ContractDomainException(updatedResponse.Message);
                    }
                }

                //foreach (var promo in request.Promotions)
                //{
                //    var promoForContract = _mapper.Map<PromotionForContract>(promo);
                //    await _promotionForContractRepository.CreateAndSave(promoForContract);

                //    //tang them thang su dung
                //    if (promo.PromotionValueType == 3 || promo.PromotionValueType == 4)
                //    {
                //        var ocspEntity = await _contractSrvPckRepository.GetByIdAsync(promo.OutContractServicePackageId);
                //        ocspEntity.SetPromotionDate(promo.PromotionValueType, promo.PromotionValue);
                //    }
                //}
                await _servicePackageSuspensionTimeRepository.SaveChangeAsync();
                return ActionResponse.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError("---- Error handling BillingPaymentPendingIntegrationEvent {Exception}", ex);
                throw;
            }
        }
    }
}
