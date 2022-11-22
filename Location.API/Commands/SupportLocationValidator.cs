using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Location.API.Commands
{
    public class SupportLocationValidator : AbstractValidator<CUSupportLocationCommand>
    {
        public SupportLocationValidator()
        {
            RuleFor(sl => sl.Name).NotNull().WithMessage("Tên địa điểm là bắt buộc").NotEmpty().WithMessage("Tên địa điểm là bắt buộc ");
            RuleFor(sl => sl.Phone).NotNull().WithMessage("Số điện thoại là bắt buộc").NotEmpty().WithMessage("Số điện thoại là bắt buộc ");
            RuleFor(sl => sl.Address).NotNull().WithMessage("Địa chỉ là bắt buộc").NotEmpty().WithMessage("Địa chỉ là bắt buộc ");
            //RuleFor(sl => sl.Name).MaximumLength(256).WithMessage("Tên quá dài (tối đa 256 ký tự)");
            //RuleFor(sl => sl.Code).NotNull().WithMessage("Mã là bắt buộc").NotEmpty().WithMessage("Mã là bắt buộc");
            //RuleFor(sl => sl.Code).MaximumLength(256).WithMessage("Mã đơn vị quá dài (tối đa 256 ký tự)");
            //RuleFor(sl => sl.Code).Matches("^[a-zA-Z0-9][a-zA-Z0-9-]+$").WithMessage("Mã đơn vị chỉ được bao gồm số, ký tự không dấu và ký tự gạch(-)");
        }
    }
}
