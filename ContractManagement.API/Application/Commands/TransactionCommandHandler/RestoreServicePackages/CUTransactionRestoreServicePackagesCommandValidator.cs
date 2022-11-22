using ContractManagement.API.Application.Commands.TransactionCommandHandler.AddNewServicePackage;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.TransactionCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.RestoreServicePackages
{
    public class CUTransactionRestoreServicePackagesCommandValidator : AbstractValidator<CUTransactionRestoreServicePackagesCommand>
    {
        public CUTransactionRestoreServicePackagesCommandValidator()
        {
            RuleFor(c => c.TransactionServicePackages).NotEmpty().WithMessage("Vui lòng chọn kênh muốn khôi phục");
            RuleForEach(x => x.TransactionServicePackages).SetValidator(new CUTransactionServicePackagesCommandValidator());
        }
    }
}
