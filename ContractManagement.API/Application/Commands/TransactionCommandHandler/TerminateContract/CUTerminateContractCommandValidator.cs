using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Commands.TransactionCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.TerminateContract
{
    public class CUTerminateContractCommandValidator : AbstractValidator<CUTerminateContractCommand>
    {
        public CUTerminateContractCommandValidator()
        {
            RuleFor(c => c.Type).NotEmpty().WithMessage("Loại phụ lục là bắt buộc");
            RuleFor(c => c.StatusId).NotEmpty().WithMessage("Trạng thái phụ lục là bắt buộc");
            RuleFor(c => c.ReasonType)
                .NotNull().WithMessage("Loại lý do hủy là bắt buộc")
                .GreaterThan(0).WithMessage("Loại lý do hủy là bắt buộc");

            When(c => c.ReasonType == TransactionReason.TerminateOtherReason.Id, () =>
            {
                RuleFor(c => c.Reason).NotEmpty().WithMessage("Lý do hủy là bắt buộc");
            });
        }
    }
}
