using ContractManagement.Domain.Commands.TransactionCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.ChangeServicePackage
{
    public class ChangeServicePackageTransactionValidator : AbstractValidator<CUChangeServicePackageTransaction>
    {
        public ChangeServicePackageTransactionValidator()
        {
            RuleFor(x => x.TransactionServicePackages.Where(x => x.IsOld == true))
                .NotEmpty().WithMessage("Vui lòng chọn kênh cần điều chỉnh gói cước");

            RuleFor(x => x.TransactionServicePackages.Where(x => x.IsOld != true))
                .NotEmpty().WithMessage("Vui lòng chọn gói cước mới muốn thay đổi");

            RuleForEach(x => x.TransactionServicePackages).SetValidator(new ServicePackageOfChangeTransactionValidator());
            
            RuleFor(c => c.TransactionDate).NotEmpty().WithMessage("Ngày tiếp nhận yêu cầu là bắt buộc");
        }
    }
}
