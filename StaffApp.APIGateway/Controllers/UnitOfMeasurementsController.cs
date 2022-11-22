using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.CommonModels;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UnitOfMeasurementsController : CustomBaseController
    {
        private readonly IUnitOfMeasurementService _unitOfMeasurementService;
        public UnitOfMeasurementsController(IUnitOfMeasurementService telcoService)
        {
            _unitOfMeasurementService = telcoService;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IEnumerable<SelectionItemDTO>>>> Get([FromQuery] UnitOfMeasurementFilterModel filterModel)
        {
            var actResponse = await _unitOfMeasurementService.GetSelectList(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }
    }
}
