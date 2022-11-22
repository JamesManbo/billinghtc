using Global.Models.StateChangedResponse;
using MediatR;
using OrganizationUnit.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrganizationUnit.Domain.Models.OrganizationUnit;

namespace OrganizationUnit.API.Application.Commands.OrganizationUnit
{
    public class CreateOrganizationUnitCommand : IRequest<ActionResponse<OrganizationUnitDTO>>
    {
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Code { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string NumberPhone { get; set; }
        public int? TypeId { get; set; } //Loại đơn vị
        public string Email { get; set; }
        public string ProvinceId { get; set; } //Tỉnh/Thành phố
        public string TreePath { get; set; }
        public string IdentityGuid { get; set; }
        public int[] ManagementUserIds { get; set; }
    }

    public class UpdateOrganizationUnitCommand : IRequest<ActionResponse<OrganizationUnitDTO>>
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
        public string ProvinceId { get; set; } //Tỉnh/Thành phố
        public string TreePath { get; set; }
        public string IdentityGuid { get; set; }
        public int[] ManagementUserIds { get; set; }
    }
}
