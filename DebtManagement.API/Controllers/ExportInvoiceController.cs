using DebtManagement.Domain.Models.FilterModels;
using DebtManagement.Infrastructure.Queries;
using Microsoft.AspNetCore.Mvc;

namespace DebtManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ExportInvoiceController : CustomBaseController
    {
        private readonly IExportInvoiceFileQueries _exportInvoiceFileQueries;

        public ExportInvoiceController(IExportInvoiceFileQueries exportInvoiceFileQueries)
        {
            _exportInvoiceFileQueries = exportInvoiceFileQueries;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] ExportInvoiceFilterModel filterModel)
        {
            return Ok(_exportInvoiceFileQueries.GetList(filterModel));
        }
        [HttpGet]
        [Route("GetCountReceiptVouchers")]
        public IActionResult GetCountReceiptVouchers([FromQuery] ExportInvoiceFilterModel filterModel)
        {
            return Ok(_exportInvoiceFileQueries.GetCountReceiptVouchers(filterModel));
        } 
        
        [HttpGet]
        [Route("GetInvoiceReportData")]
        public IActionResult GetInvoiceReportData([FromQuery] ExportInvoiceFilterModel filterModel)
        {
            return Ok(_exportInvoiceFileQueries.GetInvoiceReportData(filterModel));
        }
    }
}
