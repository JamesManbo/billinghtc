using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApp.APIGateway.Models.UserModels;
using CustomerApp.APIGateway.Services;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : CustomerBaseController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet()]
        public async Task<ActionResult<IActionResponse<UserDTO>>> Get()
        {

            var actResponse = await _userService.FindUserByUId(UserIdentity.UniversalId);
            if (actResponse == null)
            {
                return NotFound();
            }

            return Ok(actResponse);


        }

        [HttpPut()]
        public async Task<IActionResult> PutApplicationUser(AccountCommand applicationUser)
        {
            applicationUser.Id = UserIdentity.Id;

            var actResponse = await _userService.ChangeInfoAccount(applicationUser);
            if (actResponse.IsSuccess)
            {
                return Ok("Thành công");
            }

            return BadRequest(actResponse.Message);

        }
    }
}
