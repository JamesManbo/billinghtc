using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.ContractFormModels;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContractFormsController : CustomBaseController
    {
        private readonly IContractFormService _contractFormService;
        public ContractFormsController(IContractFormService contractFormService)
        {
            _contractFormService = contractFormService;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse>> Get([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Autocomplete)
            {
                return Ok(await _contractFormService.Autocomplete(filterModel));
            }

            var actResponse = await _contractFormService.GetList(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }

            return Ok(actResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IActionResponse<ContractFormDTO>>> GetById(int id)
        {
            var actResponse = await _contractFormService.GetById(id);
            if (actResponse == null)
            {
                return NotFound();
            }

            return Ok(actResponse);
        }
    }
}
