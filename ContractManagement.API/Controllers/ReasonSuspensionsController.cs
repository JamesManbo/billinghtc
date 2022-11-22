using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Infrastructure.Queries;
using Global.Models.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReasonSuspensionsController : CustomBaseController
    {
        private readonly ILogger<ReasonSuspensionsController> _logger;
        private readonly IReasonSuspensionQueries _reasonSuspensionQueries;

        public ReasonSuspensionsController(ILogger<ReasonSuspensionsController> logger,
            IReasonSuspensionQueries reasonSuspensionQueries)
        {
            _logger = logger;
            _reasonSuspensionQueries = reasonSuspensionQueries;
        }

        [HttpGet]
        public IActionResult GetReasonSuspensions([FromQuery] RequestFilterModel requestFilterModel)
        {
            if (requestFilterModel.Type == RequestType.Selection)
            {
                return Ok(_reasonSuspensionQueries.GetSelectionList(requestFilterModel));
            }

            return Ok(_reasonSuspensionQueries.GetSelectionList(requestFilterModel));
        }
    }
}