using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OutputChannelPointsController : CustomBaseController
    {
        private readonly IOutContractService _outContractService;
        public OutputChannelPointsController(IOutContractService outContractService)
        {
            _outContractService = outContractService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] OutputChannelFilterModel filterModel)
        {
            //if (filterModel.Type == RequestType.Selection)
            //{
            //     return Ok(_outputChannelPointQueries.GetSelectionList());
            // }

            // if (filterModel.Type == RequestType.Autocomplete)
            //{
                return Ok(await _outContractService.AutocompleteOutputChannelPointAsync(filterModel));
           // }

            //return Ok(_outputChannelPointQueries.GetList(filterModel));
        }
    }
}
