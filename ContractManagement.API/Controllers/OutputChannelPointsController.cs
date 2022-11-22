using ContractManagement.API.PolicyBasedAuthProvider;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.OutputChannelPointRepository;
using Global.Models.Filter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OutputChannelPointsController : CustomBaseController
    {
        private readonly ILogger<BrasInfoController> _logger;
        private readonly IMediator _mediator;
        private readonly IOutputChannelPointRepository _outputChannelPointRepository;
        private readonly IOutputChannelPointQueries _outputChannelPointQueries;

        public OutputChannelPointsController(IOutputChannelPointRepository outputChannelPointRepository,
            IOutputChannelPointQueries outputChannelPointQueries,
            ILogger<BrasInfoController> logger,
            IMediator mediator)
        {
            this._outputChannelPointRepository = outputChannelPointRepository;
            this._outputChannelPointQueries = outputChannelPointQueries;
            this._logger = logger;
            this._mediator = mediator;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] OutputChannelFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_outputChannelPointQueries.GetSelectionList());
            }

            if (filterModel.Type == RequestType.Autocomplete)
            {
                return Ok(_outputChannelPointQueries.Autocomplete(filterModel));
            }

            return Ok(_outputChannelPointQueries.GetList(filterModel));
        }

        // GET: api/RadiusServerInfo/5
        [HttpGet("{id}")]
        public IActionResult GetDetail(int id)
        {
            var equipment = _outputChannelPointQueries.Find(id);

            if (equipment == null)
            {
                return NotFound();
            }

            return Ok(equipment);
        }
    }
}
