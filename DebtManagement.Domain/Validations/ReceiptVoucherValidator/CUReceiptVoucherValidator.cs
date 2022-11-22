using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using FluentValidation.Validators;
using DebtManagement.Domain.Commands.DebtCommand;
using System.Linq;

namespace DebtManagement.Domain.Validations.ReceiptVoucherValidator
{
    public class CreateReceiptVoucherValidator : AbstractValidator<CreateReceiptVoucherCommand>
    {
        public CreateReceiptVoucherValidator()
        {
            //RuleFor(c => c.GrandTotalIncludeDebt)
            //    .GreaterThan(0)
            //    .WithMessage("Số tiền phải thu không hợp lệ");

            RuleFor(r => r.Content).NotNull().WithMessage("Nội dung phiếu thu là bắt buộc");

            When(c => c.TypeId != ReceiptVoucherType.Billing.Id, () =>
            {
                RuleFor(r => r.IssuedDate).NotNull().WithMessage("Ngày phát hành là bắt buộc");
            });

            When(c => c.Source == Commands.CommandSource.CMS || c.Source == Commands.CommandSource.MobileApplication, () =>
            {
                RuleFor(r => r.VoucherCode).NotEmpty().WithMessage("Số phiếu thu là bắt buộc");
            });

            When(c => c.TypeId != ReceiptVoucherType.Clearing.Id,
                () =>
                {
                    RuleFor(r => r.OutContractId).NotEmpty().WithMessage("Hợp đồng đầu ra là bắt buộc");
                });

            When(c => c.StatusId == ReceiptVoucherStatus.Success.Id,
                () =>
                {
                    RuleFor(c => c.PaymentDate).NotNull().WithMessage("Ngày thanh toán là bắt buộc");
                });
        }
    }

    public class UpdateReceiptVoucherValidator : AbstractValidator<UpdateReceiptVoucherCommand>
    {
        public UpdateReceiptVoucherValidator()
        {
            When(c => c.TypeId != ReceiptVoucherType.Billing.Id, () =>
            {
                RuleFor(r => r.IssuedDate).NotNull().WithMessage("Ngày phát hành là bắt buộc");
                RuleFor(r => r.PaidTotal).NotNull().WithMessage("Số tiền thực thu là bắt buộc");
            });

            When(c => c.Source == Commands.CommandSource.CMS || c.Source == Commands.CommandSource.MobileApplication, () =>
            {
                RuleFor(r => r.VoucherCode).NotEmpty().WithMessage("Số hợp đồng là bắt buộc");
            });

            RuleFor(r => r.OutContractId).NotEmpty().WithMessage("Hợp đồng đầu ra là bắt buộc");
            RuleFor(r => r.OutContractId).NotEmpty().WithMessage("Hợp đồng đầu ra là bắt buộc");
            RuleFor(r => r.Content).NotNull().WithMessage("Nội dung phiếu thu là bắt buộc");

            When(c => c.StatusId == ReceiptVoucherStatus.Success.Id, () =>
            {
                RuleFor(c => c.PaymentDate).NotNull().WithMessage("Ngày thanh toán là bắt buộc");
            });

            When(c => c.StatusId == ReceiptVoucherStatus.Canceled.Id, () =>
            {
                RuleFor(c => c.CancellationReason).NotNull().NotEmpty().WithMessage("Lý do hủy là bắt buộc");
            });


            When(c => c.StatusId == ReceiptVoucherStatus.CollectOnBehalf.Id,
            () =>
            {
                RuleFor(r => r.CashierUserId)
                .NotNull().WithMessage("Nhân viên/đại lý thu hộ là bắt buộc");
            });

            When(c => c.StatusId == ReceiptVoucherStatus.CollectOnBehalf.Id &&
                !c.IncurredDebtPayments.Exists(ReceiptVoucherValidators.ValidPaidAmount),
            () =>
            {
                RuleFor(r => r.CashierUserId)
                .NotNull().WithMessage("Nhân viên/đại lý thu hộ là bắt buộc");

                RuleFor(r => r.OpeningDebtPayments)
                .PaidAmountValid();
            });

            When(c => c.StatusId == ReceiptVoucherStatus.CollectOnBehalf.Id &&
                    !c.OpeningDebtPayments.Exists(ReceiptVoucherValidators.ValidPaidAmount),
            () =>
            {
                RuleFor(r => r.IncurredDebtPayments)
                .PaidAmountValid();

            });

            When(c => c.StatusId == ReceiptVoucherStatus.Invoiced.Id,() =>{
                  RuleFor(r => r.InvoiceDate).NotEmpty().WithMessage("Ngày xuất hóa đơn là bắt buộc");
                  RuleFor(r => r.InvoiceCode).NotEmpty().WithMessage("Số hóa đơn là bắt buộc");
            });
        }
    }

    public class CancelReceiptVoucherValidator : AbstractValidator<CancelReceiptVoucherCommand>
    {
        public CancelReceiptVoucherValidator()
        {
            RuleFor(c => c.CancellationReason).NotEmpty().WithMessage("Lý do hủy là bắt buộc");
        }
    }

    public class CancelListReceiptVoucherValidator : AbstractValidator<CancelListReceiptVoucherCommand>
    {
        public CancelListReceiptVoucherValidator()
        {
            RuleFor(c => c.Ids).NotEmpty().WithMessage("Phiếu thu là bắt buộc");
            RuleFor(c => c.CancellationReason).NotEmpty().WithMessage("Lý do hủy là bắt buộc");
        }
    }

    public static class ReceiptVoucherValidators
    {
        public static IRuleBuilderOptions<T, List<CuOpeningDebtPaymentCommand>> PaidAmountValid<T>(this IRuleBuilder<T, List<CuOpeningDebtPaymentCommand>> ruleBuilder)
        {
            return ruleBuilder.NotEmpty().WithMessage("Số tiền thực thu không hợp lệ")
                    .Must(d => d.Count > 0 && d.Exists(ValidPaidAmount))
                    .WithMessage("Số tiền thực thu không hợp lệ");
        }

        public static Predicate<CuOpeningDebtPaymentCommand> ValidPaidAmount =
            p => p.PaymentDetails.Count > 0 && p.PaymentDetails.Exists(pd => pd.PaidAmount > 0);
    }
}