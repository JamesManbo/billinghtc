using DebtManagement.Domain.Commands.ClearingCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebtManagement.API.Application.Commands.ClearingCommandHandler
{
    public class RejectClearingCommandValidator : AbstractValidator<RejectClearingVoucherCommand>
    {
        public RejectClearingCommandValidator()
        {
            RuleFor(c => c.Reason)
                .NotNull().WithMessage("Lý do hủy là bắt buộc")
                .NotEmpty().WithMessage("Lý do hủy là bắt buộc");
        }
    }
}
