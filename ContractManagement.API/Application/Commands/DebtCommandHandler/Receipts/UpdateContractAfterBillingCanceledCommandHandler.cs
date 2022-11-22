using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using ContractManagement.Domain.Commands.DebtCommand;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ContractServicePackageRepository;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using ContractManagement.Utility;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.DebtCommandHandler
{
    public class UpdateContractAfterBillingCanceledCommandHandler : IRequestHandler<UpdateContractAfterBillingCanceledCommand, ActionResponse>
    {
        private readonly IServicePackageSuspensionTimeRepository _servicePackageSuspensionTimeRepository;
        private readonly IContractSrvPckRepository _contractSrvPckRepository;

        public UpdateContractAfterBillingCanceledCommandHandler(IServicePackageSuspensionTimeRepository servicePackageSuspensionTimeRepository,
            IContractSrvPckRepository contractSrvPckRepository)
        {
            _servicePackageSuspensionTimeRepository = servicePackageSuspensionTimeRepository;
            _contractSrvPckRepository = contractSrvPckRepository;
        }

        public async Task<ActionResponse> Handle(UpdateContractAfterBillingCanceledCommand request, CancellationToken cancellationToken)
        {
            var firstBillingChannels = new List<int>();
            var canceledBillingTime = new List<(int, DateTime, DateTime)>();
            for (int i = 0; i < request.VoucherDetails.Count; i++)
            {
                var voucherDetail = request.VoucherDetails[i];
                if (!request.IsActiveSPST)
                {
                    var spstIds = voucherDetail.SPSuspensionTimeIds.SplitToInt(',');
                    var spSuspensionTimes = await _servicePackageSuspensionTimeRepository.GetByIdsAsync(spstIds);
                    foreach (var item in spSuspensionTimes)
                    {
                        item.UpdatedDate = DateTime.Now;
                        item.IsActive = request.IsActiveSPST;
                        _servicePackageSuspensionTimeRepository.Update(item);
                    }
                }

                if (voucherDetail.IsFirstDetailOfService)
                {
                    firstBillingChannels.Add(voucherDetail.OutContractServicePackageId);
                }

                if (voucherDetail.StartBillingDate.HasValue
                    && voucherDetail.EndBillingDate.HasValue)
                {
                    if (request.Promotions != null && request.Promotions.Count > 0)
                    {
                        var usingMonthPromotion = request.Promotions
                            .Where(c => c.OutContractServicePackageId == voucherDetail.OutContractServicePackageId
                                && c.PromotionType == PromotionType.GiveAwayTimeToUse.Id);
                        foreach (var promotionDetail in usingMonthPromotion)
                        {
                            if (promotionDetail.PromotionValueType == PromotionValueType.UseTimeDay.Id)
                            {
                                voucherDetail.EndBillingDate = voucherDetail.EndBillingDate.Value.AddDays((int)promotionDetail.PromotionValue);
                            }

                            if (promotionDetail.PromotionValueType == PromotionValueType.UseTimeMonth.Id)
                            {
                                voucherDetail.EndBillingDate = voucherDetail.EndBillingDate.Value.AddMonths((int)promotionDetail.PromotionValue);
                            }
                        }
                    }

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
                await _contractSrvPckRepository.UpdateFirstBillingIsFailed(firstBillingChannels.ToArray());
            }

            await _contractSrvPckRepository.SaveChangeAsync();

            return ActionResponse.Success;
        }
    }
}
