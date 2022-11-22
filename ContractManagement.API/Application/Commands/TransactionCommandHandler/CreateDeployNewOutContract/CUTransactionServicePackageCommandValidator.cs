using ContractManagement.Domain.AggregatesModel.ServicePackages;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.CreateDeployNewOutContract
{
    public class CUTransactionServicePackageCommandValidator : AbstractValidator<CUTransactionServicePackageCommand>
    {
        public CUTransactionServicePackageCommandValidator()
        {
            RuleFor(c => c.ServiceId).GreaterThan(0).WithMessage("Dịch vụ là bắt buộc");
            RuleFor(c => c.TimeLine.PaymentPeriod).GreaterThan(0).WithMessage("Kỳ hạn là bắt buộc");
        }
    }
}
