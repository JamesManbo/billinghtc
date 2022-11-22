using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApp.APIGateway.Models;
using CustomerApp.APIGateway.Services;
using Global.Configs.Authentication;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServicesController : CustomerBaseController
    {
        private readonly ITelService _service;
        public ServicesController(ITelService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<ServiceDTO>>>> GetServices([FromQuery] RequestFilterModel filterModel)
        {

            var actResponse = await _service.GetList(filterModel);
            if (actResponse==null)
            {
                return NotFound();
            }

           return Ok(actResponse);


        }
    }
}