using DebtManagement.Domain.Commands.PaymentVoucherCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;

namespace DebtManagement.Domain.Validations.PaymentVoucherValidator
{
    public class CreatePaymentVoucherValidator : AbstractValidator<CreatePaymentVoucherCommand>
    {
        public CreatePaymentVoucherValidator()
        {
            RuleFor(r => r.InContractId).NotEmpty().WithMessage("Vui lòng chọn hợp đồng đầu vào đề nghị thanh toán");
            RuleFor(r => r.VoucherCode).NotEmpty().WithMessage("Số phiếu đề nghị thanh toán là bắt buộc");
            RuleFor(r => r.IssuedDate).NotNull().WithMessage("Ngày phát hành là bắt buộc");
            RuleFor(r => r.CreatedUserId).NotNull().WithMessage("Người lập phiếu là bắt buộc");
            RuleFor(r => r.CashierUserId).NotNull().WithMessage("Người thu ngân là bắt buộc");
            RuleFor(r => r.PaidTotal).NotNull().WithMessage("Số tiền thực thu là bắt buộc");
            RuleFor(r => r.Content).NotNull().WithMessage("Nội dung phiếu thu là bắt buộc");

            When(c => c.StatusId == PaymentVoucherStatus.Success.Id,
                () =>
                {
                    RuleFor(c => c.PaymentDate).NotNull().WithMessage("Ngày thanh toán là bắt buộc");
                    RuleFor(c => c.PaidTotal).GreaterThan(0).WithMessage("Số tiền thực chi không hợp lệ");
                });
        }
    }
    public class UpdatePaymentVoucherValidator : AbstractValidator<UpdatePaymentVoucherCommand>
    {
        public UpdatePaymentVoucherValidator()
        {
            RuleFor(r => r.VoucherCode).NotEmpty().WithMessage("Số hợp đồng là bắt buộc");
            RuleFor(r => r.IssuedDate).NotNull().WithMessage("Ngày phát hành là bắt buộc");
            RuleFor(r => r.CreatedUserId).NotNull().WithMessage("Người lập phiếu là bắt buộc");
            RuleFor(r => r.CashierUserId).NotNull().WithMessage("Người thu ngân là bắt buộc");
            RuleFor(r => r.PaidTotal).NotNull().WithMessage("Số tiền thực thu là bắt buộc");
            RuleFor(r => r.Content).NotNull().WithMessage("Nội dung phiếu thu là bắt buộc");

            When(c => c.StatusId == PaymentVoucherStatus.Success.Id,
                () =>
                {
                    RuleFor(c => c.PaymentDate).NotNull().WithMessage("Ngày thanh toán là bắt buộc");
                    RuleFor(c => c.PaidTotal).GreaterThan(0).WithMessage("Số tiền thực chi không hợp lệ");
                });

            When(c => c.StatusId == PaymentVoucherStatus.Canceled.Id,
                () =>
                {
                    RuleFor(c => c.CancellationReason)
                    .NotNull().WithMessage("Lý do hủy phiếu ĐNTT là bắt buộc")
                    .NotEmpty().WithMessage("Lý do hủy phiếu ĐNTT là bắt buộc");
                });
        }
    }

    public class CancelPaymentVoucherValidator : AbstractValidator<CancelPaymentVoucherCommand>
    {
        public CancelPaymentVoucherValidator()
        {
            RuleFor(c => c.StatusId).Equal(PaymentVoucherStatus.New.Id).WithMessage("Trạng thái không hợp lệ");
            RuleFor(c => c.CancellationReason).NotNull().NotEmpty().WithMessage("Lý do hủy là bắt buộc");
        }
    }

    public class CancelListPaymentVoucherValidator : AbstractValidator<CancelListPaymentVoucherCommand>
    {
        public CancelListPaymentVoucherValidator()
        {
            RuleFor(c => c.Ids).NotNull().NotEmpty().WithMessage("Phiếu đề nghị thanh toán là bắt buộc");
            RuleFor(c => c.CancellationReason).NotNull().NotEmpty().WithMessage("Lý do hủy là bắt buộc");
        }
    }
}
