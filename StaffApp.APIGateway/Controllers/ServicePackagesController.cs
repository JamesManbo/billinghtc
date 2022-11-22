using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Models.ServicePackageModels;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServicePackagesController : CustomBaseController
    {
        private readonly IPackageService _packageService;
        public ServicePackagesController(IPackageService telcoService)
        {
            _packageService = telcoService;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<ServicePackageDTO>>>> Get(
            [FromQuery] PackageFilterModel filterModel)
        {
            var actResponse = await _packageService.GetList(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }
    }
}