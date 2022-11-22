using DebtManagement.Domain.Models.ReportModels;
using DebtManagement.Infrastructure.Queries;
using Global.Models.Filter;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DebtManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ReportCustomerController : CustomBaseController
    {
        private readonly IReportCustomerQueries _reportCustomerQueries;
        private readonly IPaymentVoucherQueries _paymentVoucherQueries;

        public ReportCustomerController(IReportCustomerQueries reportCustomerQueries, IPaymentVoucherQueries paymentVoucherQueries)
        {
            _reportCustomerQueries = reportCustomerQueries;
            _paymentVoucherQueries = paymentVoucherQueries;
        }
      

        [HttpGet]
        [Route("GetReportOutContract")]
        public IActionResult GetReportOutContract([FromQuery]ReportCustomerFillter filterModel = null)
        {
            return Ok(_reportCustomerQueries.GetReportCustomer(filterModel));
        }         

    }
}
