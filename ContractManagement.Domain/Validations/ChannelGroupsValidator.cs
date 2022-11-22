using ContractManagement.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Validations
{
    public class ChannelGroupsValidator : AbstractValidator<ChannelGroupDTO>
    {
        public ChannelGroupsValidator()
        {
            RuleFor(r => r.ChannelGroupName).NotEmpty().WithMessage("Tên chùm kênh là bắt buộc");
            RuleFor(r => r.ChannelGroupCode).NotEmpty().WithMessage("Mã chùm kênh là bắt buộc");
        }
    }
}
