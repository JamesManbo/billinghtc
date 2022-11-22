using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Infrastructure.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DashboardController : CustomBaseController
    {            
        private readonly IDashboardQueries _dashboardQueries;
        public DashboardController(IDashboardQueries dashboardQueries)
        {
            _dashboardQueries = dashboardQueries;
        }

        [HttpGet]
        [Route("GetOutContractTotalQuantityEveryMonth")]
        public IActionResult GetList()
        {
            return Ok(_dashboardQueries.GetOutContractTotalQuantityEveryMonth());
        }
        [HttpGet]
        [Route("getInContractTotalQuantityEveryMonth")]
        public IActionResult GetInContractTotalQuantityEveryMonth()
        {
            return Ok(_dashboardQueries.GetInContractTotalQuantityEveryMonth());
        }
        [HttpGet]
        [Route("getOutContractSignedQuantityEveryMonth")]
        public IActionResult GetOutContractSignedQuantityEveryMonth()
        {
            return Ok(_dashboardQueries.GetOutContractSignedQuantityEveryMonth());
        }

        [HttpGet]
        [Route("getOutContractEffectedQuantityEveryMonth")]
        public IActionResult GetOutContractEffectedQuantityEveryMonth()
        {
            return Ok(_dashboardQueries.GetOutContractEffectedQuantityEveryMonth());
        }
    }
}
