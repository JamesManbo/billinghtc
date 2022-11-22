using ContractManagement.Infrastructure.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContractSharingRevenuesController : CustomBaseController
    {
        private readonly ILogger<ContractSharingRevenuesController> _logger;
        private readonly IContractSharingRevenueQueries _contractSharingRevenueQueries;

        public ContractSharingRevenuesController(ILogger<ContractSharingRevenuesController> logger,
            IContractSharingRevenueQueries contractSharingRevenueQueries)
        {
            _logger = logger;
            _contractSharingRevenueQueries = contractSharingRevenueQueries;
        }

        [HttpGet]
        [Route("GetAllByOutContractId/{outContractId}")]
        public IActionResult GetContracts(int outContractId)
        {
            return Ok(_contractSharingRevenueQueries.GetAllByOutContractId(outContractId));
        }

        [HttpGet]
        [Route("GetAllByInContractId/{inContractId}")]
        public IActionResult GetAllByInContractId(int inContractId, int inContractType)
        {
            return Ok(_contractSharingRevenueQueries.GetAllByInContractId(inContractId, inContractType));
        }

        //[HttpGet]
        //[Route("GetSharedRevenueByInContract/{inContractId}")]
        //public IActionResult GetSharedRevenueByInContract(int inContractId)
        //{
        //    return Ok(_contractSharingRevenueQueries.GetSharedRevenueByInContract(inContractId));
        //}
    }
}