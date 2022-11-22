using ContractManagement.Domain.Commands.PromotionCommand;
using FluentValidation;

namespace ContractManagement.API.Application.Commands.ProjectCommandHandler
{
    public class PromotionValidator : AbstractValidator<CUPromotionCommand>
    {
        public PromotionValidator()
        {
            RuleFor(promo => promo.PromotionCode).NotEmpty().WithMessage("Mã khuyến mại là bắt buộc");
            RuleFor(promo => promo.PromotionName).NotEmpty().WithMessage("Tên khuyến mại là bắt buộc");
           
            RuleFor(promo => promo.StartDate).NotEmpty().WithMessage("Ngày bắt đầu là bắt buộc");
            RuleFor(promo => promo.EndDate).NotEmpty().WithMessage("Ngày kết thúc là bắt buộc");
            RuleFor(promo => promo.EndDate).GreaterThan(promo => promo.StartDate).WithMessage("Ngày bắt đầu và kêt thúc không hợp lệ.");

            RuleFor(promo => promo.PromotionType).NotEmpty().WithMessage("Loại khuyến mại là bắt buộc");

            RuleForEach(promo => promo.PromotionDetails).Must(p => p.Quantity > 0).WithMessage("Giá trị khuyến mại phải lớn hơn 0");

        }
    }
}
