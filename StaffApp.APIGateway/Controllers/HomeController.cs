using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.ReceiptVoucherModels;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Models.TransactionsModel;
using StaffApp.APIGateway.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : CustomBaseController
    {
        IHomeService _homeService;
        IOutContractService _outContractService;
        ITransactionSupporterService _transactionSupporterService;

        public HomeController(IHomeService homeService, ITransactionSupporterService transactionSupporterService, IOutContractService outContractService)
        {
            _homeService = homeService;
            _transactionSupporterService = transactionSupporterService;
            _outContractService = outContractService;
        }

        [HttpGet]
        [Route("ReceiptVoucherNumberReport")]
        public async Task<ActionResult<IActionResponse<List<CollectedVoucherDTO>>>>ReceiptVoucherNumberReport([FromQuery] CollectedVoucherFilterModel filterModel )
        {
            filterModel.CashierUserId = UserIdentity.UniversalId;
            var actResponse = await _homeService.GetList(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
            
        }

        [HttpGet]
        [Route("GetTransactionSupporterReport")]
        public async Task<ActionResult<IActionResponse<List<TransactionSupporterDTO>>>> GetTransactionSupporterReport([FromQuery] TransactionSupporterFilterModel filterModel)
        {
            filterModel.UserIdentity = UserIdentity.UniversalId;
            var actResponse = await _transactionSupporterService.GetTransactionSupporterReport(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);

        }


        [HttpGet]
        [Route("GetReportContractStatus")]
        public async Task<ActionResult<IActionResponse<List<TransactionSupporterDTO>>>> GetReportContractStatus([FromQuery] ContractStatusReportFilter filterModel)
        {
            var actResponse = await _outContractService.GetReportContractStatus(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);

        }
    }
}