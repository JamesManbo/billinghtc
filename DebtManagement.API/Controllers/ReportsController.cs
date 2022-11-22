using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtManagement.Domain.Models.FilterModels;
using DebtManagement.Infrastructure.Queries;
using Global.Models.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DebtManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportsController : CustomBaseController
    {
        private readonly IPaymentVoucherReportQueries _paymentVoucherReportQueries;
        private readonly IReceiptVoucherQueries _receiptVoucherQueries;

        public ReportsController(
            IPaymentVoucherReportQueries paymentVoucherReportQueries, IReceiptVoucherQueries receiptVoucherQueries)
        {
            _paymentVoucherReportQueries = paymentVoucherReportQueries;
            _receiptVoucherQueries = receiptVoucherQueries;
        }

        //[HttpGet]
        //[Route("PaymentVoucherReport")]
        //public IActionResult GetList([FromQuery] PaymentVoucherReportFilter filterModel)
        //{
        //    return Ok(_paymentVoucherReportQueries.GetList(filterModel));
        //}

        [HttpGet]
        [Route("GetReportServiceDebtExcelData")]
        public IActionResult GetReportServiceDebtExcelData([FromQuery] PaymentVoucherReportFilter filterModel)
        {
            return Ok(_paymentVoucherReportQueries.GetReportServiceDebtExcelData(filterModel));
        } 
        
        [HttpGet]
        [Route("GetReportServiceDebtData")]
        public IActionResult GetReportServiceDebtData([FromQuery] PaymentVoucherReportFilter filterModel)
        {
            return Ok(_paymentVoucherReportQueries.GetReportServiceDebtData(filterModel));
        }

        [HttpGet]
        [Route("GetTotalRevenuePersonalReport")]
        public IActionResult GetTotalRevenuePersonalReport([FromQuery] PaymentVoucherReportFilter filterModel)
        {
            return Ok(_receiptVoucherQueries.GetTotalRevenueEnterpiseReport(filterModel));
        }

        [HttpGet]
        [Route("GetFTTHProjectRevenue")]
        public IActionResult GetFTTHProjectRevenue([FromQuery] PaymentVoucherReportFilter filterModel)
        {
            return Ok(_receiptVoucherQueries.GetFTTHProjectRevenue(filterModel));
        }
    }
}