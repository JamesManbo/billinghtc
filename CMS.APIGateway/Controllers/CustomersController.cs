using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Protos;
using CMS.APIGateway.Services.Customer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CMS.APIGateway.Controllers
{
    [Route("api/integration/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] RequestFilterGrpcModel filterModel)
        {
            return Ok(await _customerService.GetList(filterModel));
        }
    }
}