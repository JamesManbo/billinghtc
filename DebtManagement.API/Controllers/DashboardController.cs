using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtManagement.Infrastructure.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DebtManagement.API.Controllers
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
      

        [HttpGet("GetRevenueAndTaxAmountInYear")]
        public IActionResult GetRevenueAndTaxAmountInYear()
        {
            return Ok(_dashboardQueries.GetRevenueAndTaxAmountInYear());
        }

        [HttpGet]
        [Route("GetDailyRevenueByServiceGroup")]
        public IActionResult GetDailyRevenueByServiceGroup()
        {
            return Ok(_dashboardQueries.GetDailyRevenueByServiceGroup());
        }

        [HttpGet]
        [Route("GetDailyRevenueByService")]
        public IActionResult GetDailyRevenueByService()
        {
            return Ok(_dashboardQueries.GetDailyRevenueByService());
        }
    }
}
