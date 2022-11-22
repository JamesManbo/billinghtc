using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.TransactionCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.TerminateServicePackages
{
    public class CUTransactionTerminateServicePackagesCommandValidator : AbstractValidator<CUTransactionTerminateServicePackagesCommand>
    {
        public CUTransactionTerminateServicePackagesCommandValidator()
        {
            RuleFor(c => c.TransactionServicePackages).NotEmpty().WithMessage("Vui lòng chọn kênh muốn hủy dịch vụ");
            RuleFor(c => c.ReasonType).GreaterThan(0).WithMessage("Lý do hủy là bắt buộc");
            When(c => c.ReasonType == TransactionReason.TerminateOtherReason.Id, () =>
            {
                RuleFor(c => c.Reason).NotEmpty().WithMessage("Lý do hủy là bắt buộc");
            });
        }
    }
}
