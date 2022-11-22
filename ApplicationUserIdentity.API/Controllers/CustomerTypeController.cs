using ApplicationUserIdentity.API.Application.Commands.CustomerType;
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
    public class CustomerTypeController : CustomBaseController
    {
        private readonly ICustomerTypeQueries _customerTypeQueries;
        private readonly ICustomerTypeRepository _customerTypeRepository;
        private readonly IMediator _mediator;
        public CustomerTypeController(ICustomerTypeQueries customerTypeQueries, IMediator mediator, ICustomerTypeRepository customerTypeRepository)
        {
            _customerTypeQueries = customerTypeQueries;
            _customerTypeRepository = customerTypeRepository;
            _mediator = mediator;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CustomerTypeDTO>> GetCustomerTypes(
           [FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
                return Ok(_customerTypeQueries.GetSelectionList());

            return Ok(_customerTypeQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<CustomerTypeDTO>> GetCustomerType(int id)
        {
            var CustomerType = _customerTypeQueries.Find((int)id);

            if (CustomerType == null)
            {
                return NotFound();
            }

            return Ok(CustomerType);
        }

        [HttpPost]
        public async Task<ActionResult> PostCustomerTypeAsync([FromBody] CustomerTypeCommand command)
        {
            var validator = new CreateCustomerTypeValidator();
            var validateResult = await validator.ValidateAsync(command);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            ActionResponse<CustomerTypeDTO> rs = new ActionResponse<CustomerTypeDTO>();
            var existName = _customerTypeQueries.CheckExistName(command.Id, command.Name);
            var existCode = _customerTypeQueries.CheckExistCode(command.Id, command.Code);

            if (existName)
            {
                rs.AddError("Tên kiểu khách hàng đã tồn tại", nameof(command.Name));
            }

            if (existCode)
            {
                rs.AddError("Mã kiểu khách hàng đã tồn tại", nameof(command.Code));
            }

            if (existName || existCode)
                return BadRequest(rs);

            command.CreatedBy = UserIdentity.UserName;
            var actResponse = await _customerTypeRepository.CreateAndSave(command);
            if (actResponse.IsSuccess)
            {
                return CreatedAtAction("GetCustomerType", new { id = actResponse.Result.Id }, actResponse);
            }
            return BadRequest(actResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCustomerTypeAsync(int id, [FromBody] CustomerTypeCommand updateCommand)
        {
            if (id != updateCommand.Id)
            {
                return BadRequest();
            }

            var validator = new CreateCustomerTypeValidator();
            var validateResult = await validator.ValidateAsync(updateCommand);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }


            ActionResponse<CustomerTypeDTO> rs = new ActionResponse<CustomerTypeDTO>();
            var existName = _customerTypeQueries.CheckExistName(updateCommand.Id, updateCommand.Name);
            var existCode = _customerTypeQueries.CheckExistCode(updateCommand.Id, updateCommand.Code);
            if (existName)
            {
                rs.AddError("Tên kiểu khách hàng đã tồn tại", nameof(updateCommand.Name));
            }
            if (existCode)
            {
                rs.AddError("Mã kiểu khách hàng đã tồn tại", nameof(updateCommand.Code));
            }
            if (existName || existCode)
                return BadRequest(rs);
            updateCommand.UpdatedBy = UserIdentity.UserName;
            var actionResponse = await _mediator.Send(updateCommand);

            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var customerType = _customerTypeQueries.Find(id);
            if (customerType == null)
            {
                return NotFound();
            }

            var deleteResponse = _customerTypeRepository.DeleteAndSave(id);


            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }
    }
}
