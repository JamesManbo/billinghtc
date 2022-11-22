using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.RestoreServicePackages
{
    public class CUTransactionServicePackagesCommandValidator : AbstractValidator<CUTransactionServicePackageCommand>
    {
        public CUTransactionServicePackagesCommandValidator()
        {
            RuleFor(c => c.OutContractServicePackageId).GreaterThan(0).WithMessage("Vui lòng chọn kênh muốn thực hiện");
            RuleFor(c => c.ServiceId).GreaterThan(0).WithMessage("Vui lòng chọn dịch vụ muốn thực hiện");
            RuleFor(c => c.ServicePackageId).GreaterThan(0).WithMessage("Vui lòng chọn gói cước muốn thực hiện");
        }
    }
}
