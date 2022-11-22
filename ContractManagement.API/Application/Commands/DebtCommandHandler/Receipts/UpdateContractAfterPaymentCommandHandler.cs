using AutoMapper;
using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using ContractManagement.Domain.Commands.DebtCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Infrastructure.Repositories.ContractServicePackageRepository;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using ContractManagement.Infrastructure.Repositories.PromotionRepository;
using ContractManagement.Utility;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.DebtCommandHandler
{
    public class UpdateContractAfterPaymentCommandHandler : IRequestHandler<UpdateContractAfterPaymentCommand, ActionResponse>
    {
        private readonly IContractSrvPckRepository _contractSrvPckRepository;
        private readonly IServicePackageSuspensionTimeRepository _servicePackageSuspensionTimeRepository;
        private readonly IMapper _mapper;
        private readonly IPromotionForContractRepository _promotionForContractRepository;
        public UpdateContractAfterPaymentCommandHandler(IContractSrvPckRepository contractSrvPckRepository,
            IServicePackageSuspensionTimeRepository servicePackageSuspensionTimeRepository, IMapper mapper,
            IPromotionForContractRepository promotionForContractRepository)
        {
            _contractSrvPckRepository = contractSrvPckRepository;
            _servicePackageSuspensionTimeRepository = servicePackageSuspensionTimeRepository;
            _mapper = mapper;
            _promotionForContractRepository = promotionForContractRepository;
        }

        public async Task<ActionResponse> Handle(UpdateContractAfterPaymentCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsActiveSPST)
            {
                for (int i = 0; i < request.VoucherDetails.Count; i++)
                {
                    var spstIds = request.VoucherDetails[i].SPSuspensionTimeIds.SplitToInt(',');
                    var spSuspensionTimes = await _servicePackageSuspensionTimeRepository.GetByIdsAsync(spstIds);
                    foreach (var item in spSuspensionTimes)
                    {
                        item.UpdatedDate = DateTime.Now;
                        item.IsActive = request.IsActiveSPST;
                        if (request.VoucherDetails[i].DiscountAmountSuspend > item.RemainingAmount)
                        {
                            request.VoucherDetails[i].DiscountAmountSuspend -= item.RemainingAmount;
                            item.RemainingAmount = 0;
                        }
                        else
                        {
                            item.RemainingAmount -= request.VoucherDetails[i].DiscountAmountSuspend;
                        }
                        _servicePackageSuspensionTimeRepository.Update(item);
                    }

                    foreach (var promo in request.Promotions)
                    {
                        var promoForContract = _mapper.Map<PromotionForContract>(promo);
                        await _promotionForContractRepository.CreateAndSave(promoForContract);

                        //tang them thang su dung
                        if (promo.PromotionValueType == 3 || promo.PromotionValueType == 4)
                        {
                            var ocspEntity = await _contractSrvPckRepository.GetByIdAsync(promo.OutContractServicePackageId);
                            ocspEntity.SetPromotionDate(promo.PromotionValueType, promo.PromotionValue);
                        }
                    }
                }

                await _servicePackageSuspensionTimeRepository.SaveChangeAsync();
            }

            return ActionResponse.Success;
        }
    }
}
