using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.VoucherTargetCommand
{
    public class UpdateContractorPropertyCommand : IRequest
    {
        public int TargetId { get; set; }
        public int? StructureId { get; set; }
        public int? CategoryId { get; set; }
        public string GroupId { get; set; }
        public string OldGroupName { get; set; }
        public string NewGroupName { get; set; }
        public int? ClassId { get; set; }
        public int? TypeId { get; set; }
        public string IndustryId { get; set; }
        public string StructureName { get; set; }
        public string CategoryName { get; set; }
        public string ClassName { get; set; }
        public string TypeName { get; set; }
        public string OldIndustryName { get; set; }
        public string NewIndustryName { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }
    }
}
