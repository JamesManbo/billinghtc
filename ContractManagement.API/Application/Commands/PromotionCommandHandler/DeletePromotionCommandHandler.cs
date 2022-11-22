using ContractManagement.Domain.AggregatesModel.PromotionAggregate;
using ContractManagement.Domain.Commands.PromotionCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.PromotionRepository;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.PromotionCommandHandler
{
    public class DeletePromotionCommandHandler : IRequestHandler<DeletePromotionCommand, ActionResponse>
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IPromotionDetailRepository _promotionDetailRepository;

        public DeletePromotionCommandHandler(IPromotionRepository promotionRepository,
            IPromotionDetailRepository promotionDetailRepository)
        {
            _promotionRepository = promotionRepository;
            _promotionDetailRepository = promotionDetailRepository;
        }

        public async Task<ActionResponse> Handle(DeletePromotionCommand request, CancellationToken cancellationToken)
        {
            if (request.Id > 0)
            {
                var promotion = await _promotionRepository.GetByIdAsync(request.Id);
                if (promotion != null)
                {
                    promotion.IsDeleted = true;
                    promotion.IsActive = false;
                    promotion.UpdatedDate = DateTime.UtcNow.AddHours(7);
                    promotion.UpdatedBy = request.UpdatedBy;

                    var promotionDetails = await _promotionDetailRepository.GetByPromotionId(promotion.Id);
                    foreach (var promoDetail in promotionDetails)
                    {
                        promoDetail.IsDeleted = true;
                        promoDetail.IsActive = false;
                        promoDetail.UpdatedDate = DateTime.UtcNow.AddHours(7);
                        promoDetail.UpdatedBy = request.UpdatedBy;
                    }

                    await this._promotionRepository.SaveChangeAsync();
                }
            }

            return ActionResponse.Success;
        }
    }
}
