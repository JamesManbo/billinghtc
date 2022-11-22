using Global.Models.StateChangedResponse;
using MediatR;
using OrganizationUnit.Domain.Models.Role;

namespace OrganizationUnit.API.Application.Commands.Role
{
    public class CreateRoleCommand : IRequest<ActionResponse<RoleDTO>>
    {
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public string RoleDescription { get; set; }
        public CreateRoleCommand()
        {

        }
    }
    public class UpdateRoleCommand : IRequest<ActionResponse<RoleDTO>>
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public string RoleDescription { get; set; }
        public UpdateRoleCommand()
        {

        }
    }
}
