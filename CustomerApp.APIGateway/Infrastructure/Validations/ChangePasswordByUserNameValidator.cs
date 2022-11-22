using CustomerApp.APIGateway.Models.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Infrastructure.Validations
{
    public class ChangePasswordByUserNameValidator : AbstractValidator<ChangePasswordByUserNameRequest>
    {
        public ChangePasswordByUserNameValidator()
        {
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Vui lòng nhập mật khẩu mới.");

            RuleFor(c => c.NewPassword)
                .NotEmpty().WithMessage("Vui lòng nhập mật khẩu mới.")
                .MinimumLength(6).WithMessage("Mật khẩu mới tối thiểu 6 ký tự")
                .Equal(s => s.ConfirmPassword).WithMessage("Nhập lại mật khẩu không khớp");
        }
    }
}
