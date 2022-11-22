using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationUserIdentity.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserUserGroupController : CustomBaseController
    {
        private readonly IUserUserGroupRepository _groupRepository;

        public UserUserGroupController(
            IUserUserGroupRepository groupRepository
)
        {

            _groupRepository = groupRepository;

        }

        [HttpDelete()]
        [Route("DeleteMapGroupUserByUserIdAndGroupId")]
        public async Task<IActionResult> DeleteApplicationUser(int userId, int groupId)
        {

            await _groupRepository.DeleteMapGroupUserByUserIdAndGroupId(userId, groupId);

            return Ok();
        }

        [HttpPost()]
        [Route("AddMapUsersUserGroup")]
        public async Task<IActionResult> AddMapUsersUserGroup(AddMapUsersAndUserGroupDTO req)
        {
            if(req.UserIds==null || req.UserIds.Count==0 || !req.GroupId.HasValue)
            {
                return BadRequest();
            }

            await _groupRepository.AddMapUsersUserGroup(req.UserIds, req.GroupId.Value);

            return Ok();
        }
    }
}