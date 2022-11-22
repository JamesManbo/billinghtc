using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using StaffApp.APIGateway.Infrastructure.Helpers;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationUsersController : CustomBaseController
    {
        private readonly IApplicationUsersService _applicationUsersService;
        public ApplicationUsersController(IApplicationUsersService applicationUsersService)
        {
            _applicationUsersService = applicationUsersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] UsersInGroupRequestFilterModel filterModel)
        {
            var result = await _applicationUsersService.GetList(filterModel);
            if (result != null)
            {
                JToken json = JsonHelper.DeserializeWithLowerCasePropertyNames(result);
                return Ok(json);

            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("universal/{uid}")]
        public async Task<IActionResult> GetApplicationUserByUid(string uid)
        {
            var applicationUser = await _applicationUsersService.GetApplicationUserByUid(new StringValue() { Value = uid });

            if (applicationUser == null)
            {
                return NotFound();
            }

            return Ok(JObject.Parse(applicationUser));
        }

        [Route("GenerateUserCode")]
        [HttpGet]
        public async Task<IActionResult> GenerateContractCodeAsync(string groupCodes, string categoryCode, string typeCode)
        {
            return Ok(await _applicationUsersService.GenerateUserCode(groupCodes, categoryCode, typeCode));
        }

    }
}
