using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.UnitOfMeasurementCommand
{
    public class UnitOfMeasurementValidator : AbstractValidator<CUUnitOfMeasurementCommand>
    {
        public UnitOfMeasurementValidator()
        {
            RuleFor(unit => unit.Description).NotEmpty().WithMessage("Tên đơn vị đo lường là bắt buộc");
            RuleFor(unit => unit.Description).MinimumLength(3).WithMessage("Tên đơn vị đo lường quá ngắn(tối thiểu 3 ký tự)");
            RuleFor(unit => unit.Description).MaximumLength(256).WithMessage("Tên đơn vị đo lường quá dài(tối đa 256 ký tự)");
            RuleFor(unit => unit.Label).NotEmpty().WithMessage("Ký hiệu đơn vị đo lường là bắt buộc");
            RuleFor(unit => unit.Label).MaximumLength(256).WithMessage("Ký hiệu đơn vị đo lường quá dài(tối đa 256 ký tự)");
            //RuleFor(unit => unit.Type).GreaterThan(0).WithMessage("Loại đơn vị đo lường là bắt buộc");
        }

    }
}
