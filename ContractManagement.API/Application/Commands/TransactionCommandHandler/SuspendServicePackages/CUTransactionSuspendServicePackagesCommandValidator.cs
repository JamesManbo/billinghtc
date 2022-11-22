using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.TransactionCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.SuspendServicePackages
{
    public class CUTransactionSuspendServicePackagesCommandValidator : AbstractValidator<CUTransactionSuspendServicePackagesCommand>
    {
        public CUTransactionSuspendServicePackagesCommandValidator()
        {
            RuleFor(c => c.TransactionServicePackages).NotEmpty().WithMessage("Vui lòng chọn kênh muốn tạm ngưng");
            RuleForEach(x => x.TransactionServicePackages).SetValidator(new CUTransactionServicePackagesCommandValidator());
            RuleFor(c => c.ReasonType).GreaterThan(0).WithMessage("Lý do tạm ngưng là bắt buộc");
            When(c => c.ReasonType == TransactionReason.SuspensionOtherReason.Id, () =>
            {
                RuleFor(c => c.Reason).NotEmpty().WithMessage("Lý do tạm ngưng là bắt buộc");
            });
        }
    }
}
