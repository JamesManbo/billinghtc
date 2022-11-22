using FluentValidation;
using StaffApp.APIGateway.Models.ContractContentModels;
using StaffApp.APIGateway.Models.ContractModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Infrastructure.ValidationConfigs
{
    public class CreateOutContractValidator : AbstractValidator<CreateOutContract>
    {
        public CreateOutContractValidator()
        {
            RuleFor(c => c.Contractor)
                .NotNull().WithMessage("Vui lòng lựa chọn hoặc thêm mới khách hàng vào hợp đồng")
                //.Must(e => !string.IsNullOrWhiteSpace(e.IdentityGuid))
                //.WithMessage("Vui lòng lựa chọn hoặc thêm mới khách hàng vào hợp đồng")
                ;

            RuleFor(c => c.ContractCode)
                .NotEmpty().WithMessage("Số hợp đồng là bắt buộc")
                .MinimumLength(6).WithMessage("Số hợp đồng quá ngắn");

            RuleFor(c => c.MarketAreaId)
                .GreaterThan(0).WithMessage("Vùng thị trường là bắt buộc");

            //RuleFor(c => c.OrganizationUnitId)
            //    .NotEmpty().WithMessage("Phòng ban phụ trách là bắt buộc");

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
            RuleFor(c => c.ContractContentCommand).SetValidator(new ContractContentValidator());
        }
    }

    public class RequireEffectiveDateOcSrvPackageValidator : AbstractValidator<CUContractServicePackage>
    {
        public RequireEffectiveDateOcSrvPackageValidator()
        {
            RuleFor(x => x.TimeLine.Effective).NotNull()
                .WithMessage(s =>
                    $"Hợp đồng với trạng thái \"Đã nghiệm thu\" yêu cầu ngày nghiệm thu ở dịch vụ {s.ServiceName}, gói cước {s.PackageName}");
        }
    }

    public class ContractContentValidator: AbstractValidator<CUContractContent>
    {
        public ContractContentValidator()
        {

            RuleFor(c => c.Content)
                .NotEmpty().WithMessage("Điều khoản là bắt buộc");
            RuleFor(c => c.DigitalSignature != null ? c.DigitalSignature.TemporaryUrl : string.Empty)
    .NotEmpty().WithMessage("Chữ ký là bắt buộc");
        }
    }
}
