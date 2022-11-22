using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ContractManagement.API.Application.Commands.CurrencyUnitCommandHandler;
using ContractManagement.Domain.Commands.CUCurrencyUnitCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.CurrencyUnitRepository;
using ContractManagement.Infrastructure.Services;
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
    public class CurrencyUnitsController : CustomBaseController
    {
        private readonly ILogger<CurrencyUnitsController> _logger;
        private readonly IMediator _mediator;
        private readonly ContractDbContext _dbContext;
        private readonly ICurrencyUnitRepository _currencyUnitRepository;
        private readonly ICurrencyUnitQueries _currencyUnitQueries;
        private readonly IExchangeRateService _exchangeRateService;
        public CurrencyUnitsController(
            ILogger<CurrencyUnitsController> logger,
            IMediator mediator,
            ContractDbContext dbContext,
            ICurrencyUnitRepository currencyUnitRepository,
            IExchangeRateService exchangeRateService,
            ICurrencyUnitQueries currencyUnitQueries
            )
        {
            _logger = logger;
            _mediator = mediator;
            _dbContext = dbContext;
            _currencyUnitRepository = currencyUnitRepository;
            _exchangeRateService = exchangeRateService;
            _currencyUnitQueries = currencyUnitQueries;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_currencyUnitQueries.GetSelectionList(filterModel));
            }

            if (filterModel.Type == RequestType.Autocomplete)
            {
                return Ok(_currencyUnitQueries.Autocomplete(filterModel));
            }

            if (filterModel.Type == RequestType.SimpleAll)
            {
                return Ok(_currencyUnitQueries.GetAll(filterModel));
            }

            return Ok(_currencyUnitQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var currencyUnit = _currencyUnitQueries.Find(id);

            if (currencyUnit == null)
            {
                return NotFound();
            }

            return Ok(currencyUnit);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCurrencyUnitCommand createCurrencyUnitCommand)
        {
            var createRsp = await _mediator.Send(createCurrencyUnitCommand);
            if (createRsp.IsSuccess)
            {
                return Ok(createRsp);
            }

            return BadRequest(createRsp);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCurrencyUnitCommand updateCurrencyUnitCommand)
        {
            var updateRsp = await _mediator.Send(updateCurrencyUnitCommand);
            if (updateRsp.IsSuccess)
            {
                return Ok(updateRsp);
            }
            return BadRequest(updateRsp);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_currencyUnitRepository.DeleteAndSave(id));
        }

        [HttpGet("GetCurrencyList/{currencyUnitCode}")]
        public IActionResult GetCurrencyList(string currencyUnitCode)
        {
            return Ok(_currencyUnitQueries.GetCurrencyList(currencyUnitCode));
        }

        [HttpGet("GetAllPriceByCurrencyUnit")]
        public IActionResult GetAllPriceByCurrencyUnit([FromQuery] RequestFilterModel filterModel)
        {
            return Ok(_currencyUnitQueries.GetAllPriceByCurrencyUnit(filterModel));
        }

        [HttpGet("ExchangeRate")]
        public IActionResult ExchangeRate(string fromCode, string toCode)
        {
            var rs = _exchangeRateService.ExchangeRate(fromCode, toCode);
            if (rs.HasValue) return Ok(rs.Value);

            return BadRequest();
        }

        [HttpGet("ExchangeRates")]
        public IActionResult ExchangeRates(string currencyUnitCodes)
        {
            var rs = _exchangeRateService.ExchangeRates(currencyUnitCodes);
            if (rs != null) return Ok(rs);
            return BadRequest();
        }

        [HttpGet("GetAllExchangeRate")]
        public IActionResult GetAllExchangeRate()
        {
            return Ok(_exchangeRateService.GetAllExchangeRate());
        }
    }
}
