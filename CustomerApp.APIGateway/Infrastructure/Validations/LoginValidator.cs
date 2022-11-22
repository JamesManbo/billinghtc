using CustomerApp.APIGateway.Models.RequestModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Infrastructure.Validations
{
    public class LoginValidator : AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Vui lòng nhập tài khoản.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Vui lòng nhập mật khẩu.");
        }
    }
}
