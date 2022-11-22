using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApp.APIGateway.Models.PackageModels;
using CustomerApp.APIGateway.Models.RequestModels;
using CustomerApp.APIGateway.Services;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PackagesController : CustomerBaseController
    {
        private readonly IPackageService _packageService;
        public PackagesController(IPackageService packageService)
        {
            _packageService = packageService;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<PackageDTO>>>> Get(
            [FromQuery] PackageFilterModel filterModel)
        {
            filterModel.OnlyRoot = true;
            var actResponse = await _packageService.GetList(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }
    }
}
