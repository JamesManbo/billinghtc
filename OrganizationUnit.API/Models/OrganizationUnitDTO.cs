using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Models
{
    public class OrganizationUnitDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string NumberPhone { get; set; }
        public int? TypeId { get; set; } //Loại đơn vị
        public string Email { get; set; }
        public int? ProvinceId { get; set; } //Tỉnh/Thành phố
    }
}
