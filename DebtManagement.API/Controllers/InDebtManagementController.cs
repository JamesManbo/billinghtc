using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtManagement.API.PolicyBasedAuthProvider;
using DebtManagement.Infrastructure.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DebtManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InDebtManagementController : CustomBaseController
    {
        private readonly IInDebtManagementQueries _inDebtManagementQueries;

        public InDebtManagementController(IInDebtManagementQueries inDebtManagementQueries)
        {
            _inDebtManagementQueries = inDebtManagementQueries;
        }

        // GET: api/InDebtManagement
        [HttpGet]
        [PermissionAuthorize("VIEW_IN_DEBT_BY_PARTNER")]
        public IActionResult GetInDebtByPartner([FromQuery] InDebtManagementFilterModel filterModel)
        {
            return Ok(this._inDebtManagementQueries.GetDebtByPartner(filterModel));
        }

        // GET: api/InDebtManagement/5
        [HttpGet("indebt-by-contracts")]
        [PermissionAuthorize("VIEW_IN_DEBT_BY_CONTRACT")]
        public IActionResult GetInDebtByContract(string partnerId, [FromQuery] InDebtManagementFilterModel filterModel)
        {
            return Ok(this._inDebtManagementQueries.GetDebtByContracts(partnerId, filterModel));
        }
    }
}
