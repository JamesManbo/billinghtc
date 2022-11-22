using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using OrganizationUnit.Domain.AggregateModels.OrganizationUnitAggregate;
using OrganizationUnit.Domain.Models.OrganizationUnit;
using OrganizationUnit.Infrastructure.Repositories;
using OrganizationUnit.Infrastructure.Repositories.OrganizationUnitRepository;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Application.Commands.OrganizationUnit
{
    public class CreateOrganizationUnitCommandHandler : IRequestHandler<CreateOrganizationUnitCommand, ActionResponse<OrganizationUnitDTO>>
    {
        private readonly IOrganizationUnitRepository _organizationUnitRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;

        public CreateOrganizationUnitCommandHandler(IOrganizationUnitRepository organizationUnitRepository,
            IWrappedConfigAndMapper configAndMapper,
            IUserRepository userRepository)
        {
            _organizationUnitRepository = organizationUnitRepository;
            _configAndMapper = configAndMapper;
            this._userRepository = userRepository;
        }

        public async Task<ActionResponse<OrganizationUnitDTO>> Handle(CreateOrganizationUnitCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<OrganizationUnitDTO>();

            if (_organizationUnitRepository.CheckExistByName(request.Name))
            {
                commandResponse.AddError("Đơn vị đã tồn tại ", nameof(request.Name));
            }

            if (_organizationUnitRepository.CheckExistByCode(request.Code))
            {
                commandResponse.AddError("Mã đơn vị đã tồn tại", nameof(request.Code));
            }

            if (request.NumberPhone != null && _organizationUnitRepository.CheckExistByNumberPhone(request.NumberPhone))
            {
                commandResponse.AddError("Số điện thoại đơn vị đã tồn tại ", nameof(request.NumberPhone));
            }

            if (request.Email != null && _organizationUnitRepository.CheckExistByEmail(request.Email))
            {
                commandResponse.AddError("Email đơn vị đã tồn tại", nameof(request.Email));
            }

            if (!commandResponse.IsSuccess) return commandResponse;

            if (!request.ParentId.HasValue || request.ParentId <= 0)
            {
                request.TreePath = $"/{request.Code}/";
            }
            else
            {
                var parent = await _organizationUnitRepository.GetByIdAsync(request.ParentId);
                request.TreePath = $"{parent.TreePath}{request.Code}/";
            }

            request.NumberPhone = string.IsNullOrEmpty(request.NumberPhone) ? string.Empty : request.NumberPhone;

            var orgUnitEntity = new Domain.AggregateModels.OrganizationUnitAggregate.OrganizationUnit();
            orgUnitEntity.Name = request.Name;
            orgUnitEntity.ParentId = request.ParentId;
            orgUnitEntity.Code = request.Code;
            orgUnitEntity.ShortName = request.ShortName;
            orgUnitEntity.Address = request.Address;
            orgUnitEntity.NumberPhone = request.NumberPhone;
            orgUnitEntity.TypeId = request.TypeId;
            orgUnitEntity.Email = request.Email;
            orgUnitEntity.ProvinceId = request.ProvinceId;
            orgUnitEntity.TreePath = request.TreePath;
            orgUnitEntity.IdentityGuid = request.IdentityGuid;

            _organizationUnitRepository.Create(orgUnitEntity);


            if (request.ManagementUserIds?.Length > 0)
            {
                foreach (var mngUserId in request.ManagementUserIds)
                {
                    orgUnitEntity.OrganizationUnitUsers.Add(new OrganizationUnitUser()
                    {
                        UserId = mngUserId,
                        PositionLevel = 1
                    });
                }
            }

            await _userRepository.SaveChangeAsync();
            if (commandResponse.IsSuccess)
            {
                commandResponse.SetResult(
                    orgUnitEntity.MapTo<OrganizationUnitDTO>(_configAndMapper.MapperConfig)
                    );
            }
            return commandResponse;
        }
    }

    public class UpdateOrganizationUnitCommandHandler : IRequestHandler<UpdateOrganizationUnitCommand, ActionResponse<OrganizationUnitDTO>>
    {
        private readonly IOrganizationUnitRepository _organizationUnitRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IUserRepository _userRepository;

        public UpdateOrganizationUnitCommandHandler(IOrganizationUnitRepository organizationUnitRepository,
            IWrappedConfigAndMapper configAndMapper,
            IUserRepository userRepository)
        {
            _organizationUnitRepository = organizationUnitRepository;
            _configAndMapper = configAndMapper;
            this._userRepository = userRepository;
        }

        public async Task<ActionResponse<OrganizationUnitDTO>> Handle(UpdateOrganizationUnitCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<OrganizationUnitDTO>();

            if (request.Name != null && request.Name != "" && _organizationUnitRepository.CheckExistByName(request.Name, request.Id))
            {
                commandResponse.AddError("Đơn vị đã tồn tại ", nameof(request.Name));
            }

            if (request.Code != null && request.Code != "" && _organizationUnitRepository.CheckExistByCode(request.Code, request.Id))
            {
                commandResponse.AddError("Mã đơn vị đã tồn tại", nameof(request.Code));
            }

            if (request.NumberPhone != null && request.NumberPhone != "" && _organizationUnitRepository.CheckExistByNumberPhone(request.NumberPhone, request.Id))
            {
                commandResponse.AddError("Số điện thoại đơn vị đã tồn tại ", nameof(request.NumberPhone));
            }

            if (request.Email != null && request.Email != "" && _organizationUnitRepository.CheckExistByEmail(request.Email, request.Id))
            {
                commandResponse.AddError("Email đơn vị đã tồn tại", nameof(request.Email));
            }

            if (!commandResponse.IsSuccess) return commandResponse;

            if (!request.ParentId.HasValue || request.ParentId <= 0)
            {
                request.TreePath = $"/{request.Code}/";
            }
            else
            {
                var parent = await _organizationUnitRepository.GetByIdAsync(request.ParentId);
                request.TreePath = $"{parent.TreePath}{request.Code}/";
            }

            request.NumberPhone = string.IsNullOrEmpty(request.NumberPhone) ? string.Empty : request.NumberPhone;

            var orgUnitEntity = await _organizationUnitRepository.GetByIdAsync(request.Id);
            orgUnitEntity.Name = request.Name;
            orgUnitEntity.ParentId = request.ParentId;
            orgUnitEntity.Code = request.Code;
            orgUnitEntity.ShortName = request.ShortName;
            orgUnitEntity.Address = request.Address;
            orgUnitEntity.NumberPhone = request.NumberPhone;
            orgUnitEntity.TypeId = request.TypeId;
            orgUnitEntity.Email = request.Email;
            orgUnitEntity.ProvinceId = request.ProvinceId;
            orgUnitEntity.TreePath = request.TreePath;
            orgUnitEntity.IdentityGuid = request.IdentityGuid;

            _organizationUnitRepository.Update(orgUnitEntity);

            if (request.ManagementUserIds?.Length > 0)
            {
                var toDeleteUserId = orgUnitEntity.OrganizationUnitUsers
                      .Where(o => !request.ManagementUserIds.Contains(o.UserId))
                      .Select(o => o.UserId)
                      .ToArray();

                var toAddNewUsers = request.ManagementUserIds
                    .Where(c => orgUnitEntity.OrganizationUnitUsers.All(o => o.UserId != c));

                var toUpdateUsers = orgUnitEntity.OrganizationUnitUsers
                      .Where(o => request.ManagementUserIds.Contains(o.UserId));

                foreach (var deleteUserId in toDeleteUserId)
                {
                    orgUnitEntity.OrganizationUnitUsers.RemoveWhere(o => o.UserId == deleteUserId);
                }

                foreach (var orgUser in toUpdateUsers)
                {
                    orgUser.PositionLevel = 1;
                }

                foreach (var addNewUserId in toAddNewUsers)
                {
                    orgUnitEntity.OrganizationUnitUsers.Add(new OrganizationUnitUser()
                    {
                        UserId = addNewUserId,
                        PositionLevel = 1
                    });
                }
            }
            else
            {
                orgUnitEntity.OrganizationUnitUsers.Clear();
            }

            await _organizationUnitRepository.SaveChangeAsync();

            if (commandResponse.IsSuccess)
            {
                commandResponse.SetResult(
                    orgUnitEntity.MapTo<OrganizationUnitDTO>(_configAndMapper.MapperConfig)
                );
            }

            return commandResponse;
        }
    }
}
