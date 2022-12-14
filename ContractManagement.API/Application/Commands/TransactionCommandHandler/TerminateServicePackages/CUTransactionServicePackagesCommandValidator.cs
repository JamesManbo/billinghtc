using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.TerminateServicePackages
{
    public class CUTransactionServicePackagesCommandValidator : AbstractValidator<CUTransactionServicePackageCommand>
    {
        public CUTransactionServicePackagesCommandValidator(bool multipleContract)
        {
            RuleFor(c => c.OutContractServicePackageId).GreaterThan(0).WithMessage("Dịch vụ gói cước hợp đồng đầu ra là bắt buộc");
            RuleFor(c => c.ServiceId).GreaterThan(0).WithMessage("Dịch vụ là bắt buộc");
            When(customer => multipleContract == false, () =>
            {
                RuleFor(c => c.ServicePackageId).GreaterThan(0).WithMessage("Gói cước là bắt buộc");
            });
        }
    }
}
