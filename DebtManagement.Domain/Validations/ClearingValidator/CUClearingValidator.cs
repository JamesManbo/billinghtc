using DebtManagement.Domain.Commands.ClearingCommand;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Validations.ClearingValidator
{
    public class CUClearingValidator : AbstractValidator<CUClearingCommand>
    {
        public CUClearingValidator()
        {
            RuleFor(r => r.CodeClearing).NotEmpty().WithMessage("Mã bù trừ là bắt buộc");
            RuleFor(r => r.SelectionReceiptIds).NotEmpty().WithMessage("Phiếu thu là bắt buộc");
            RuleFor(r => r.SelectionPaymentIds).NotEmpty().WithMessage("Phiếu đề nghị thanh toán là bắt buộc");
            RuleFor(r => r.MarketAreaId).GreaterThan(0).WithMessage("Vùng thị trường là bắt buộc");
            RuleFor(r => r.ClearingDate).NotEmpty().WithMessage("Ngày thực hiện là bắt buộc");
            When(o => !string.IsNullOrEmpty(o.Id), () =>
             {
                 RuleFor(r => r.StatusId).GreaterThan(0).WithMessage("Trạng thái là bắt buộc");
             });
            RuleFor(r => r.ApprovalUserId).NotEmpty().WithMessage("Người xác nhận thanh toán là bắt buộc");
            RuleFor(r => r.CreatedUserId).NotEmpty().WithMessage("Người thực hiện là bắt buộc");
            RuleFor(r => r.OrganizationUnitId).NotEmpty().WithMessage("Phòng ban là bắt buộc");
        }
    }
}
