using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Application.Commands.UserGroup
{
    public class CUUserGroupCommandHandler : IRequestHandler<CUUserGroupCommand, ActionResponse<UserGroupDTO>>
    {
        private readonly IUserGroupRepository _userGroupRepository;

        public CUUserGroupCommandHandler(IUserGroupRepository userGroupRepository)
        {
            _userGroupRepository = userGroupRepository;
            
        }
        public async Task<ActionResponse<UserGroupDTO>> Handle(CUUserGroupCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<UserGroupDTO>();

            if (request.Id == 0)
            {
                commandResponse.CombineResponse(await _userGroupRepository.CreateAndSave(request));
            }
            else
            {
                commandResponse.CombineResponse(await _userGroupRepository.UpdateAndSave(request));
            }

            return commandResponse;
        }
    }
}
