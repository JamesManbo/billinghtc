using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.API.Application.Services.Radius;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.RadiusDomain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RadiusServicesController : CustomBaseController
    {

        private readonly IRadiusAndBrasManagementService _radiusManagementService;

        public RadiusServicesController(
            IRadiusAndBrasManagementService radiusMngService)
        {
            _radiusManagementService = radiusMngService;
        }

        // GET: api/RadiusServices
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _radiusManagementService.GetRadiusServiceByAllServer());
        }
    }
}
