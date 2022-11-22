using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.FilterModels.ReportsModel;
using ContractManagement.Infrastructure.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportsController : CustomBaseController
    {
        private readonly ILogger<ReportsController> _logger;
        private readonly IReportsQueries _reportsQueries;

        public ReportsController(ILogger<ReportsController> logger,
            IReportsQueries reportsQueries)
        {
            _logger = logger;
            _reportsQueries = reportsQueries;
        }
       
        [HttpGet]
        [Route("getCommissionAndSharingReportData")]
        public IActionResult GetCommissionAndSharingReportData([FromQuery] CommissionAndSharingReportFilter reportFilter)
        {
            return Ok(_reportsQueries.GetCommissionAndSharingReportData(reportFilter));
        } 
        
        [HttpGet]
        [Route("GetFeeReportData")]
        public IActionResult GetFeeReportData([FromQuery] FeeReportFilter reportFilter)
        {
            return Ok(_reportsQueries.GetFeeReportData(reportFilter));
        }
        [HttpGet]
        public IActionResult GetAll([FromQuery] FeeReportFilter reportFilter)
        {
            return Ok(_reportsQueries.GetFeeReportData(reportFilter));
        }
        
        [HttpGet]
        [Route("getFeeTotalReportData")]
        public IActionResult GetFeeTotalReportData([FromQuery] FeeReportFilter reportFilter)
        {
            return Ok(_reportsQueries.GetFeeTotalReportData(reportFilter));
        }
        
        [HttpGet]
        [Route("getTotalDataIncontractFeeSharingReport")]
        public IActionResult GetTotalDataIncontractFeeSharingReport([FromQuery] FeeReportFilter reportFilter)
        {
            return Ok(_reportsQueries.GetTotalDataIncontractFeeSharingReport(reportFilter));
        }

        [HttpGet]
        [Route("GetTotalRevenuePersonalReport")]
        public async Task<IActionResult> GetTotalRevenuePersonalReport([FromQuery] MasterCustomerNationwideBusinessFilterModel filterModel)
        {
            return Ok(await _reportsQueries.GetTotalRevenueEnterpiseReport(filterModel));
        }

        [HttpGet]
        [Route("GetFTTHProjectRevenue")]
        public async Task<IActionResult> GetFTTHProjectRevenue([FromQuery] MasterCustomerNationwideBusinessFilterModel filterModel)
        {
            return Ok(await _reportsQueries.GetFTTHProjectRevenue(filterModel));
        }
    }
}
