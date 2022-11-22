using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.ServicesModels;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServicesController : CustomBaseController
    {
        private readonly ITelcoService _telcoService;
        public ServicesController(ITelcoService telcoService)
        {
            _telcoService = telcoService;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<ServiceDTO>>>> Get([FromQuery] RequestFilterModel filterModel)
        {
            var actResponse = await _telcoService.GetList(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetServiceAsync(int id)
        {
            var service = await _telcoService.GetDetail(id);

            if (service == null)
            {
                return NotFound();
            }

            return Ok(service);
        }
    }
}