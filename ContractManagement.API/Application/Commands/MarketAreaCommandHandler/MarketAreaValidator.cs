using ContractManagement.Domain.Commands.MarketAreaCommand;
using FluentValidation;

namespace ContractManagement.API.Application.Commands.MarketAreaCommandHandler
{
    public class MarketAreaValidator : AbstractValidator<CUMarketAreaCommand>
    {
        public MarketAreaValidator()
        {
            RuleFor(market => market.MarketName).NotEmpty().WithMessage("Tên vùng thị trường là bắt buộc");
            RuleFor(market => market.MarketCode).NotEmpty().WithMessage("Mã vùng thị trường là bắt buộc")
                .Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$").WithMessage("Mã chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");

        }
    }
}
