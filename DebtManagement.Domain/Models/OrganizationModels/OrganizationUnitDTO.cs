using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.OrganizationModels
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
        public int? TypeId { get; set; }
        public string Email { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceId { get; set; }
        public string TreePath { get; set; }
        public string IdentityGuid { get; set; }
        public int? TotalEmployees { get; set; }
    }
}
