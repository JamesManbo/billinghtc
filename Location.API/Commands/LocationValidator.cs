using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Location.API.Commands
{
    public class CULocationValidator : AbstractValidator<CULocationCommand>
    {
        public CULocationValidator()
        {
            RuleFor(organizationUnit => organizationUnit.Name)
                .NotNull().WithMessage("Tên đơn vị là bắt buộc")
                .NotEmpty().WithMessage("Tên đơn vị là bắt buộc");

            RuleFor(organizationUnit => organizationUnit.Name)
                .MaximumLength(256).WithMessage("Tên đơn vị quá dài (tối đa 256 ký tự)");

            RuleFor(organizationUnit => organizationUnit.LocationId)
                .NotNull().WithMessage("Mã đơn vị hành chính là bắt buộc")
                .NotEmpty().WithMessage("Mã đơn vị hành chính là bắt buộc");

            RuleFor(organizationUnit => organizationUnit.Code)
                .NotNull().WithMessage("Tên viết tắt đơn vị hành chính là bắt buộc")
                .NotEmpty().WithMessage("Tên viết tắt đơn vị hành chính là bắt buộc");

            RuleFor(organizationUnit => organizationUnit.Code)
                .MaximumLength(256).WithMessage("Tên viết tắt quá dài (tối đa 256 ký tự)");

            RuleFor(organizationUnit => organizationUnit.Code)
                .Matches("^[a-zA-Z0-9][a-zA-Z0-9-]+$").WithMessage("Tên viết tắt chỉ được bao gồm số, ký tự không dấu và ký tự gạch(-)");

        }
    }
}
