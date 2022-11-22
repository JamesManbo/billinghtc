
using ContractManagement.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Validations
{
    public class ServicePackagePriceValidator : AbstractValidator<ServicePackagePriceDTO>
    {
        public ServicePackagePriceValidator()
        {
            RuleFor(r => r.ChannelId).NotEmpty().WithMessage("Gói cước là bắt buộc");
            RuleFor(r => r.PriceValue).NotEmpty().WithMessage("Giá là bắt buộc");

        }
    }
}
