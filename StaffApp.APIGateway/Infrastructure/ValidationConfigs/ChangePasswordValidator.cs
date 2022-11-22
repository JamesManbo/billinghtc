using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using StaffApp.APIGateway.Models;

namespace StaffApp.APIGateway.Infrastructure.ValidationConfigs
{
    public class ChangePasswordValidator : AbstractValidator<DraftChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.OldPassword)
    .NotEmpty().WithMessage("Vui lòng nhập mật khẩu cũ.");
                
            RuleFor(c => c.NewPassword)
                .NotEmpty().WithMessage("Vui lòng nhập mật khẩu mới.")
                .MinimumLength(6).WithMessage("Mật khẩu mới tối thiểu 6 ký tự")
                .Equal(s=>s.ConfirmPassword).WithMessage("Nhập lại mật khẩu không khớp");
        }
    }
}
