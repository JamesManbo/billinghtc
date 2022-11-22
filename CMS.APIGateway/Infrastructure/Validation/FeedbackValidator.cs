using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.APIGateway.Models.FeedbackAndRequest;
using FluentValidation;

namespace CMS.APIGateway.Infrastructure.Validation
{
    public class FeedbackValidator : AbstractValidator<CUFeedbackAndRequest>
    {
        public FeedbackValidator()
        {
            RuleFor(e => e.CId)
                .NotEmpty().WithMessage("Mã CId là bắt buộc")
                .NotNull().WithMessage("Mã CId là bắt buộc");
            RuleFor(e => e.DateRequested)
                .NotNull().WithMessage("Ngày nhận yêu cầu là bắt buộc");
            RuleFor(e => e.Content)
                .NotNull().WithMessage("Nội dung báo cáo sự cố là bắt buộc");
        }
    }
}
