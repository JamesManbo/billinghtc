using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.VoucherTargetCommand
{
    public class RemoveContractorPropertyCommand : IRequest
    {
        public int? StructureId { get; set; }
        public int? CategoryId { get; set; }
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public int? ClassId { get; set; }
        public int? TypeId { get; set; }
        public int? IndustryId { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }
    }
}
