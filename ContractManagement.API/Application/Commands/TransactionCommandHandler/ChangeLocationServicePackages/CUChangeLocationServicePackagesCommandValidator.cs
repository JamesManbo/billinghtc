using ContractManagement.Domain.Commands.TransactionCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.ChangeLocationServicePackages
{
    public class CUChangeLocationServicePackagesCommandValidator : AbstractValidator<CUChangeLocationServicePackagesCommand>
    {
        public CUChangeLocationServicePackagesCommandValidator()
        {
            RuleFor(c => c.TransactionServicePackages)
                .NotEmpty().WithMessage("Vui lòng chọn kênh muốn thực hiện dịch chuyển địa điểm");
        }
    }
}
