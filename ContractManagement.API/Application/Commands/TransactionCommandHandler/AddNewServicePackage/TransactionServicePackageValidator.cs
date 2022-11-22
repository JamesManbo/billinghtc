using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.AddNewServicePackage
{
    public class TransactionServicePackageValidator : AbstractValidator<CUTransactionServicePackageCommand>
    {
        public TransactionServicePackageValidator()
        {
            RuleFor(c => c.ServiceId).GreaterThan(0).WithMessage("Dịch vụ là bắt buộc");
            RuleFor(c => c.TimeLine.PaymentPeriod).GreaterThan(0).WithMessage("Kỳ hạn là bắt buộc");
            RuleFor(c => c.RadiusAccount).MaximumLength(256).WithMessage("Tài khoản Radius tối đa 256 ký tự");
            RuleFor(c => c.RadiusPassword).MaximumLength(256).WithMessage("Mật khẩu Radius tối đa 256 ký tự");
        }
    }
}
