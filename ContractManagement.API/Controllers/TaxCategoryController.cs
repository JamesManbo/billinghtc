using System.Threading.Tasks;
using ContractManagement.Domain.Commands.TaxCategoryCommand;
using ContractManagement.Infrastructure.Queries;
using GenericRepository.Configurations;
using MediatR;
using Global.Models.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GenericRepository.Extensions;
using Microsoft.AspNetCore.Authorization;
using Global.Models.StateChangedResponse;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Repositories.TaxCatagoriesRepository;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaxCategoryController : CustomBaseController
    {
        private readonly ILogger<TaxCategoryController> _logger;
        private readonly IMediator _mediator;
        private readonly ITaxCategoryQueries _taxQueries;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly ITaxCategoryRepositoty _taxCategoryRepository;

        public TaxCategoryController(
            ILogger<TaxCategoryController> logger,
            IMediator mediator,
            ITaxCategoryQueries taxQueries,
            ITaxCategoryRepositoty taxCategoryRepository,
            IWrappedConfigAndMapper configAndMapper)
        {
            _logger = logger;
            _mediator = mediator;
            _taxQueries = taxQueries;
            _taxCategoryRepository = taxCategoryRepository;
            _configAndMapper = configAndMapper;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        {

            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_taxQueries.GetSelectionList(filterModel));
            }

            if (filterModel.Type == RequestType.SimpleAll)
            {
                return Ok(_taxQueries.GetAll(filterModel));
            }

            return Ok(_taxQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetTaxCategory(int id)
        {
            var taxCategory = _taxQueries.Find(id);

            if (taxCategory == null)
            {
                return NotFound();
            }

            return Ok(taxCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CUTaxCategoryCommand createTaxCategoryCommand)
        {
            var actionResponse = await _mediator.Send(createTaxCategoryCommand);
            if (actionResponse.IsSuccess)
            {
                return CreatedAtAction("GetTaxCategory", new { id = actionResponse.Result.Id }, actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CUTaxCategoryCommand updateTaxCategoryCommand)
        {
            if (id != updateTaxCategoryCommand.Id)
            {
                return BadRequest();
            }

            var actionResponse = await _mediator.Send(updateTaxCategoryCommand);
            if (actionResponse.IsSuccess)
            {
                return CreatedAtAction("GetTaxCategory", new { id = actionResponse.Result.Id }, actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public IActionResult Delete(int id)
        {
            return Ok(_taxCategoryRepository.DeleteAndSave(id));
        }
    }
}