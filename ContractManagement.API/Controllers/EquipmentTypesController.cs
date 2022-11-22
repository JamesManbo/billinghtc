using System;
using System.Threading.Tasks;
using ContractManagement.API.Application.Commands.EquipmentTypeCommandHandler;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure;
using ContractManagement.Domain.Commands.EquipmentTypeCommand;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.EquipmentPictureRepository;
using ContractManagement.Infrastructure.Repositories.EquipmentTypeRepository;
using ContractManagement.Infrastructure.Repositories.PictureRepository;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EquipmentTypesController : CustomBaseController
    {
        private readonly ILogger<EquipmentTypesController> _logger;
        private readonly IMediator _mediator;
        private readonly IEquipmentTypeQueries _equipmentTypeQueries;
        //private readonly IStaticResourceService _staticResourceService;
        private readonly IEquipmentTypeRepository _equipmentTypeRepository;

        public EquipmentTypesController(
            ILogger<EquipmentTypesController> logger,
            IMediator mediator,
            IEquipmentTypeQueries queries,
            IEquipmentTypeRepository equipmentTypeRepository
            )
        {
            _logger = logger;
            _mediator = mediator;
            _equipmentTypeQueries = queries;
            _equipmentTypeRepository = equipmentTypeRepository;

        }

        [HttpGet("autocomplete-instance")]
        public IActionResult AutocompleteInstance([FromQuery] RequestFilterModel filterModel)
        {
            return Ok(this._equipmentTypeQueries.AutocompleteInstance(filterModel));
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_equipmentTypeQueries.GetSelectionList(filterModel));
            }

            if (filterModel.Type == RequestType.Autocomplete)
            {
                return Ok(_equipmentTypeQueries.Autocomplete(filterModel));
            }

            if (filterModel.Type == RequestType.SimpleAll)
            {
                return Ok(_equipmentTypeQueries.GetAll(filterModel));
            }

            return Ok(_equipmentTypeQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var equipment = _equipmentTypeQueries.Find(id);

            if (equipment == null)
            {
                return NotFound();
            }

            return Ok(equipment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateEquipmentTypeCommand createEquipmentTypeCommand)
        {
            var actionResponse = await _mediator.Send(createEquipmentTypeCommand);
            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEquipmentTypeCommand updateEquipmentTypeCommand)
        {
            if (id != updateEquipmentTypeCommand.Id)
            {
                return BadRequest();
            }

            var actionResponse = await _mediator.Send(updateEquipmentTypeCommand);
            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_equipmentTypeRepository.DeleteAndSave(id));
        }
    }
}