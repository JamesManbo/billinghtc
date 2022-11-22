using System.Threading.Tasks;
using ContractManagement.Domain.Commands.ServicePackageCommand;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ServicePackagePriceRepository;
using ContractManagement.Infrastructure.Repositories.ServicePackageRepository;
using Global.Models.Filter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServicePackagesController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly IServicePackageQueries _servicePackagesQueries;
        private readonly IServicePackagePriceQueries _servicePackagePriceQueries;
        private readonly IServicePackagesRepository _servicePackagesRepository;
        private readonly IServicePackagePriceRepository _servicePackagePriceRepository;
        public ServicePackagesController(IMediator mediator,
            IServicePackageQueries servicePackagesQueries,
            IServicePackagePriceQueries servicePackagePriceQueries,
            IServicePackagesRepository servicePackagesRepository,
            IServicePackagePriceRepository servicePackagePriceRepository
            )
        {
            _mediator = mediator;
            _servicePackagesQueries = servicePackagesQueries;
            _servicePackagePriceQueries = servicePackagePriceQueries;
            _servicePackagesRepository = servicePackagesRepository;
            _servicePackagePriceRepository = servicePackagePriceRepository;
        }

        [HttpGet]
        public IActionResult GetServices([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_servicePackagesQueries.GetSelectionList(filterModel));
            }

            if (filterModel.Type == RequestType.SimpleAll)
            {
                return Ok(_servicePackagesQueries.GetAllSimple(filterModel));
            }

            return Ok(_servicePackagesQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetServicePackage(int id)
        {
            var servicePackage = _servicePackagesQueries.Find(id);
            if (servicePackage == null)
            {
                return NotFound();
            }

            return Ok(servicePackage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutServicePackage(int id, [FromBody] CUServicePackageCommand updateServicePackageCommand)
        {
            if (id != updateServicePackageCommand.Id)
            {
                return BadRequest();
            }

            var actionResponse = await _mediator.Send(updateServicePackageCommand);

            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpPost]
        public async Task<IActionResult> PostServicePackage([FromBody] CUServicePackageCommand createServicePackageCommand)
        {
            var actResponse = await _mediator.Send(createServicePackageCommand);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
                //CreatedAtAction("GetMarketArea", new { id = actResponse.Result.Id }, actResponse.Result);
            }
            return BadRequest(actResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServicePackage(int id)
        {
            var servicePackage = _servicePackagesQueries.Find(id);
            if (servicePackage == null)
            {
                return NotFound();
            }

            var deleteResponse = _servicePackagesRepository.DeleteAndSave(id);
            var delPackagePrice = await _servicePackagePriceRepository.DeleteAndSaveByPackageIdAsync(id);

            if (deleteResponse.IsSuccess && delPackagePrice.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }
    }
}