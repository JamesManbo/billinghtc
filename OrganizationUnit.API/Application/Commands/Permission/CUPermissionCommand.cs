using Global.Models.StateChangedResponse;
using MediatR;
using OrganizationUnit.API.Models;
using OrganizationUnit.Domain.Models.Permission;

namespace OrganizationUnit.API.Application.Commands
{
    public class CreatePermissionCommand : IRequest<ActionResponse<PermissionDTO>>
    {
        public int Id { get; set; }
        public int PermissionSetId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionPage { get; set; }
        public string PermissionCode { get; set; }
        public string Description { get; set; }
    }

    public class UpdatePermissionCommand : IRequest<ActionResponse<PermissionDTO>>
    {
        public int Id { get; set; }
        public int PermissionSetId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionCode { get; set; }
        public string PermissionPage { get; set; }
        public string Description { get; set; }
    }
}
