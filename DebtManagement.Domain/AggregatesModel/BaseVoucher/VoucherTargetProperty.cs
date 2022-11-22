using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DebtManagement.Domain.AggregatesModel.BaseVoucher
{
    [Table("VoucherTargetProperties")]
    public class VoucherTargetProperty : Entity
    {
        public int TargetId { get; set; }
        public int? StructureId { get; set; }
        public int? CategoryId { get; set; }
        public string GroupIds { get; set; }
        public int? ClassId { get; set; }
        public int? TypeId { get; set; }
        public string IndustryIds { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }
        public string StructureName { get; set; }
        public string CategoryName { get; set; }
        public string GroupNames { get; set; }
        public string ClassName { get; set; }
        public string TypeName { get; set; }
        public string IndustryNames { get; set; }
    }
}
