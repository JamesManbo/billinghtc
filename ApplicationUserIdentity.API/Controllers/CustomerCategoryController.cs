using ApplicationUserIdentity.API.Application.Commands.CustomerCategory;
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
    public class CustomerCategoryController : CustomBaseController
    {
        private readonly ICustomerCategoryQueries _customerCategoryQueries;
        private readonly ICustomerCategoryRepository _customerCategoryRepository;
        private readonly IMediator _mediator;
        public CustomerCategoryController(ICustomerCategoryQueries customerCategoryQueries, IMediator mediator, ICustomerCategoryRepository customerCategoryRepository)
        {
            _customerCategoryQueries = customerCategoryQueries;
            _customerCategoryRepository = customerCategoryRepository;
            _mediator = mediator;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CustomerCategoryDTO>> GetCustomerCategories(
           [FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
                return Ok(_customerCategoryQueries.GetSelectionList());

            return Ok(_customerCategoryQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public ActionResult<IEnumerable<CustomerCategoryDTO>> GetCustomerCategory(int id)
        {
            var customerCategory = _customerCategoryQueries.Find((int)id);

            if (customerCategory == null)
            {
                return NotFound();
            }

            return Ok(customerCategory);
        }

        [HttpPost]
        public async Task<ActionResult> PostCustomerCategoryAsync([FromBody] CustomerCategoryCommand command)
        {
            var validator = new CreateCustomerCategoryValidator();
            var validateResult = await validator.ValidateAsync(command);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            ActionResponse<CustomerCategoryDTO> rs = new ActionResponse<CustomerCategoryDTO>();
            var existName = _customerCategoryQueries.CheckExistName(command.Id, command.Name);
            var existCode = _customerCategoryQueries.CheckExistCode(command.Id, command.Code);
           
            if (existName)
            {
                rs.AddError("Tên danh mục khách hàng đã tồn tại", nameof(command.Name));
            }
            if (existCode)
            {
                rs.AddError("Mã danh mục khách hàng đã tồn tại", nameof(command.Code));
            }
            if (existName || existCode)
                return BadRequest(rs);

            command.CreatedBy = UserIdentity.UserName;
            var actResponse = await _customerCategoryRepository.CreateAndSave(command);
            if (actResponse.IsSuccess)
            {
                return CreatedAtAction("GetCustomerCategory", new { id = actResponse.Result.Id }, actResponse);
            }
            return BadRequest(actResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> PutCustomerCategoryAsync(int id, [FromBody] CustomerCategoryCommand updateCommand)
        {
            if (id != updateCommand.Id)
            {
                return BadRequest();
            }

            var validator = new CreateCustomerCategoryValidator();
            var validateResult = await validator.ValidateAsync(updateCommand);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            ActionResponse<CustomerCategoryDTO> rs = new ActionResponse<CustomerCategoryDTO>();
            var existName = _customerCategoryQueries.CheckExistName(updateCommand.Id, updateCommand.Name);
            var existCode = _customerCategoryQueries.CheckExistCode(updateCommand.Id, updateCommand.Code);
            if (existName)
            {
                rs.AddError("Tên danh mục khách hàng đã tồn tại", nameof(updateCommand.Name));
            }
            if (existCode)
            {
                rs.AddError("Mã danh mục khách hàng đã tồn tại", nameof(updateCommand.Code));
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
            var customerCategory = _customerCategoryQueries.Find(id);
            if (customerCategory == null)
            {
                return NotFound();
            }

            var deleteResponse = _customerCategoryRepository.DeleteAndSave(id);


            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }
    }
}
