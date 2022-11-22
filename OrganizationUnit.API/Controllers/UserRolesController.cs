using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenericRepository.Configurations;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrganizationUnit.Domain.Models.UserRole;
using OrganizationUnit.Infrastructure.Queries;
using OrganizationUnit.Infrastructure.Repositories;

namespace OrganizationUnit.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserRolesController : CustomBaseController
    {
        private readonly ILogger<UserRolesController> _logger;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IUserRoleQueryRepository _queryRepository;

        public UserRolesController(
            ILogger<UserRolesController> logger,
            IUserRoleQueryRepository queryRepository,
            IUserRoleRepository userRoleRepository,
            IWrappedConfigAndMapper configAndMapper)
        {
            _logger = logger;
            _userRoleRepository = userRoleRepository;
            _configAndMapper = configAndMapper;
            _queryRepository = queryRepository;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        {
            return Ok(_queryRepository.GetList(filterModel));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserRoleDto UserRoleDto)
        {
            var entity = await _userRoleRepository.CreateAndSave(UserRoleDto);
            return Ok(entity.Result);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_userRoleRepository.IsExisted(id)) return BadRequest();

            return Ok(_userRoleRepository.DeleteAndSave(id));
        }
    }
}