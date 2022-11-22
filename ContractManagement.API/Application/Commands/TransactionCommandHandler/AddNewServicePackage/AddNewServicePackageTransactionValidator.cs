using ContractManagement.Domain.Commands.TransactionCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.AddNewServicePackage
{
    public class AddNewServicePackageTransactionValidator : AbstractValidator<CUAddNewServicePackageTransaction>
    {
        public AddNewServicePackageTransactionValidator()
        {
            RuleFor(c => c.TransactionServicePackages).NotEmpty().WithMessage("Dịch vụ gói cước là bắt buộc");
            RuleForEach(x => x.TransactionServicePackages).SetValidator(new TransactionServicePackageValidator());
        }
    }
}
