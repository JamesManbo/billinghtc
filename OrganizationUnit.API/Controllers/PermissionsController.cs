using System.Threading.Tasks;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrganizationUnit.API.Application.Commands;
using OrganizationUnit.Domain.Models.Permission;
using OrganizationUnit.Infrastructure.Queries;
using OrganizationUnit.Infrastructure.Repositories;

namespace OrganizationUnit.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PermissionsController : CustomBaseController
    {
        private readonly ILogger<PermissionsController> _logger;
        private readonly IMediator _mediator;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private IPermissionQueryRepository _queryRepository;

        public PermissionsController(
            ILogger<PermissionsController> logger,
            IMediator mediator,
            IPermissionQueryRepository queryRepository,
            IPermissionRepository permissionRepository,
            IWrappedConfigAndMapper configAndMapper)
        {
            _logger = logger;
            _mediator = mediator;
            _permissionRepository = permissionRepository;
            _configAndMapper = configAndMapper;
            _queryRepository = queryRepository;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        {
            return Ok(_queryRepository.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _permissionRepository.GetByIdAsync(id);
            return Ok(entity.MapTo<UpdatePermissionCommand>(_configAndMapper.MapperConfig));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreatePermissionCommand createPermissionCommand)
        {
            if (createPermissionCommand.PermissionName != null && createPermissionCommand.PermissionName != ""
                && createPermissionCommand.PermissionCode != null && createPermissionCommand.PermissionCode != ""
            )
            {
                var actionResponse = new ActionResponse<PermissionDTO>();
                var permissionName = createPermissionCommand.PermissionName.Trim();
                var permissionCode = createPermissionCommand.PermissionCode.Trim();
                var checkExitName = _permissionRepository.CheckExitPermissionName(permissionName, 0);
                var checkExitCode = _permissionRepository.CheckExitPermissionCode(permissionCode, 0);
                if (!checkExitName || !checkExitCode)//Tồn tại tên hoặc mã 
                {
                    if (!checkExitName)
                    {
                        actionResponse.AddError("Tên quyền đã tồn tại ", nameof(createPermissionCommand.PermissionName));
                    }
                    if (!checkExitCode)
                    {
                        actionResponse.AddError("Mã quyền đã tồn tại", nameof(createPermissionCommand.PermissionCode));
                    }
                    return BadRequest(actionResponse);
                }
                else
                {
                    actionResponse.CombineResponse(await _mediator.Send(createPermissionCommand));
                    if (actionResponse.IsSuccess)
                    {
                        return Ok(actionResponse);
                    }

                    return BadRequest(actionResponse);
                }
            }
            else
            {
                var actionResponse = new ActionResponse<PermissionDTO>();
                actionResponse.CombineResponse(await _mediator.Send(createPermissionCommand));
                return BadRequest(actionResponse);
            }

        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePermissionCommand updatePermissionCommand)
        {
            var actionResponse = new ActionResponse<PermissionDTO>();
            var permissionName = updatePermissionCommand.PermissionName.Trim();
            var permissionCode = updatePermissionCommand.PermissionCode.Trim();
            var checkExitName = _permissionRepository.CheckExitPermissionName(permissionName, id);
            var checkExitCode = _permissionRepository.CheckExitPermissionCode(permissionCode, id);
            if (!checkExitName || !checkExitCode)//Tồn tại tên hoặc mã 
            {
                if (!checkExitName)
                {
                    actionResponse.AddError("Tên quyền đã tồn tại", nameof(updatePermissionCommand.PermissionName));
                }
                if (!checkExitCode)
                {
                    actionResponse.AddError("Mã quyền đã tồn tại", nameof(updatePermissionCommand.PermissionCode));
                }
                return BadRequest(actionResponse);
            }
            else
            {
                if (id != updatePermissionCommand.Id)
                {
                    return BadRequest();
                }
                else
                {
                    actionResponse.CombineResponse(await _mediator.Send(updatePermissionCommand));
                    if (actionResponse.IsSuccess)
                    {
                        return Ok(actionResponse);
                    }
                    else
                    {
                        return BadRequest(actionResponse);
                    }
                }
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_permissionRepository.IsExisted(id)) return BadRequest();

            return Ok(_permissionRepository.DeleteAndSave(id));
        }
    }
}