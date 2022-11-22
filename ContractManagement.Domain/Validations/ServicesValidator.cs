
using ContractManagement.Domain.Commands.ServiceCommand;
using ContractManagement.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Validations
{
    public class ServicesValidator : AbstractValidator<ServiceCommand>
    {
        public ServicesValidator()
        {
            RuleFor(r => r.ServiceName).NotEmpty().WithMessage("Tên dịch vụ là bắt buộc");
            RuleFor(r => r.ServiceCode)
                .NotEmpty().WithMessage("Mã dịch vụ là bắt buộc")
                .Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$").WithMessage("Mã chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");
            RuleFor(r => r.GroupId).NotEmpty().WithMessage("Nhóm dịch vụ là bắt buộc");

        }
    }
}
