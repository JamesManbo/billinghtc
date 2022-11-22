using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Configs.Authentication;
using Global.Configs.SystemArgument;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OrganizationUnit.API.Application.Commands.User;
using OrganizationUnit.API.Infrastructure.Services;
using OrganizationUnit.API.PolicyBasedAuthProvider;
using OrganizationUnit.Domain.Commands.User;
using OrganizationUnit.Domain.Models.OrganizationUnit;
using OrganizationUnit.Domain.Models.User;
using OrganizationUnit.Infrastructure.Queries;

namespace OrganizationUnit.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.CmsApiIdentityKey)]
    [Route("[controller]")]
    public class UsersController : CustomBaseController
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IMediator _mediator;
        private readonly IUserQueries _userQueries;
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        protected readonly DepartmentCode DepartmentCode;

        public UsersController(
            ILogger<UsersController> logger,
            IMediator mediator,
            IUserQueries userQueries,
            IUserService userService,
            IUserRoleQueryRepository userRoleQueies, IConfiguration config)
        {
            _logger = logger;
            _mediator = mediator;
            _userQueries = userQueries;
            _userService = userService;
            _config = config;
            DepartmentCode = _config.GetSection("DepartmentCode").Get<DepartmentCode>();
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] UserRequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_userQueries.GetSelectionList());
            }

            if (filterModel.Type == RequestType.Autocomplete)
            {
                return Ok(_userQueries.Autocomplete(filterModel));
            }

            return Ok(_userQueries.GetList(filterModel));
        }

        [HttpGet("GetListTypeSelection")]
        public IActionResult GetListTypeSelection(bool? isPartner = null, string organizationUnitTag = "", string organizationUnitCode = "", bool isNotBelongToOrgUnit = false)
        {
            string filterOrgUnitCode = organizationUnitCode;
            if (string.IsNullOrEmpty(filterOrgUnitCode))
            {
                switch (organizationUnitTag)
                {
                    case "PKD":
                        filterOrgUnitCode = DepartmentCode.BusinessDepartmentCode;
                        break;
                    case "CSKH":
                        filterOrgUnitCode = DepartmentCode.CustomerCareDepartmentCode;
                        break;
                    case "BGD":
                        filterOrgUnitCode = DepartmentCode.BoardOfDirectorsCode;
                        break;
                    default:
                        filterOrgUnitCode = organizationUnitCode;
                        break;
                }
            }
            return Ok(_userQueries.GetListTypeSelection(isPartner, filterOrgUnitCode, isNotBelongToOrgUnit));
        }

        [HttpGet("{id}")]
        public ActionResult<IActionResponse<UserDTO>> GetById(int id, bool includePermissions = true)
        {
            var currentUser = _userQueries.FindById(id);
            if (includePermissions)
            {
                var userPermissions = _userQueries.GetPermissionsOfUser(UserIdentity.Id);
                currentUser.Roles = userPermissions.Select(c => c.RoleName).Distinct().ToArray();
                currentUser.Permissions = userPermissions.Select(c => c.PermissionCode).ToArray();
                currentUser.PermissionPages = userPermissions.Select(c => c.PermissionPage).Distinct().ToArray();
            }
            return Ok(currentUser);
        }

        [HttpGet("universal/{id}")]
        public ActionResult<IActionResponse<UserDTO>> GetByUid(string id, bool includePermissions = true)
        {
            var currentUser = _userQueries.FindById(id);
            if (includePermissions)
            {
                var userPermissions = _userQueries.GetPermissionsOfUser(UserIdentity.Id);
                currentUser.Roles = userPermissions.Select(c => c.RoleName).Distinct().ToArray();
                currentUser.Permissions = userPermissions.Select(c => c.PermissionCode).ToArray();
                currentUser.PermissionPages = userPermissions.Select(c => c.PermissionPage).Distinct().ToArray();
            }
            return Ok(currentUser);
        }

        [HttpGet("GetBankById/{id}")]
        public ActionResult<IActionResponse> GetBankById(int id)
        {
            var bankAccounts = _userQueries.GetBankById(id);
            return Ok(bankAccounts);
        }

        [HttpGet("GetCurrentUser")]
        public ActionResult<IActionResponse<UserDTO>> GetCurrentUser()
        {
            var currentUser = _userQueries.FindById(UserIdentity.Id);
            var userPermissions = _userQueries.GetPermissionsOfUser(UserIdentity.Id);
            currentUser.Roles = userPermissions.Select(c => c.RoleName).Distinct().ToArray();
            currentUser.Permissions = userPermissions.Select(c => c.PermissionCode).ToArray();
            currentUser.PermissionPages = userPermissions.Select(c => c.PermissionPage).Distinct().ToArray();
            currentUser.RoleCodes = userPermissions.Select(c => c.RoleCode).Distinct().ToArray();
            return Ok(currentUser);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand createUserCommand)
        {
            var actionResponse = await _mediator.Send(createUserCommand);
            if (actionResponse.IsSuccess)
            {
                var returnValue = _userQueries.FindById(actionResponse.Result);
                var addConfigUserAndSystem = _userService.AddConfigUsers(returnValue.Id);
                if (addConfigUserAndSystem == true)
                {
                    return CreatedAtAction("GetByUid", new { id = actionResponse.Result, version = 1 }, ActionResponse.SuccessWithResult(returnValue));
                }
            }

            return BadRequest(actionResponse);
        }

        [HttpPut("{id}")]
        [PermissionAuthorize("EDIT_USER")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserCommand updateUserCommand)
        {
            var actionResponse = new ActionResponse<UserDTO>();
            if (id != updateUserCommand.Id)
            {
                return BadRequest();
            }
            else
            {
                actionResponse.CombineResponse(await _mediator.Send(updateUserCommand));
                if (actionResponse.IsSuccess)
                {
                    var returnValue = _userQueries.FindById(id);
                    return CreatedAtAction("GetById", new { id }, ActionResponse.SuccessWithResult(returnValue));
                }
                else
                {
                    return BadRequest(actionResponse);
                }
            }
        }

        [HttpPut("UpdateCurrentUser")]
        public async Task<IActionResult> UpdateCurrentUser([FromBody] UpdateUserCommand updateUserCommand)
        {
            var actionResponse = new ActionResponse<UserDTO>();
            if (UserIdentity.Id != updateUserCommand.Id)
            {
                return BadRequest();
            }
            else
            {
                updateUserCommand.IsSelfUpdate = true;
                actionResponse.CombineResponse(await _mediator.Send(updateUserCommand));
                if (actionResponse.IsSuccess)
                {
                    var returnValue = _userQueries.FindById(UserIdentity.Id);
                    return CreatedAtAction("GetById", new { UserIdentity.Id }, ActionResponse.SuccessWithResult(returnValue));
                }
                else
                {
                    return BadRequest(actionResponse);
                }
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!_userService.IsExisted(id)) return BadRequest();

            return Ok(_userService.DeleteAndSave(id));
        }


        [HttpGet]
        [Route("GetAllUserByRoleSelection")]
        public IActionResult GetAllUserByRoleSelection(string sRole)
        {
            return Ok(_userQueries.GetAllUserByRoleSelection(sRole));
        }

        [HttpGet]
        [Route("GetAllUserFilter")]
        public IActionResult GetAllUserFilter(string sRole, string filterValue)
        {
            return Ok(_userQueries.GetAllUserFilter(sRole, filterValue));
        }
        [HttpGet]
        [Route("GetAllSupporters")]
        public IActionResult GetAllSupporters(string keyword)
        {
            return Ok(_userQueries.GetAllUserByDepartment(DepartmentCode.SupporterDepartmentCode, keyword));
        }
    }
}