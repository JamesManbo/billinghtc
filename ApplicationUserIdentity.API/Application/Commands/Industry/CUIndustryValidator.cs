using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Application.Commands.Industry
{
    public class CUIndustryValidator : AbstractValidator<CUIndustryCommand>
    {
        public CUIndustryValidator()
    {
        RuleFor(industry => industry.Name).NotNull().WithMessage("Tên lĩnh vực là bắt buộc").NotEmpty().WithMessage("Tên lĩnh vực là bắt buộc");
        RuleFor(industry => industry.Code).NotNull().WithMessage("Mã lĩnh vực là bắt buộc").NotEmpty().WithMessage("Mã lĩnh vực là bắt buộc")
            .Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$").WithMessage("Mã chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");
    }
}
}
