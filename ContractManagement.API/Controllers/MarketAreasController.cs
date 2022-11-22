using ContractManagement.API.Application.Commands.MarketAreaCommandHandler;
using ContractManagement.Domain.Commands.MarketAreaCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.MarketAreaRespository;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MarketAreasController : CustomBaseController
    {
        private readonly ILogger<MarketAreasController> _logger;
        private readonly ContractDbContext _dbContext;
        private readonly IMediator _mediator;
        private readonly IMarketAreaQueries _marketAreaQueries;
        private readonly IMarketAreaRepository _marketAreaRepository;

        public MarketAreasController(
            ILogger<MarketAreasController> logger,
            IMediator mediator,
            IMarketAreaQueries marketAreaQueries,
            IMarketAreaRepository marketAreaRepository,
            ContractDbContext dbContext)
        {
            _logger = logger;
            _mediator = mediator;
            _marketAreaQueries = marketAreaQueries;
            _marketAreaRepository = marketAreaRepository;
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_marketAreaQueries.GetSelectionList(filterModel));
            }

            if (filterModel.Type == RequestType.SimpleAll)
            {
                return Ok(_marketAreaQueries.GetAll(filterModel));
            }

            if (filterModel.Type == RequestType.Hierarchical)
            {
                return Ok(_marketAreaQueries.GetHierarchicalList(filterModel));
            }

            return Ok(_marketAreaQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetMarketArea(int id)
        {
            var marketArea = _marketAreaQueries.Find(id);

            if (marketArea == null)
            {
                return NotFound();
            }

            return Ok(marketArea);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CUMarketAreaCommand createMarketAreaCommand)
        {
            var actResponse = await _mediator.Send(createMarketAreaCommand);
            if (actResponse.IsSuccess)
            {
                return CreatedAtAction("GetMarketArea", new { id = actResponse.Result.Id }, actResponse);
            }

            return BadRequest(actResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CUMarketAreaCommand updateMarketAreaCommand)
        {
            if (id != updateMarketAreaCommand.Id)
            {
                return BadRequest();
            }
            else
            {
                var actionResponse = await _mediator.Send(updateMarketAreaCommand);

                if (actionResponse.IsSuccess)
                {
                    return CreatedAtAction("GetMarketArea", new { id = actionResponse.Result.Id }, actionResponse);
                }

                return BadRequest(actionResponse);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var marketArea = _marketAreaQueries.Find(id);
            if (marketArea == null)
            {
                return NotFound();
            }

            var deleteResponse = _marketAreaRepository.DeleteAndSave(id);

            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }
    }
}