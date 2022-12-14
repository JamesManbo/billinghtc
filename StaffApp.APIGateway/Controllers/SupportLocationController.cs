using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.SupportLocationModel;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SupportLocationController : CustomBaseController
    {
        private readonly ISupportLocationService _supportLocationsService;
        public SupportLocationController(ISupportLocationService supportLocationsService)
        {
            _supportLocationsService = supportLocationsService;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<SupportLocationDTO>>>> Get([FromQuery] RequestFilterModel filterModel)
        {
            var actResponse = await _supportLocationsService.GetList(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }
    }
}
