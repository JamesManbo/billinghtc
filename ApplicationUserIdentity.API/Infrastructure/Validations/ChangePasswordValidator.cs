using ApplicationUserIdentity.API.Models.AccountViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Validations
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordViewModel>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Vui lòng nhập mật khẩu cũ");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Vui lòng nhập mật khẩu mới");
            RuleFor(x => x.NewPassword)
                .MinimumLength(6).WithMessage("Mật khẩu tối thiểu 6 ký tự");
            RuleFor(x => x.ConfirmNewPassword)
                .NotEmpty().WithMessage("Vui lòng nhập lại mật khẩu mới");
            RuleFor(x => x.ConfirmNewPassword).Equal(x => x.NewPassword).WithMessage("Nhập lại mật khẩu không khớp");
        }
    }
}
