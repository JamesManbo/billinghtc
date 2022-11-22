using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Filter;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StaffApp.APIGateway.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrganizationUnitsController : CustomBaseController
    {

        private readonly IOrganizationUnitsService _organizationUnitsService;
        public OrganizationUnitsController(IOrganizationUnitsService organizationUnitsService)
        {
            _organizationUnitsService = organizationUnitsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] RequestFilterModel filterModel)
        {
            var result = await _organizationUnitsService.GetList(filterModel);
            if (result != null)
            {
                return Ok(JObject.Parse(result));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
