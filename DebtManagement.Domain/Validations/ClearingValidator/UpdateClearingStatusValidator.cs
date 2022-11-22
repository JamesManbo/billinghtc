using DebtManagement.Domain.Commands.ClearingCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Validations.ClearingValidator
{
    public class UpdateClearingStatusValidator : AbstractValidator<UpdateClearingStatusCommand>
    {
        public UpdateClearingStatusValidator()
        {
            RuleFor(r => r.Id).NotEmpty().WithMessage("Trạng thái là bắt buộc");
            RuleFor(r => r.StatusId).GreaterThan(0).WithMessage("Trạng thái là bắt buộc");
        }
    }
}
