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
    public class ReasonTypeContractTerminationsController : CustomBaseController
    {
        private readonly ILogger<ReasonTypeContractTerminationsController> _logger;
        private readonly IReasonTypeContractTerminationQueries _reasonTypeContractTerminationQueries;

        public ReasonTypeContractTerminationsController(ILogger<ReasonTypeContractTerminationsController> logger,
            IReasonTypeContractTerminationQueries reasonTypeContractTerminationQueries)
        {
            _logger = logger;
            _reasonTypeContractTerminationQueries = reasonTypeContractTerminationQueries;
        }

        [HttpGet]
        public IActionResult GetReasonSuspensions([FromQuery] RequestFilterModel requestFilterModel)
        {
            if (requestFilterModel.Type == RequestType.Selection)
            {
                return Ok(_reasonTypeContractTerminationQueries.GetSelectionList(requestFilterModel));
            }

            return Ok(_reasonTypeContractTerminationQueries.GetSelectionList(requestFilterModel));
        }
    }
}