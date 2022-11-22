using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerApp.APIGateway.Models.ReceiptVoucherModels;
using CustomerApp.APIGateway.Models.RequestModels;
using CustomerApp.APIGateway.Services;
using DebtManagement.API.Protos;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PayBillsController : CustomerBaseController
    {
        private readonly IReceiptVoucherService _receiptVoucherService;
        private readonly IContractService _contractService;
        private readonly IContractorService _contractorService;
        public PayBillsController(IReceiptVoucherService receiptVoucherService, IContractService contractService, IContractorService contractorService)
        {
            _receiptVoucherService = receiptVoucherService;
            _contractService = contractService;
            _contractorService = contractorService;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<ReceiptVoucherGridDTO>>>> Get(
            [FromQuery] ReceiptVoucherFilterModel filterModel)
        {
            var contractor = await _contractorService.GetContactorByIdentity(UserIdentity.UniversalId);
            if (contractor == null)
            {
                return NotFound();
            }
            var outContracts = await _contractService.GetListContact(new ContractFilterModel { ContractorId = contractor.Id, Paging = false });
            if (outContracts == null || outContracts.TotalItemCount <= 0)
            {
                return NotFound();
            }

            filterModel.OutContractIds = string.Join(",", outContracts.Subset.Select(c => c.Id).ToList());
            var actResponse = await _receiptVoucherService.GetList(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IActionResponse<ReceiptVoucherDTO>>> GetDetail(string id)
        {
            var receiptVoucherDetail = await _receiptVoucherService.GetDetail(id);

            if (receiptVoucherDetail == null) return NotFound();

            return Ok(receiptVoucherDetail);
        }
    }
}