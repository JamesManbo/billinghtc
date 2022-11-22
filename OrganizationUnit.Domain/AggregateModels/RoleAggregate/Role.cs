using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using OrganizationUnit.Domain.AggregateModels.RoleAggregate;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Domain.Seed;

namespace OrganizationUnit.Domain.RoleAggregate
{
    [Table("Roles")]
    public class Role : Entity, IAggregateRoot
    {
        [Required(ErrorMessage = "Tên nhóm quyền là bắt buộc")]
        [MinLength(3, ErrorMessage = "Tên nhóm quyền quá ngắn")]
        [StringLength(256)]
        public string RoleName { get; set; }
        [StringLength(256)]
        [Required(ErrorMessage = "Mã nhóm quyền là bắt buộc")]
        [RegularExpression("^[a-zA-Z0-9][a-zA-Z0-9_]+$",
            ErrorMessage = "Mã chỉ được bao gồm số, ký tự không dấu và ký tự gạch dưới(_)")]
        [MinLength(3, ErrorMessage = "Mã nhóm quyền quá ngắn")]
        [MaxLength(256, ErrorMessage = "Mã nhóm quyền quá dài")]
        public string RoleCode { get; set; }
        [StringLength(4000)]
        [MaxLength(1000, ErrorMessage = "Mô tả nhóm quyền quá dài(tối đa 1000 ký tự)")]
        public string RoleDescription { get; set; }

        public Role()
        {
        }
    }
}
