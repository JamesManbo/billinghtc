using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StaffApp.APIGateway.Models.ContractModels;
using StaffApp.APIGateway.Models.CustomerModels;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContractorsController : CustomBaseController
    {
        private readonly IContractorService _contractorService;
        private readonly IConfiguration _configuration;
        public ContractorsController(IContractorService contractorService, IConfiguration configuration)
        {
            _contractorService = contractorService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<ContractorDTO>>>> GetAsync([FromQuery] ContractorFilterModel filterModel)
        {
            filterModel.ServiceIds = _configuration.GetValue<int>("ServiceIdFTTH").ToString();
            var actResponse = await _contractorService.GetListContactor(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }
        
        [HttpGet("GetListHTC")]
        public async Task<ActionResult<IActionResponse<IPagedList<ContractorDTO>>>> GetListHTC([FromQuery] RequestFilterModel filterModel)
        {
            
            var actResponse = await _contractorService.GetListOnlyContactor(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }
    }
}