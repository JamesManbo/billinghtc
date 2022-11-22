using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Application.Commands.UserGroup
{
    public class CreateUserGroupValidator : AbstractValidator<CUUserGroupCommand>
    {
        public CreateUserGroupValidator()
        {
            RuleFor(group => group.GroupName).NotNull().WithMessage("Tên nhóm là bắt buộc").NotEmpty().WithMessage("Tên nhóm là bắt buộc");
            RuleFor(group => group.GroupCode).NotNull().WithMessage("Mã nhóm là bắt buộc").NotEmpty().WithMessage("Mã nhóm là bắt buộc")
                .Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$").WithMessage("Mã chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");
        }
        
    }
}
