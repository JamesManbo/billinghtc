using ApplicationUserIdentity.API.Application.Commands.CustomerStructure;
using ApplicationUserIdentity.API.Infrastructure.Queries;
using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Models.CustomerViewModels;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerStructureController : CustomBaseController
    {
        private readonly ICustomerStructureQueries _customerStructureQueries;
        private readonly ICustomerStructureRepository _customerStructureRepository;
        private readonly IMediator _mediator;
        public CustomerStructureController(ICustomerStructureQueries customerStructureQueries, IMediator mediator, ICustomerStructureRepository customerStructureRepository)
        {
            _customerStructureQueries = customerStructureQueries;
            _customerStructureRepository = customerStructureRepository;
            _mediator = mediator;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CustomerStructureDTO>> GetCustomerStructures(
           [FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
                return Ok(_customerStructureQueries.GetSelectionList());

            return Ok(_customerStructureQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<CustomerStructureDTO>> GetCustomerStructure(int id)
        {
            var customerStructure = _customerStructureQueries.Find((int)id);

            if (customerStructure == null)
            {
                return NotFound();
            }

            return Ok(customerStructure);
        }

        [HttpPost]
        public async Task<ActionResult> PostCustomerStructureAsync([FromBody] CustomerStructureCommand command)
        {
            var validator = new CreateCustomerStructureValidator();
            var validateResult = await validator.ValidateAsync(command);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            ActionResponse<CustomerStructureDTO> rs = new ActionResponse<CustomerStructureDTO>();
            var existName = _customerStructureQueries.CheckExistName(command.Id, command.Name);
            var existCode = _customerStructureQueries.CheckExistCode(command.Id, command.Code);
            if (existName)
            {
                rs.AddError("Tên cơ cấu khách hàng đã tồn tại", nameof(command.Name));
            }
            if (existCode)
            {
                rs.AddError("Mã cơ cấu khách hàng đã tồn tại", nameof(command.Code));
            }
            if (existName || existCode)
                return BadRequest(rs);
            command.CreatedBy = UserIdentity.UserName;
            var actResponse = await _customerStructureRepository.CreateAndSave(command);
            if (actResponse.IsSuccess)
            {
                return CreatedAtAction("GetCustomerStructure", new { id = actResponse.Result.Id }, actResponse);
            }
            return BadRequest(actResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCustomerStructureAsync(int id, [FromBody] CustomerStructureCommand updateCommand)
        {
            if (id != updateCommand.Id)
            {
                return BadRequest();
            }

            var validator = new CreateCustomerStructureValidator();
            var validateResult = await validator.ValidateAsync(updateCommand);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }


            ActionResponse<CustomerStructureDTO> rs = new ActionResponse<CustomerStructureDTO>();
            var existName = _customerStructureQueries.CheckExistName(updateCommand.Id, updateCommand.Name);
            var existCode = _customerStructureQueries.CheckExistCode(updateCommand.Id, updateCommand.Code);
            if (existName)
            {
                rs.AddError("Tên cơ cấu khách hàng đã tồn tại", nameof(updateCommand.Name));
            }
            if (existCode)
            {
                rs.AddError("Mã cơ cấu khách hàng đã tồn tại", nameof(updateCommand.Code));
            }
            if (existName || existCode)
                return BadRequest(rs);
            updateCommand.UpdatedBy = UserIdentity.UserName;
            var actionResponse = await _customerStructureRepository.UpdateAndSave(updateCommand);

            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var customerStructure = _customerStructureQueries.Find(id);
            if (customerStructure == null)
            {
                return NotFound();
            }

            var deleteResponse = _customerStructureRepository.DeleteAndSave(id);


            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }
    }
}
