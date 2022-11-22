using System;
using OrganizationUnit.Domain.AggregateModels.UserAggregate;
using OrganizationUnit.Domain.Seed;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrganizationUnit.Domain.AggregateModels.OrganizationUnitAggregate
{
    [Table("OrganizationUnits")]
    public class OrganizationUnit : Entity
    {
        public OrganizationUnit()
        {
            OrganizationUnitUsers = new HashSet<OrganizationUnitUser>();
        }

        [Required]
        public string IdentityGuid { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
        public int? ParentId { get; set; }
        [StringLength(256)]
        public string Code { get; set; }
        [StringLength(256)]
        public string ShortName { get; set; }
        [StringLength(1000)]
        public string Address { get; set; }
        [StringLength(100)]
        public string NumberPhone { get; set; }
        public int? TypeId { get; set; } // Loại đơn vị
        [StringLength(256)]
        public string Email { get; set; }
        public string ProvinceId { get; set; } // Tỉnh/Thành phố
        public string TreePath { get; set; }
        public virtual HashSet<OrganizationUnitUser> OrganizationUnitUsers { get; set; }

    }
}
