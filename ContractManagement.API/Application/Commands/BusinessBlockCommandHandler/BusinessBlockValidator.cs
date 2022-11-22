using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.BusinessBlockCommandHandler
{
    public class BusinessBlockValidator : AbstractValidator<BusinessBlockCommand>
    {
        public BusinessBlockValidator()
        {
            RuleFor(block => block.BusinessBlockName).NotEmpty().WithMessage("Tên khối kinh doanh quản lý là bắt buộc");
            RuleFor(block => block.BusinessBlockName).MinimumLength(3).WithMessage("Tên khối kinh doanh quản lý quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(block => block.BusinessBlockName).MaximumLength(256).WithMessage("Tên khối kinh doanh quản lý quá dài(tối đa 256 ký tự)");
        }
    }
}
