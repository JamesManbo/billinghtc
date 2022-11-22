using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Infrastructure.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProjectTechnicianController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly IProjectTechnicianQueries _transactionQueries;
        public ProjectTechnicianController(IMediator mediator, IProjectTechnicianQueries transactionQueries)
        {
            _mediator = mediator;
            _transactionQueries = transactionQueries;

        }

        [HttpGet]
        [Route("GetCurrentPendingTaskBySupporter")]
        public IActionResult GetCurrentPendingTaskBySupporter(int marketId, DateTime startDate, DateTime endDate)
        {
            return Ok(_transactionQueries.GetCurrentPendingTaskBySupporter(marketId, startDate, endDate));
        }
    }
}
