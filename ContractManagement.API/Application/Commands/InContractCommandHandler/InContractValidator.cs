using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.Commands.InContractCommand;
using FluentValidation;

namespace ContractManagement.API.Application.Commands.InContractCommandHandler
{

    public class CreateInContractValidator : AbstractValidator<CreateInContractCommand>
    {
        public CreateInContractValidator()
        {
            RuleFor(c => c.Contractor)
                .NotNull().WithMessage("Vui lòng lựa chọn hoặc thêm mới khách hàng vào hợp đồng")
                .Must(e => !string.IsNullOrWhiteSpace(e.IdentityGuid))
                .WithMessage("Vui lòng lựa chọn hoặc thêm mới khách hàng vào hợp đồng");

            RuleFor(c => c.ContractCode)
                .NotEmpty().WithMessage("Số hợp đồng là bắt buộc")
                .MinimumLength(6).WithMessage("Số hợp đồng quá ngắn");

            RuleFor(c => c.MarketAreaId)
                .GreaterThan(0).WithMessage("Vùng thị trường là bắt buộc");

            RuleFor(c => c.OrganizationUnitId)
                .NotEmpty().WithMessage("Phòng ban phụ trách là bắt buộc");

            RuleFor(c => c.CurrencyUnitId)
                .GreaterThan(0).WithMessage("Đơn vị tiền tệ là bắt buộc");

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

            When(c => c.ContractTypeId == InContractType.InChannelRental.Id, () =>
            {
                RuleFor(c => c.ServicePackages)
                    .NotNull().WithMessage("Vui lòng thêm kênh truyền")
                    .NotEmpty().WithMessage("Vui lòng thêm kênh truyền");
            });

            //When(c => c.ContractTypeId == InContractType.InCommission.Id, () =>
            //{
            //    RuleFor(c => c.InContractServices)
            //        .NotEmpty().WithMessage("Vui lòng thêm tỉ lệ hoa hồng theo dịch vụ");
            //});
        }
    }

    public class UpdateInContractValidator : AbstractValidator<UpdateInContractCommand>
    {
        public UpdateInContractValidator()
        {
            RuleFor(c => c.Contractor)
                .NotNull().WithMessage("Vui lòng lựa chọn hoặc thêm mới bên cung cấp dịch vụ")
                .Must(e => !string.IsNullOrWhiteSpace(e.IdentityGuid))
                .WithMessage("Vui lòng lựa chọn hoặc thêm mới bên cung cấp dịch vụ");

            RuleFor(c => c.ContractCode)
                .NotEmpty().WithMessage("Số hợp đồng là bắt buộc")
                .MinimumLength(6).WithMessage("Số hợp đồng quá ngắn");

            RuleFor(c => c.MarketAreaId)
                .GreaterThan(0).WithMessage("Vùng thị trường là bắt buộc");

            RuleFor(c => c.OrganizationUnitId)
                .NotEmpty().WithMessage("Phòng ban phụ trách là bắt buộc");

            RuleFor(c => c.MarketAreaId)
                .GreaterThan(0).WithMessage("Đơn vị tiền tệ là bắt buộc");

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

            When(c => c.ContractTypeId == InContractType.InChannelRental.Id, () =>
            {
                RuleFor(c => c.ServicePackages)
                    .NotNull().WithMessage("Vui lòng thêm kênh truyền")
                    .NotEmpty().WithMessage("Vui lòng thêm kênh truyền");
            });

            //When(c => c.ContractTypeId == InContractType.InCommission.Id, () =>
            //{
            //    RuleFor(c => c.InContractServices)
            //        .NotEmpty().WithMessage("Vui lòng thêm tỉ lệ hoa hồng theo dịch vụ");
            //});
        }
    }
}
