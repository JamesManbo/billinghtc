using CustomerApp.APIGateway.Models.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Infrastructure.Validations
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty().WithMessage("Vui lòng nhập mật khẩu cũ.");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Vui lòng nhập mật khẩu mới.");
        }
    }
}
