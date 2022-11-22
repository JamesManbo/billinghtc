using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Validations.ReceiptVoucherValidator
{
    public class BadReceiptVoucherValidator : AbstractValidator<BadReceiptVoucherCommand>
    {
        public BadReceiptVoucherValidator()
        {
            RuleFor(e => e.AttachmentFiles).NotNull().WithMessage("Tệp đính kèm là bắt buộc");
        }
    }
}
