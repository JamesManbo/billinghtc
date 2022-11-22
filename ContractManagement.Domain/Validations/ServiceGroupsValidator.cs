
using ContractManagement.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Validations
{
    public class ServiceGroupsValidator : AbstractValidator<ServiceGroupDTO>
    {
        public ServiceGroupsValidator()
        {
            RuleFor(r => r.GroupName).NotEmpty().WithMessage("Tên nhóm dịch vụ là bắt buộc");
            RuleFor(r => r.GroupCode)
                .NotEmpty().WithMessage("Mã nhóm dịch vụ là bắt buộc")
                .Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$").WithMessage("Mã chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");

        }
    }
}
