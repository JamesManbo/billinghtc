using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Application.Commands.CustomerStructure
{
    public class CreateCustomerStructureValidator : AbstractValidator<CustomerStructureCommand>
    {
        public CreateCustomerStructureValidator()
        {
            RuleFor(structure => structure.Code).NotNull().WithMessage("Mã cơ cấu khách hàng là bắt buộc").NotEmpty().WithMessage("Mã cơ cấu khách hàng là bắt buộc");
            RuleFor(structure => structure.Name).NotNull().WithMessage("Tên cơ cấu khách hàng là bắt buộc").NotEmpty().WithMessage("Tên cơ cấu khách hàng là bắt buộc");
        }
    }
}
