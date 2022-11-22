using ContractManagement.Domain.Commands.TransactionCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.UpgradeBandwidths
{
    public class CUUpgradeBandwidthsCommandValidator : AbstractValidator<CUUpgradeBandwidthsCommand>
    {
        public CUUpgradeBandwidthsCommandValidator()
        {
            RuleFor(c => c.TransactionServicePackages).NotEmpty().WithMessage("Dịch vụ gói cước là bắt buộc");
            RuleForEach(x => x.TransactionServicePackages).SetValidator(new CUTransactionServicePackagesCommandValidator());
            RuleFor(c => c.Type).NotEmpty().WithMessage("Loại phụ lục là bắt buộc");
            RuleFor(c => c.StatusId).NotEmpty().WithMessage("Trạng thái phụ lục là bắt buộc");
            RuleFor(c => c.TransactionDate).NotEmpty().WithMessage("Ngày tiếp nhận yêu cầu là bắt buộc");
        }
    }
}
