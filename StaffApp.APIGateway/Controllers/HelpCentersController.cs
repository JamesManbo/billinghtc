using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.HelpCenterModels;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HelpCentersController : CustomBaseController
    {
        [HttpGet]
        public ActionResult<IActionResponse<IPagedList<HelpCenterDTO>>> Get([FromQuery] RequestFilterModel filterModel)
        {
            return Ok();
        }
    }
}