using ContractManagement.Domain.Commands.ServicePackageCommand;
using FluentValidation;
using System;
using System.Linq;

namespace ContractManagement.API.Application.Commands.ServicePackageCommandHandler
{
    public class ServicePackageCommandValidator : AbstractValidator<CUServicePackageCommand>
    {
        public ServicePackageCommandValidator()
        {
            RuleFor(p => p.PackageName).NotEmpty().WithMessage("Tên gói cước là bắt buộc");
            RuleFor(p => p.PackageCode).NotEmpty().WithMessage("Mã gói cước là bắt buộc")
                .Matches("^[a-zA-Z0-9][a-zA-Z0-9_]+$").WithMessage("Mã chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)");
            RuleFor(p => p.ServiceId).NotEmpty().WithMessage("Dịch vụ là bắt buộc");
            RuleFor(p => p.DomesticBandwidth).NotEmpty().WithMessage("Băng thông trong nước là bắt buộc").GreaterThan(0).WithMessage("Băng thông trong nước phải lớn hơn 0");
            //RuleForEach(lst => lst.ListProjectPrice).Must(p => p.PriceValue > 0).WithMessage("Giá tiền theo dự án phải lớn hơn 0")
            //        .Must(p => p.StartDate < p.EndDate).WithMessage("Khoảng thời gian không hợp lệ");

            //When(p => p.ServicePackageRadiusServices != null && p.ServicePackageRadiusServices.Any(), () =>
            //{
            //    RuleForEach(e => e.ServicePackageRadiusServices)
            //    .Must(e => e != null && e.RadiusServerId >= 0)
            //    .WithMessage("Bạn chưa chọn dịch vụ tương ứng trên Hệ thống Radius {RadiusServerName}");
            //});
        }
    }
}
