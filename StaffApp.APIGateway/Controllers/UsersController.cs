using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OrganizationUnit.API.Protos.Users;
using StaffApp.APIGateway.Models.AuthModels;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : CustomBaseController
    {

        private readonly IUsersService _usersService;
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] UserFilterModel filterModel)
        {
            var result = await _usersService.GetList(filterModel);
            if (result != null)
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                var rs = JsonConvert.DeserializeObject(result, serializerSettings);


                return Ok(rs);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetListTypeSelection")]
        public async Task<IActionResult> GetListTypeSelection(bool isPartner)
        {
            var result = await _usersService.GetListTypeSelection(isPartner);
            if (result != null)
            {
                return Ok(JArray.Parse(result));
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("GetInfo")]
        public async Task<IActionResult> GetInfo()
        {
            var user = await _usersService.GetInfo(UserIdentity.UniversalId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(JObject.Parse(user));
        }

        [HttpPut]
        [Route("ChangeSettingAccount")]
        public async Task<ActionResult<IActionResponse>> ChangeSettingAccount(SettingAccountCommandGrpc command)
        {
            command.IdentityGuid = UserIdentity.UniversalId;
            command.UserId = UserIdentity.Id;
            var result = await _usersService.ChangeSetting(command);
            if (result != null && result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
