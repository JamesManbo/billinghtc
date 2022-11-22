using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Application.Commands.UserGroup;
using ApplicationUserIdentity.API.Infrastructure.Queries;
using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Models;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationUserIdentity.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationUserGroupsController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IUserGroupQueriesRepository _userGroupQueries;

        public ApplicationUserGroupsController(
            IMediator mediator,
            IUserGroupQueriesRepository queryRepository,
            IUserGroupRepository roleRepository
           )
        {
            _mediator = mediator;
            _userGroupRepository = roleRepository;
            _userGroupQueries = queryRepository;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_userGroupQueries.GetSelectionList(filterModel));
            }
            return Ok(_userGroupQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var group = _userGroupQueries.FindById(id);

            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CUUserGroupCommand createCommand)
        {
            var validator = new CreateUserGroupValidator();
            var validateResult = await validator.ValidateAsync(createCommand);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            ActionResponse<UserGroupDTO> rs = new ActionResponse<UserGroupDTO>();
            var existName = _userGroupQueries.CheckExistName(createCommand.Id, createCommand.GroupName);
            var existCode = _userGroupQueries.CheckExistCode(createCommand.Id, createCommand.GroupCode);
            if (existName)
            {
                rs.AddError("Tên nhóm người dùng đã tồn tại", nameof(createCommand.GroupName));
            }
            if (existCode)
            {
                rs.AddError("Mã nhóm người dùng đã tồn tại", nameof(createCommand.GroupCode));
            }
            if (existName || existCode)
                return BadRequest(rs);

            var actResponse = await _userGroupRepository.CreateAndSave(createCommand);
            if (actResponse.IsSuccess)
            {
                return CreatedAtAction("GetById", new { id = actResponse.Result.Id }, actResponse);
            }
            return BadRequest(actResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CUUserGroupCommand updateCommand)
        {
            if (id != updateCommand.Id)
            {
                return BadRequest();
            }

            var validator = new CreateUserGroupValidator();
            var validateResult = await validator.ValidateAsync(updateCommand);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            ActionResponse<UserGroupDTO> rs = new ActionResponse<UserGroupDTO>();
            var existName = _userGroupQueries.CheckExistName(updateCommand.Id, updateCommand.GroupName);
            var existCode = _userGroupQueries.CheckExistCode(updateCommand.Id, updateCommand.GroupCode);
            if (existName)
            {
                rs.AddError("Tên nhóm người dùng đã tồn tại", nameof(updateCommand.GroupName));
            }
            if (existCode)
            {
                rs.AddError("Mã nhóm người dùng đã tồn tại", nameof(updateCommand.GroupCode));
            }
            if (existName || existCode)
                return BadRequest(rs);

            var actionResponse = await _mediator.Send(updateCommand);

            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var servicePackage = _userGroupQueries.FindById(id);
            if (servicePackage == null)
            {
                return NotFound();
            }

            var deleteResponse = _userGroupRepository.DeleteAndSave(id);


            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }
    }
}