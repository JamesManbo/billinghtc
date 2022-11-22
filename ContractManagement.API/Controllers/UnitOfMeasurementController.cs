using System;
using System.Threading.Tasks;
using ContractManagement.API.Application.Commands.UnitOfMeasurementCommand;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.UnitOfMeasurementRepository;
using GenericRepository.Configurations;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UnitOfMeasurementController : CustomBaseController
    {
        private readonly ILogger<UnitOfMeasurementController> _logger;
        private readonly IMediator _mediator;
        private readonly IUnitOfMeasurementQueries _query;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IUnitOfMeasurementRepository _unitOfMeasurementRepository;

        public UnitOfMeasurementController(
            ILogger<UnitOfMeasurementController> logger,
            IMediator mediator,
            IUnitOfMeasurementQueries queryRepository,
            IUnitOfMeasurementRepository unitOfMeasurementRepository,
            IWrappedConfigAndMapper configAndMapper
            )
        {
            _logger = logger;
            _mediator = mediator;
            _query = queryRepository;
            _unitOfMeasurementRepository = unitOfMeasurementRepository;
            _configAndMapper = configAndMapper;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] UnitOfMeasurementFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_query.GetSelectionList(filterModel));
            }
            return Ok(_query.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetUnitOfMeasurement(int id)
        {
            var unitOfMeasurement = _query.Find(id);

            if (unitOfMeasurement == null)
            {
                return NotFound();
            }

            return Ok(unitOfMeasurement);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CUUnitOfMeasurementCommand createUnitOfMeasurementCommand)
        {
            createUnitOfMeasurementCommand.CreatedDate = DateTime.Now;
            createUnitOfMeasurementCommand.CreatedBy = UserIdentity.UserName;
            var valueActionResponse = await _mediator.Send(createUnitOfMeasurementCommand);
            if (valueActionResponse.IsSuccess)
            {
                return CreatedAtAction("GetUnitOfMeasurement", new { id = valueActionResponse.Result.Id }, valueActionResponse);
            }

            return BadRequest(valueActionResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CUUnitOfMeasurementCommand updateUnitOfMeasurementCommand)
        {
            if (id != updateUnitOfMeasurementCommand.Id)
            {
                return BadRequest();
            }
            else
            {
                updateUnitOfMeasurementCommand.UpdatedDate = DateTime.Now;
                updateUnitOfMeasurementCommand.UpdatedBy = UserIdentity.UserName;
                var actionResponse = await _mediator.Send(updateUnitOfMeasurementCommand);
                if (actionResponse.IsSuccess)
                {
                    return Ok(actionResponse);
                }

                return BadRequest(actionResponse);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_unitOfMeasurementRepository.DeleteAndSave(id));
        }

        [HttpGet("GetSelectionListUOM")]
        public IActionResult GetSelectionListUOM(string label)
        {
            return Ok(_query.GetSelectionListUOM(label));
        }
    }
}