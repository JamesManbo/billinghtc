using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.FilterModels.ReportsModel;
using ContractManagement.Infrastructure.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupporterProjectFtthController : CustomBaseController
    {
        private readonly ILogger<SupporterProjectFtthController> _logger;
        private readonly IMediator _mediator;
        private readonly ISuppporterProjectFtthQueries _suppporterProjectFtthQueries;

        public SupporterProjectFtthController(ILogger<SupporterProjectFtthController> logger,
            IMediator mediator,
            ISuppporterProjectFtthQueries suppporterProjectFtthQueries)
        {
            _logger = logger;
            _mediator = mediator;
            _suppporterProjectFtthQueries = suppporterProjectFtthQueries;
        }

       

        [HttpGet]
        [Route("GetTotalRetailAndBussinessCustomers")]
        public IActionResult GetTotalRetailAndBussinessCustomers([FromQuery] FromAndToYearMonthFilter filter)
        {
            return Ok(_suppporterProjectFtthQueries.GetTotalRetailAndBussinessCustomers(filter));
        }

        [HttpGet]
        [Route("GetTotalRetailAndBussinessEquipments")]
        public IActionResult GetTotalRetailAndBussinessEquipments(DateTime startDate, DateTime endDate)
        {
            return Ok(_suppporterProjectFtthQueries.GetTotalRetailAndBussinessEquipments(startDate, endDate));
        }
    }
}
