using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenericRepository.Configurations;
using Global.Models.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrganizationUnit.Domain.Models.RolePermission;
using OrganizationUnit.Infrastructure.Queries;
using OrganizationUnit.Infrastructure.Repositories;

namespace OrganizationUnit.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RolePermissionsController : CustomBaseController
    {
        private readonly ILogger<RolePermissionsController> _logger;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IRolePermissionQueryRepository _queryRepository;

        public RolePermissionsController(
            ILogger<RolePermissionsController> logger,
            IRolePermissionQueryRepository queryRepository,
            IRolePermissionRepository rolePermissionRepository,
            IWrappedConfigAndMapper configAndMapper)
        {
            _logger = logger;
            _rolePermissionRepository = rolePermissionRepository;
            _configAndMapper = configAndMapper;
            _queryRepository = queryRepository;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        {
            return Ok(_queryRepository.GetList(filterModel));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAndUpdate([FromBody] RolePermissionDto rolePermissionDto)
        {
            var entity = _rolePermissionRepository.GetEntityByRoleIdAndPermissionId(rolePermissionDto.RoleId, rolePermissionDto.PermissionId);
            if (entity == null)
            {
                return Ok(await _rolePermissionRepository.CreateAndSave(rolePermissionDto));
            }
            else
            {
                entity.Grant = rolePermissionDto.Grant;
                entity.Deny = rolePermissionDto.Deny;
                return Ok(await _rolePermissionRepository.UpdateAndSave(entity));
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_rolePermissionRepository.IsExisted(id)) return BadRequest();

            return Ok(_rolePermissionRepository.DeleteAndSave(id));
        }
    }
}