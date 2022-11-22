using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.AggregatesModel.TaxAggreagate;
using ContractManagement.Domain.Commands.ServicePackagePriceCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Validations;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories;
using ContractManagement.Infrastructure.Repositories.ServicePackagePriceRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServicePackageProjectPriceController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly IServicePackagePriceQueries _servicePackagePriceQueries;
        private readonly IServicePackagePriceRepository _servicePackagePriceRepository;
        public ServicePackageProjectPriceController(IServicePackagePriceQueries servicePackagePriceQueries
                                                    , IServicePackagePriceRepository servicePackagePriceRepository
                                                    , IMediator mediator)
        {
            _servicePackagePriceQueries = servicePackagePriceQueries;
            _servicePackagePriceRepository = servicePackagePriceRepository;
            _mediator = mediator;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicePackagePrice(int id,[FromBody] CUServicePackagePriceCommand servicePriceCommand)
        {
            if (id != servicePriceCommand.Id)
            {
                return BadRequest();
            }

            var actResponse = await _mediator.Send(servicePriceCommand);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CUServicePackagePriceCommand servicePriceCommand)
        {
            servicePriceCommand.CreatedBy = UserIdentity.UserName;
            servicePriceCommand.UpdatedBy = UserIdentity.UserName;
            servicePriceCommand.TaxCategoryId = TaxCategory.VAT.Id;
            servicePriceCommand.PriceBeforeTax = servicePriceCommand.PriceValue / (((100+ int.Parse(TaxCategory.VAT.TaxValue.ToString()))/100));
            var actResponse = await _mediator.Send(servicePriceCommand);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse); 
            }
            else
            {
                return BadRequest(actResponse);
            }
            
        }
    }
}