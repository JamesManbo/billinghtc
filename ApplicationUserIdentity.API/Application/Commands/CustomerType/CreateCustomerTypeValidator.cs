using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Application.Commands.CustomerType
{
    public class CreateCustomerTypeValidator : AbstractValidator<CustomerTypeCommand>
    {
        public CreateCustomerTypeValidator()
        {
            RuleFor(type => type.Code).NotNull().WithMessage("Mã kiểu khách hàng là bắt buộc").NotEmpty().WithMessage("Mã kiểu khách hàng là bắt buộc");
            RuleFor(type => type.Name).NotNull().WithMessage("Tên kiểu khách hàng là bắt buộc").NotEmpty().WithMessage("Tên kiểu khách hàng là bắt buộc");
        }
    }
}
