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
using OrganizationUnit.API.Application.Commands.Role;
using OrganizationUnit.API.Infrastructure.Services;
using OrganizationUnit.Domain.Models.Role;
using OrganizationUnit.Infrastructure.Queries;
using OrganizationUnit.Infrastructure.Repositories;

namespace OrganizationUnit.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RolesController : CustomBaseController
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IMediator _mediator;
        private readonly IRoleRepository _roleRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IRoleQueries _queryRepository;

        public RolesController(
            ILogger<RolesController> logger,
            IMediator mediator,
            IRoleQueries queryRepository,
            IRoleRepository roleRepository,
            IWrappedConfigAndMapper configAndMapper)
        {
            _logger = logger;
            _mediator = mediator;
            _roleRepository = roleRepository;
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
            var entity = await _roleRepository.GetByIdAsync(id);
            return Ok(entity.MapTo<UpdateRoleCommand>(_configAndMapper.MapperConfig));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateRoleCommand createRoleCommand)
        {
            if (createRoleCommand.RoleName != null && createRoleCommand.RoleName != ""
                && createRoleCommand.RoleCode != null && createRoleCommand.RoleCode != ""
            )
            {
                var actionResponse = new ActionResponse<RoleDTO>();
                var roleName = createRoleCommand.RoleName.Trim();
                var roleCode = createRoleCommand.RoleCode.Trim();
                var checkExitName = _roleRepository.CheckExitRoleName(roleName, 0);
                var checkExitCode = _roleRepository.CheckExitRoleCode(roleCode, 0);
                if (!checkExitName || !checkExitCode)//Tồn tại tên hoặc mã 
                {
                    if (!checkExitName)
                    {
                        actionResponse.AddError("Tên vai trò đã tồn tại ", nameof(createRoleCommand.RoleName));
                    }
                    if (!checkExitCode)
                    {
                        actionResponse.AddError("Mã vai trò đã tồn tại", nameof(createRoleCommand.RoleCode));
                    }
                    return BadRequest(actionResponse);
                }
                else
                {
                    actionResponse.CombineResponse(await _mediator.Send(createRoleCommand));
                    if (actionResponse.IsSuccess)
                    {
                        return Ok(actionResponse);
                    }

                    return BadRequest(actionResponse);
                }
            }
            else
            {
                var actionResponse = new ActionResponse<RoleDTO>();
                actionResponse.CombineResponse(await _mediator.Send(createRoleCommand));
                return BadRequest(actionResponse);
            }
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRoleCommand updateRoleCommand)
        {
            var actionResponse = new ActionResponse<RoleDTO>();
            var roleName = updateRoleCommand.RoleName.Trim();
            var roleCode = updateRoleCommand.RoleCode.Trim();
            var checkExitName = _roleRepository.CheckExitRoleName(roleName, id);
            var checkExitCode = _roleRepository.CheckExitRoleCode(roleCode, id);
            if (!checkExitName || !checkExitCode)//Tồn tại tên hoặc mã 
            {
                if (!checkExitName)
                {
                    actionResponse.AddError("Tên vai trò đã tồn tại", nameof(updateRoleCommand.RoleName));
                }
                if (!checkExitCode)
                {
                    actionResponse.AddError("Mã vai trò đã tồn tại", nameof(updateRoleCommand.RoleCode));
                }
                return BadRequest(actionResponse);
            }
            else
            {
                if (id != updateRoleCommand.Id)
                {
                    return BadRequest();
                }
                else
                {
                    actionResponse.CombineResponse(await _mediator.Send(updateRoleCommand));
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
            if (!_roleRepository.IsExisted(id)) return BadRequest();

            return Ok(_roleRepository.DeleteAndSave(id));
        }
    }
}