using ContractManagement.Domain.Commands.CUCurrencyUnitCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.CurrencyUnitCommandHandler
{
    public class CreateCurrencyUnitValidator : AbstractValidator<CreateCurrencyUnitCommand>
    {
        public CreateCurrencyUnitValidator()
        {
            RuleFor(equipment => equipment.CurrencyUnitName).NotNull().WithMessage("Tên đơn vị tiền tệ là bắt buộc");
            RuleFor(equipment => equipment.CurrencyUnitName).MinimumLength(3).WithMessage("Tên đơn vị tiền tệ quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(equipment => equipment.CurrencyUnitName).MaximumLength(256).WithMessage("Tên đơn vị tiền tệ quá dài(tối đa 256 ký tự)");
            RuleFor(equipment => equipment.CurrencyUnitCode).NotNull().WithMessage("Mã đơn vị tiền tệ là bắt buộc");
            RuleFor(equipment => equipment.CurrencyUnitCode).Length(3).WithMessage("Mã đơn vị tiền tệ phải là 3 ký tự");
            RuleFor(equipment => equipment.CurrencyUnitCode).Matches("^[a-zA-Z]").WithMessage("Mã đơn vị tiền tệ chỉ được bao gồm các ký tự viết hoa");
        }
    }

    public class UpdateCurrencyUnitValidator : AbstractValidator<UpdateCurrencyUnitCommand>
    {
        public UpdateCurrencyUnitValidator()
        {
            RuleFor(equipment => equipment.CurrencyUnitName).NotNull().WithMessage("Tên đơn vị tiền tệ là bắt buộc");
            RuleFor(equipment => equipment.CurrencyUnitName).MinimumLength(3).WithMessage("Tên đơn vị tiền tệ quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(equipment => equipment.CurrencyUnitName).MaximumLength(256).WithMessage("Tên đơn vị tiền tệ quá dài(tối đa 256 ký tự)");
            RuleFor(equipment => equipment.CurrencyUnitCode).NotNull().WithMessage("Mã đơn vị tiền tệ là bắt buộc");
            RuleFor(equipment => equipment.CurrencyUnitCode).Length(3).WithMessage("Mã đơn vị tiền tệ phải là 3 ký tự)");
            RuleFor(equipment => equipment.CurrencyUnitCode).Matches("^[a-zA-Z]").WithMessage("Mã đơn vị tiền tệ chỉ được bao gồm các ký tự viết hoa");
        }
    }
}
