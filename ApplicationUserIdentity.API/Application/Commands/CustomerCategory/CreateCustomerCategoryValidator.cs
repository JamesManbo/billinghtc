using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Application.Commands.CustomerCategory
{
    public class CreateCustomerCategoryValidator : AbstractValidator<CustomerCategoryCommand>
    {
        public CreateCustomerCategoryValidator()
        {
            RuleFor(category => category.Code).NotNull().WithMessage("Mã danh mục khách hàng là bắt buộc").NotEmpty().WithMessage("Mã danh mục khách hàng là bắt buộc");
            RuleFor(category => category.Name).NotNull().WithMessage("Tên danh mục khách hàng là bắt buộc").NotEmpty().WithMessage("Tên danh mục khách hàng là bắt buộc");
        }
    }
}
