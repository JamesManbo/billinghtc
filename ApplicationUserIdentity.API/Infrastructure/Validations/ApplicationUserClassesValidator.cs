using ApplicationUserIdentity.API.Models.AccountViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Infrastructure.Validations
{
    public class ApplicationUserClassesValidator : AbstractValidator<UserClassViewModel>
    {
        public ApplicationUserClassesValidator()
        {
            RuleFor(x => x.ClassName)
                .NotEmpty().WithMessage("Tên hạng khách hàng là bắt buộc");
            RuleFor(x => x.ClassCode)
                .NotEmpty().WithMessage("Mã hạng khách hàng là bắt buộc");
            RuleFor(x => x.ConditionStartPoint)
                .GreaterThan(0).WithMessage("Điểm đầu đánh giá là bắt buộc");
            RuleFor(x => x.ConditionEndPoint)
                .GreaterThan(0).WithMessage("Điểm cuối đánh giá là bắt buộc");
            RuleFor(x => x.ConditionStartPoint)
                .LessThan(y => y.ConditionEndPoint).WithMessage("Điểm cuối đánh giá phải lớn hơn điểm đầu đánh giá");
        }
    }
}
