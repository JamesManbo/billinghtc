

using ApplicationUserIdentity.API.Models;
using Global.Models.StateChangedResponse;
using MediatR;

namespace ApplicationUserIdentity.API.Application.Commands.UserGroup
{
    public class CUUserGroupCommand : IRequest<ActionResponse<UserGroupDTO>>
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string GroupCode { get; set; }
        public string Description { get; set; }
    }
}
