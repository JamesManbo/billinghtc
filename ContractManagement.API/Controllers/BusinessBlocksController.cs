using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.API.Application.Commands.BusinessBlockCommandHandler;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.BusinessBlockRepostory;
using GenericRepository.Configurations;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BusinessBlocksController : CustomBaseController
    {
        private readonly ILogger<BusinessBlocksController> _logger;
        private readonly IMediator _mediator;
        private readonly IBusinessBlockQueries _query;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IBusinessBlockRepostory _businessBlockRepository;

        public BusinessBlocksController(
            ILogger<BusinessBlocksController> logger,
            IMediator mediator,
            IBusinessBlockQueries queryRepository,
            IBusinessBlockRepostory businessBlockRepository,
            IWrappedConfigAndMapper configAndMapper
            )
        {
            _logger = logger;
            _mediator = mediator;
            _query = queryRepository;
            _businessBlockRepository = businessBlockRepository;
            _configAndMapper = configAndMapper;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        {
                return Ok(_query.GetSelectionList());
        }

        [HttpGet("{id}")]
        public IActionResult GetBusinessBlock(int id)
        {
            var businessBlock = _query.Find(id);

            if (businessBlock == null)
            {
                return NotFound();
            }

            return Ok(businessBlock);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BusinessBlockCommand createBusinessBlockCommand)
        {
            var actionResponse = await _mediator.Send(createBusinessBlockCommand);
            if (actionResponse.IsSuccess)
            {
                return CreatedAtAction("GetBusinessBlock", new { id = actionResponse.Result.Id }, actionResponse);
            }

            return BadRequest(actionResponse);
        }
    }
}
