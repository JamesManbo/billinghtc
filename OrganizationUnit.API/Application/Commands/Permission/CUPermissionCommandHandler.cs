using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using OrganizationUnit.API.Models;
using OrganizationUnit.Infrastructure.Repositories;
using System.Threading;
using System.Threading.Tasks;
using OrganizationUnit.Domain.Models.Permission;

namespace OrganizationUnit.API.Application.Commands
{
    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, ActionResponse<PermissionDTO>>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;

        public CreatePermissionCommandHandler(IPermissionRepository permissionRepository, IWrappedConfigAndMapper configAndMapper)
        {
            _permissionRepository = permissionRepository;
            _configAndMapper = configAndMapper;
        }

        public async Task<ActionResponse<PermissionDTO>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<PermissionDTO>();

            var createRole = await _permissionRepository.CreateAndSave(request); 

            commandResponse.CombineResponse(createRole);

            if (commandResponse.IsSuccess)
            {
                commandResponse.SetResult(createRole.Result.MapTo<PermissionDTO>(_configAndMapper.MapperConfig));
            }

            return commandResponse;
        }
    }

    public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, ActionResponse<PermissionDTO>>
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;

        public UpdatePermissionCommandHandler(IPermissionRepository permissionRepository, IWrappedConfigAndMapper configAndMapper)
        {
            _permissionRepository = permissionRepository;
            _configAndMapper = configAndMapper;
        }

        public async Task<ActionResponse<PermissionDTO>> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<PermissionDTO>();

            var updateRole = await _permissionRepository.UpdateAndSave(request);

            commandResponse.CombineResponse(updateRole);

            if (commandResponse.IsSuccess)
            {
                commandResponse.SetResult(updateRole.Result.MapTo<PermissionDTO>(_configAndMapper.MapperConfig));
            }

            return commandResponse;
        }
    }
}
