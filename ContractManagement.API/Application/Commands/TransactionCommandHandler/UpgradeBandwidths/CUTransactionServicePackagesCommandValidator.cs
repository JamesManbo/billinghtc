using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.UpgradeBandwidths
{
    public class CUTransactionServicePackagesCommandValidator : AbstractValidator<CUTransactionServicePackageCommand>
    {
        public CUTransactionServicePackagesCommandValidator()
        {
            RuleFor(c => c.ServiceId).GreaterThan(0).WithMessage("Dịch vụ là bắt buộc");
            RuleFor(c => c.ServicePackageId).GreaterThan(0).WithMessage("Gói cước là bắt buộc");
            When(c => c.IsOld != true, () =>
            {
                RuleFor(c => c.PackagePrice).GreaterThan(0).WithMessage("Giá tiền gói cước là bắt buộc");
            });
        }
    }
}
