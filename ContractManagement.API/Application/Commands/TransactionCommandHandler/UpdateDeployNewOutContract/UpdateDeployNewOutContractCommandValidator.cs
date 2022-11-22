using ContractManagement.Domain.Commands.TransactionCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.UpdateDeployNewOutContract
{
    public class UpdateDeployNewOutContractCommandValidator : AbstractValidator<UpdateDeployNewOutContractCommand>
    {
        public UpdateDeployNewOutContractCommandValidator()
        {
            RuleFor(c => c.Id).GreaterThan(0).WithMessage("Phụ lục là bắt buộc");
            RuleFor(c => c.TransactionServicePackages).NotNull().WithMessage("Dịch vụ là bắt buộc");
            When(c => c.TransactionEquipments.Any(), () =>
            {
                RuleForEach(x => x.TransactionEquipments).SetValidator(new CUTransactionEquipmentCommandValidator());
            });
        }
    }
}
