using ContractManagement.Domain.Commands.ServicePackagePriceCommand;
using FluentValidation;
using System;

namespace ContractManagement.API.Application.Commands.ServicePackagePriceCommandHandler
{
    public class ServicePackagePriceValidator : AbstractValidator<CUServicePackagePriceCommand>
    {
        public ServicePackagePriceValidator()
        {
            RuleFor(c => c.ProjectId).NotNull().WithMessage("Vui lòng lựa chọn hoặc thêm mới dự án");

            //RuleFor(c => c.PriceValue).GreaterThan(0).WithMessage("Giá tiền phải lớn hơn 0")
            //    .Must(BeMoney).WithMessage("Giá trị tiền không hợp lệ");

            RuleFor(c => c.StartDate).NotNull().NotEmpty().WithMessage("Ngày bắt đầu là bắt buộc");

            RuleFor(c => c.EndDate).NotNull().NotEmpty().WithMessage("Ngày kết thúc là bắt buộc");

            RuleFor(c => c.StartDate).LessThan(c => c.EndDate).WithMessage("Ngày bắt đầu phải nhỏ hơn ngày kết thúc");

            RuleFor(c => c.EndDate).GreaterThan(c => c.StartDate).WithMessage("Ngày kết thúc phải lớn hơn ngày bắt đầu");

        }

        private bool BeMoney(decimal arg)
        {
            if (arg % 100 == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
