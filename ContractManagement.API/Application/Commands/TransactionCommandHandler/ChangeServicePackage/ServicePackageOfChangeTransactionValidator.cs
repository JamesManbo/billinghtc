using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.ChangeServicePackage
{
    public class ServicePackageOfChangeTransactionValidator : AbstractValidator<CUTransactionServicePackageCommand>
    {
        public ServicePackageOfChangeTransactionValidator()
        {
            When(u => u.IsOld == true, () =>
              {
                  RuleFor(x => x.ServiceId).GreaterThan(0).WithMessage("Dịch vụ cũ là bắt buộc");
              });


            When(u => u.IsOld != true, () =>
            {
                RuleFor(x => x.ServiceId).GreaterThan(0).WithMessage("Dịch vụ mới là bắt buộc");
            });

            RuleFor(c => c.TimeLine.PaymentPeriod).GreaterThan(0).WithMessage("Kỳ hạn là bắt buộc");
        }
    }
}
