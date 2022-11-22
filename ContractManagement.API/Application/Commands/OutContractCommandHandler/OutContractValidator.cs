using System.Data;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Commands.OutContractCommand;
using FluentValidation;

namespace ContractManagement.API.Application.Commands.OutContractCommandHandler
{
    public class OutContractValidator : AbstractValidator<CreateContractCommand>
    {
        public OutContractValidator()
        {
            //RuleFor(c => c.Contractor)
            //    .NotNull().WithMessage("Vui lòng lựa chọn hoặc thêm mới khách hàng vào hợp đồng")
            //    .Must(e => !string.IsNullOrWhiteSpace(e.IdentityGuid))
            //    .WithMessage("Vui lòng lựa chọn hoặc thêm mới khách hàng vào hợp đồng");

            RuleFor(c => c.ContractCode)
                .NotEmpty().WithMessage("Số hợp đồng là bắt buộc")
                .MinimumLength(6).WithMessage("Số hợp đồng quá ngắn");

            //RuleFor(c => c.AgentCode)
            //    .NotEmpty().WithMessage("Mã đại lý là bắt buộc");

            RuleFor(c => c.MarketAreaId)
                .GreaterThan(0).WithMessage("Vùng thị trường là bắt buộc");

            RuleFor(c => c.OrganizationUnitId)
                .NotEmpty().WithMessage("Phòng ban phụ trách là bắt buộc");
            RuleFor(c => c.SignedUserId)
                .NotEmpty().WithMessage("Nhân viên ký hợp đồng là bắt buộc");

            //RuleFor(c => c.CurrencyUnitId)
            //    .NotEmpty().WithMessage("Đơn vị tiền tệ là bắt buộc");

            RuleFor(c => c.ServicePackages)
                .NotEmpty().WithMessage("Vui lòng thêm dịch vụ và gói cước sử dụng");

            RuleFor(c => c.Payment != null ? c.Payment.Address : string.Empty)
                .MaximumLength(1000).WithMessage("Địa chỉ thanh toán quá dài(tối đa 1000 ký tự)");

            RuleFor(c => c.FiberNodeInfo)
                .MaximumLength(1000).WithMessage("Thông tin node quang quá dài(tối đa 1000 ký tự)");

            RuleFor(c => c.ContractNote)
                .MaximumLength(1000).WithMessage("Ghi chú quá dài(tối đa 1000 ký tự)");

            RuleFor(c => c.NumberBillingLimitDays)
                .NotNull().WithMessage("Thời hạn thanh toán là bắt buộc");

            RuleFor(c => c.NumberBillingLimitDays)
                .GreaterThanOrEqualTo(1).WithMessage("Thời hạn thanh toán tối thiểu là 1");

            When(c => c.ContractStatusId == 3, () =>
            {
                RuleForEach(c => c.ServicePackages)
                    .SetValidator(new RequireEffectiveDateOcSrvPackageValidator());
            });

            RuleForEach(c => c.ServicePackages)
                .SetValidator(new CUContractServicePackageValidator());
        }
    }

    public class UpdateOutContractValidator : AbstractValidator<UpdateContractCommand>
    {
        public UpdateOutContractValidator()
        {
            RuleFor(c => c.Contractor)
                .NotNull().WithMessage("Vui lòng lựa chọn hoặc thêm mới khách hàng vào hợp đồng")
                .Must(e => !string.IsNullOrWhiteSpace(e.IdentityGuid))
                .WithMessage("Vui lòng lựa chọn hoặc thêm mới khách hàng vào hợp đồng");

            RuleFor(c => c.ContractCode)
                .NotEmpty().WithMessage("Số hợp đồng là bắt buộc")
                .MinimumLength(6).WithMessage("Số hợp đồng quá ngắn");

            //RuleFor(c => c.AgentCode)
            //    .NotEmpty().WithMessage("Mã đại lý là bắt buộc");

            RuleFor(c => c.MarketAreaId)
                .GreaterThan(0).WithMessage("Vùng thị trường là bắt buộc");

            RuleFor(c => c.OrganizationUnitId)
                .NotEmpty().WithMessage("Phòng ban phụ trách là bắt buộc");

            RuleFor(c => c.CurrencyUnitId)
                .NotEmpty().WithMessage("Đơn vị tiền tệ là bắt buộc");

            RuleFor(c => c.ServicePackages)
                .NotEmpty().WithMessage("Vui lòng thêm dịch vụ và gói cước sử dụng");

            RuleFor(c => c.Payment != null ? c.Payment.Address : string.Empty)
                .MaximumLength(1000).WithMessage("Địa chỉ thanh toán quá dài(tối đa 1000 ký tự)");

            RuleFor(c => c.FiberNodeInfo)
                .MaximumLength(1000).WithMessage("Thông tin node quang quá dài(tối đa 1000 ký tự)");

            RuleFor(c => c.ContractNote)
                .MaximumLength(1000).WithMessage("Ghi chú quá dài(tối đa 1000 ký tự)");

            RuleFor(c => c.NumberBillingLimitDays)
                .NotNull().WithMessage("Thời hạn thanh toán là bắt buộc");

            RuleFor(c => c.NumberBillingLimitDays)
                .GreaterThanOrEqualTo(1).WithMessage("Thời hạn thanh toán tối thiểu là 1");

            When(c => c.ContractStatusId == 3, () =>
            {
                RuleForEach(c => c.ServicePackages)
                    .SetValidator(new RequireEffectiveDateOcSrvPackageValidator());
            });

            RuleForEach(c => c.ServicePackages)
                .SetValidator(new CUContractServicePackageValidator());
        }
    }

    public class RequireEffectiveDateOcSrvPackageValidator : AbstractValidator<CUOutContractChannelCommand>
    {
        public RequireEffectiveDateOcSrvPackageValidator()
        {
            RuleFor(x => x.TimeLine.Effective).NotNull()
                .WithMessage(s =>
                    $"Hợp đồng với trạng thái \"Đã nghiệm thu\" yêu cầu ngày nghiệm thu ở" +
                    $"{(!string.IsNullOrEmpty(s.CId) ? " kênh " + s.CId + "," : "")}" +
                    $" dịch vụ {s.ServiceName}, gói cước {s.PackageName}");
        }
    }

    public class CUContractServicePackageValidator : AbstractValidator<CUOutContractChannelCommand>
    {
        public CUContractServicePackageValidator()
        {

            RuleFor(c => c.ServiceId).GreaterThan(0).WithMessage("Dịch vụ là bắt buộc");
            RuleFor(c => c.TimeLine.PaymentPeriod).GreaterThan(0).WithMessage(m => $"Kỳ hạn thanh toán là bắt buộc ở dịch vụ {m.ServiceName}, gói cước {m.PackageName}");

            //When(o => o.HasStartAndEndPoint == true, () =>
            //{
            //    RuleFor(c => c.InstallationAddress).Must(c => !string.IsNullOrEmpty(c.StartPoint) || !string.IsNullOrEmpty(c.EndPoint))
            //    .WithMessage(m => $"Điểm đầu hoặc điểm cuối phải có giá trị ở dịch vụ {m.ServiceName}, gói cước {m.PackageName}");

            //    When(o => !string.IsNullOrEmpty(o.InstallationAddress.StartPoint), () =>
            //    {
            //        RuleFor(c => c.SpInstallationFee).GreaterThanOrEqualTo(0).WithMessage(m => $"Phí cài đặt điểm đầu là bắt buộc ở dịch vụ {m.ServiceName}, gói cước {m.PackageName}");
            //        RuleFor(c => c.SpPackagePrice).GreaterThanOrEqualTo(0).WithMessage(m => $"Cước hàng tháng điểm đầu là bắt buộc ở dịch vụ {m.ServiceName}, gói cước {m.PackageName}");
            //    });

            //    When(o => !string.IsNullOrEmpty(o.InstallationAddress.EndPoint), () =>
            //    {
            //        RuleFor(c => c.EpInstallationFee).GreaterThanOrEqualTo(0).WithMessage(m => $"Phí cài đặt điểm cuối là bắt buộc ở dịch vụ {m.ServiceName}, gói cước {m.PackageName}");
            //        RuleFor(c => c.EpPackagePrice).GreaterThanOrEqualTo(0).WithMessage(m => $"Cước hàng tháng điểm cuối là bắt buộc ở dịch vụ {m.ServiceName}, gói cước {m.PackageName}");
            //    });
            //});

            //When(o => o.HasStartAndEndPoint != true, () =>
            //{
            //    RuleFor(c => c.InstallationAddress.Street).NotEmpty().WithMessage(m => $"Địa chỉ lắp đặt là bắt buộc ở dịch vụ {m.ServiceName}, gói cước {m.PackageName}");
            //    RuleFor(c => c.PackagePrice).GreaterThanOrEqualTo(0).WithMessage(m => $"Cước hàng tháng là bắt buộc ở dịch vụ {m.ServiceName}, gói cước {m.PackageName}");
            //});
            //When(o => o.FlexiblePricingTypeId <= 2, () => {
            //    RuleFor(c => c.PackagePrice).GreaterThanOrEqualTo(1).WithMessage(m => $"Đơn giá hàng tháng ở dịch vụ {m.ServiceName}, gói cước {m.PackageName} phải lớn hơn 0");
            //});

            When(o => o.HasStartAndEndPoint != true, () =>
            {
                RuleFor(c => c.InstallationFee)
                .GreaterThanOrEqualTo(0)
                .WithMessage(m => $"Phí hòa mạng/lắp đặt là bắt buộc ở dịch vụ {m.ServiceName}, gói cước {m.PackageName}");
            });
        }
    }
}