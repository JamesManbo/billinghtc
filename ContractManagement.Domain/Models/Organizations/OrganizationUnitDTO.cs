using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models.Organizations
{
    public class OrganizationUnitDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string ParentName { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string NumberPhone { get; set; }
        public int? TypeId { get; set; } //Loại đơn vị
        public string Email { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceId { get; set; } //Tỉnh/Thành phố
        public string TreePath { get; set; }
        public string IdentityGuid { get; set; }
        public int? TotalEmployees { get; set; }
        public int? ManagementUserId { get; set; }
        public string ManagementUser { get; set; }
    }
}
