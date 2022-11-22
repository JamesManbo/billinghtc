using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.CustomerModels;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : CustomBaseController
    {
        private readonly ICustomerService _service;
        public CustomerController(ICustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<CustomerDTO>>>> GetCustomers([FromQuery] RequestFilterModel filterModel)
        {
            filterModel.Filters = "isEnterprise::false::eq";
            var actResponse = await _service.GetListCustomer(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }

        [HttpPost]
        public async Task<IActionResult> PostApplicationUser(CCustomerCommandGrpc applicationUser)
        {
            applicationUser.CreatedBy = UserIdentity.UniversalId;

            var actResponse = await _service.CreateCustomer(applicationUser);
            if (actResponse == null) return BadRequest("");
            if (actResponse.IsSuccess) return Ok(actResponse.CustomerModel);
                
            return BadRequest(actResponse.Message);

        }

        [HttpGet]
        [Route("GetCustomerClass")]
        public async Task<ActionResult<IActionResponse<IEnumerable<CustomerClassDtoGrpc>>>> GetCustomerClassAsync()
        {
            var actResponse = await _service.GetCustomerClass();
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }
        
        [HttpGet]
        [Route("GetCustomerCategory")]
        public async Task<ActionResult<IActionResponse<IEnumerable<CustomerCategoryDtoGrpc>>>> GetCustomerCategory()
        {
            var actResponse = await _service.GetCustomerCategory();
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }
        
        [HttpGet]
        [Route("GetCustomerGroup")]
        public async Task<ActionResult<IActionResponse<IEnumerable<CustomerGroupDtoGrpc>>>> GetCustomerGroup()
        {
            var actResponse = await _service.GetCustomerGroup();
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }
        
        [HttpGet]
        [Route("GetCustomerType")]
        public async Task<ActionResult<IActionResponse<IEnumerable<CustomerTypeDtoGrpc>>>> GetCustomerType()
        {
            var actResponse = await _service.GetCustomerType();
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }

        [HttpGet]
        [Route("GetCustomerStructure")]
        public async Task<ActionResult<IActionResponse<IEnumerable<CustomerStructDtoGrpc>>>> GetCustomerStruct()
        {
            var actResponse = await _service.GetCustomerStruct();
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }
        
        [HttpGet]
        [Route("GetIndustries")]
        public async Task<ActionResult<IActionResponse<IEnumerable<IndustryDto>>>> GetIndustries()
        {
            var actResponse = await _service.GetIndustries();
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }
    }
}