using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.API.Application.Commands.ServicePackagePriceCommandHandler;
using ContractManagement.Domain.AggregatesModel.TaxAggreagate;
using ContractManagement.Domain.Commands.ServicePackagePriceCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Validations;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories;
using ContractManagement.Infrastructure.Repositories.ServicePackagePriceRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServicePackagePriceController : CustomBaseController
    {
        private readonly IServicePackagePriceQueries _servicePackagePriceQueries;
        private readonly IServicePackagePriceRepository _servicePackagePriceRepository;
        public ServicePackagePriceController(IServicePackagePriceQueries servicePackagePriceQueries, IServicePackagePriceRepository servicePackagePriceRepository)
        {
            _servicePackagePriceQueries = servicePackagePriceQueries;
            _servicePackagePriceRepository = servicePackagePriceRepository;
        }

        [HttpGet]
        public ActionResult<IPagedList<ServicePackagePriceDTO>> GetServices([FromQuery] RequestFilterModel filterModel)
        {
            return Ok(_servicePackagePriceQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetServicePackagePrice(int id)
        {
            var servicePackage = _servicePackagePriceQueries.Find(id);

            if (servicePackage == null)
            {
                return NotFound();
            }

            return Ok(servicePackage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicePackagePrice(int id, ServicePackagePriceDTO servicePackagePrice)
        {
            if (id != servicePackagePrice.Id)
            {
                return BadRequest();
            }
            var validator = new Domain.Validations.ServicePackagePriceValidator();
            var validateResult = await validator.ValidateAsync(servicePackagePrice);
            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            var actionResponse = await _servicePackagePriceRepository.UpdateAndSave(servicePackagePrice);
            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpPost]
        public async Task<IActionResult> PostServicePackagePrice([FromBody] CUServicePackagePriceCommand servicePriceCommand)
        {
            servicePriceCommand.CreatedBy = UserIdentity.UserName;
            servicePriceCommand.UpdatedBy = UserIdentity.UserName;
            servicePriceCommand.TaxCategoryId = TaxCategory.VAT.Id;
            servicePriceCommand.PriceBeforeTax = servicePriceCommand.PriceValue / (((100 + int.Parse(TaxCategory.VAT.TaxValue.ToString())) / 100));
            var validator = new Application.Commands.ServicePackagePriceCommandHandler.ServicePackagePriceValidator();
            var validateResult = await validator.ValidateAsync(servicePriceCommand);
            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            var actionResponse = await _servicePackagePriceRepository.CreateAndSave(servicePriceCommand);
            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);// CreatedAtAction("GetServicePackagePrice", new { id = actionResponse.Result.Id }, actionResponse);
            }

            return BadRequest(actionResponse);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteServicePackagePrice(int id)
        {
            var servicePackagePrice = _servicePackagePriceQueries.Find(id);
            if (servicePackagePrice == null)
            {
                return NotFound();
            }

            var deleteResponse = _servicePackagePriceRepository.DeleteAndSave(id);

            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }

    }
}