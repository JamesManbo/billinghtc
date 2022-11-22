using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using OrganizationUnit.API.Models;
using OrganizationUnit.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OrganizationUnit.Domain.Models.Role;

namespace OrganizationUnit.API.Application.Commands.Role
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ActionResponse<RoleDTO>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;

        public CreateRoleCommandHandler(IRoleRepository roleRepository, IWrappedConfigAndMapper configAndMapper)
        {
            _roleRepository = roleRepository;
            _configAndMapper = configAndMapper;
        }

        public async Task<ActionResponse<RoleDTO>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<RoleDTO>();

            var createRole = await _roleRepository.CreateAndSave(request);

            commandResponse.CombineResponse(createRole);

            if (commandResponse.IsSuccess)
            {
                commandResponse.SetResult(createRole.Result.MapTo<RoleDTO>(_configAndMapper.MapperConfig));
            }

            return commandResponse;
        }
    }
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ActionResponse<RoleDTO>>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;

        public UpdateRoleCommandHandler(IRoleRepository roleRepository, IWrappedConfigAndMapper configAndMapper)
        {
            _roleRepository = roleRepository;
            _configAndMapper = configAndMapper;
        }

        public async Task<ActionResponse<RoleDTO>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<RoleDTO>();

            var updateRole = await _roleRepository.UpdateAndSave(request);

            commandResponse.CombineResponse(updateRole);

            if (commandResponse.IsSuccess)
            {
                commandResponse.SetResult(updateRole.Result.MapTo<RoleDTO>(_configAndMapper.MapperConfig));
            }

            return commandResponse;
        }
    }
}
