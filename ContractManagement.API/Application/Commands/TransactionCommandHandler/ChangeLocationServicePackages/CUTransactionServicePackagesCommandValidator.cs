using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.ChangeLocationServicePackages
{
    public class CUTransactionServicePackagesCommandValidator : AbstractValidator<CUTransactionServicePackageCommand>
    {
        public CUTransactionServicePackagesCommandValidator()
        {
            RuleFor(c => c.OutContractServicePackageId).GreaterThan(0).WithMessage("Dịch vụ gói cước hợp đồng đầu ra là bắt buộc");
            RuleFor(c => c.ServiceId).GreaterThan(0).WithMessage("Dịch vụ là bắt buộc");

            //TODO Outlet channel point logic changes
            //When(c => c.HasStartAndEndPoint == true, () =>
            //{
            //    RuleFor(c => c.InstallationAddress.StartPoint).NotEmpty().WithMessage("Địa chỉ cài đặt điểm đầu mới là bắt buộc");
            //    RuleFor(c => c.InstallationAddress.EndPoint).NotEmpty().WithMessage("Địa chỉ cài đặt điểm cuối mới là bắt buộc");
            //});
            //When(c => c.HasStartAndEndPoint != true, () =>
            //{
            //    RuleFor(c => c.InstallationAddress.Street).NotEmpty().WithMessage("Địa chỉ cài đặt mới là bắt buộc");
            //});
        }
    }
}
