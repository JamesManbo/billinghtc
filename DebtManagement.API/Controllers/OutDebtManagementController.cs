using DebtManagement.API.PolicyBasedAuthProvider;
using DebtManagement.Infrastructure.Queries;
using Global.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DebtManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OutDebtManagementController : CustomBaseController
    {
        private readonly IOutDebtManagementQueries _debtManagementQueries;

        public OutDebtManagementController(IOutDebtManagementQueries debtManagementQueries)
        {
            _debtManagementQueries = debtManagementQueries;
            //_debtManagementQueries.RestrictByOrganization();
        }

        // GET: api/DebtManagement
        [HttpGet]
        [PermissionAuthorize("VIEW_OUT_DEBT_BY_CUSTOMER")]
        public async Task<IActionResult> GetPagedListByCustomer([FromQuery] DebtManagementFilterModel requestFilterModel)
        {
            return Ok(await _debtManagementQueries.GetDebtByCustomers(requestFilterModel));
        }

        [HttpGet, Route("out-debt-by-contracts")]
        [PermissionAuthorize("VIEW_OUT_DEBT_BY_CONTRACT")]
        public IActionResult GetPagedListByContract([FromQuery] DebtManagementFilterModel requestFilterModel)
        {
            return Ok(_debtManagementQueries.GetDebtByContracts(requestFilterModel));
        }

        [HttpGet, Route("debt-collection-onbehalf")]
        [PermissionAuthorize("VIEW_OUT_DEBT_COLLECTION_ON_BEHALF")]
        public IActionResult GetCollectionOnBehalf([FromQuery] DebtManagementFilterModel requestFilterModel)
        {
            return Ok(_debtManagementQueries.GetDebtCollectionOnBehalf(requestFilterModel));
        }

        [HttpGet, Route("GetOpeningDebtOfContract")]
        public IActionResult GetOpeningDebtOfContract([FromQuery] string targetUserId, [FromQuery] DateTime? startPeriod = null,
            [FromQuery] int? excludeVchrId = null)
        {
            if (!startPeriod.HasValue)
            {
                startPeriod = DateTime.Now;
            }

            return Ok(_debtManagementQueries.GetOpeningDebtByTarget(targetUserId, startPeriod.Value, excludeVchrId));
        }
    }
}
