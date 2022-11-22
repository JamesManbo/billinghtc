using ContractManagement.Domain.FilterModels.ReportsModel;
using ContractManagement.Infrastructure.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContractEquipmentsController : CustomBaseController
    {
        private readonly ILogger<ContractEquipmentsController> _logger;
        private readonly IMediator _mediator;
        private readonly IContractEquipmentQueries _contractEquipmentQueries;
        private readonly IReportsQueries _reportsQueries;

        public ContractEquipmentsController(ILogger<ContractEquipmentsController> logger,
            IMediator mediator,
            IContractEquipmentQueries contractEquipmentQueries,
            IReportsQueries reportsQueries)
        {
            _logger = logger;
            _mediator = mediator;
            _contractEquipmentQueries = contractEquipmentQueries;
            _reportsQueries = reportsQueries;
        }

        [HttpGet("GetAllByOutContractId/{ids}")]
        public IActionResult GetAllByOutContractId(string ids)
        {
            return Ok(_contractEquipmentQueries.GetAllByOutContractId(ids));
        }

        [HttpGet("GetAllHasToReclaimByOutContractId/{ids}")]
        public IActionResult GetAllHasToReclaimByOutContractId(string ids)
        {
            return Ok(_contractEquipmentQueries.GetAllHasToReclaimByOutContractId(ids));
        }

        [HttpGet("GetEquipmentInProjectOfOutContract")]
        public IActionResult GetEquipmentInProjectOfOutContract([FromQuery] EquipmentInProjectFilterModel equipmentInProjectFilterModel)
        {
            return Ok(_reportsQueries.GetEquipmentInProjectOfOutContract(equipmentInProjectFilterModel));
        }

        [HttpGet("GetEquipmentInProjectOfOutContractDetail")]
        public IActionResult GetEquipmentInProjectOfOutContractDetail([FromQuery] EquipmentInProjectFilterModel equipmentInProjectFilterModel)
        {
            return Ok(_reportsQueries.GetEquipmentInProjectOfOutContractDetail(equipmentInProjectFilterModel));
        }

        [HttpGet("GetListMasterCustomerNationwideBusiness")]
        public IActionResult GetListMasterCustomerNationwideBusiness([FromQuery] MasterCustomerNationwideBusinessFilterModel masterCustomerNationwideBusinessFilterModel)
        {
            return Ok(_reportsQueries.GetListMasterCustomerNationwideBusiness(masterCustomerNationwideBusinessFilterModel));

        }
    }
}