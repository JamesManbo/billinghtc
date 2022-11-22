using ContractManagement.Domain.Commands.TransactionCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.CreateDeployNewOutContract
{
    public class CreateDeployNewOutContractCommandValidator : AbstractValidator<CreateDeployNewOutContractCommand>
    {
        public CreateDeployNewOutContractCommandValidator()
        {
            RuleFor(c => c.TransactionServicePackages).NotEmpty().WithMessage("Dịch vụ là bắt buộc");
            RuleForEach(x => x.TransactionServicePackages).SetValidator(new CUTransactionServicePackageCommandValidator());
        }
    }
}
