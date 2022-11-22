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
    public class DashboardSupporterController : CustomBaseController
    {
        private readonly IDashboardQueries _dashboardQueries;
        public DashboardSupporterController(IDashboardQueries dashboardQueries)
        {
            _dashboardQueries = dashboardQueries;
        }

        [HttpGet]
        [Route("GetTotalNationalProject")]
        public IActionResult GetTotalNationalProject(DateTime startDate, DateTime endDate)
        {
            return Ok(_dashboardQueries.GetTotalNationalProject(startDate, endDate));
        }

        [HttpGet]
        [Route("GetTotalCustomerDashboardSupporter")]
        public IActionResult GetTotalCustomerDashboardSupporter(DateTime startDate, DateTime endDate)
        {
            return Ok(_dashboardQueries.GetTotalCustomerDashboardSupporter(startDate, endDate));
        }
        [HttpGet]
        [Route("GetTotalEquipmentDashboardSupporter")]
        public IActionResult GetTotalEquipmentDashboardSupporter(DateTime startDate, DateTime endDate)
        {
            return Ok(_dashboardQueries.GetTotalEquipmentDashboardSupporter(startDate, endDate));
        }
        [HttpGet]
        [Route("GetTotalCustomerIncrementDashboardSupporter")]
        public IActionResult GetTotalCustomerIncrementDashboardSupporter(DateTime startDate, DateTime endDate)
        {
            return Ok(_dashboardQueries.GetTotalCustomerIncrementDashboardSupporter(startDate, endDate));
        }
        [HttpGet]
        [Route("GetTotalCustomerDecrementDashboardSupporter")]
        public IActionResult GetTotalCustomerDecrementDashboardSupporter(DateTime startDate, DateTime endDate)
        {
            return Ok(_dashboardQueries.GetTotalCustomerDecrementDashboardSupporter(startDate, endDate));
        }

        [HttpGet]
        [Route("GetCurrentWorkStatus")]
        public IActionResult GetCurrentWorkStatus(int maketId)
        {
            return Ok(_dashboardQueries.GetCurrentWorkStatus(maketId));
        }
    }
}
