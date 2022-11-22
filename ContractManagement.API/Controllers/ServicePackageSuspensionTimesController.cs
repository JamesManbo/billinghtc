using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServicePackageSuspensionTimesController : CustomBaseController
    {
        private readonly IServicePackageSuspensionTimeQueries _servicePackageSuspensionTimeQueries;

        public ServicePackageSuspensionTimesController(IServicePackageSuspensionTimeQueries servicePackageSuspensionTimeQueries)
        {
            _servicePackageSuspensionTimeQueries = servicePackageSuspensionTimeQueries;
        }

        [HttpGet("GetListRestoredByOCSPIds")]
        public IActionResult GetListRestoredByOCSPIdsInTime(string oCSPIds)
        {
            return Ok(_servicePackageSuspensionTimeQueries.GetListRestoredByOCSPIds(oCSPIds.SplitToInt(',')));
        }
    }
}
