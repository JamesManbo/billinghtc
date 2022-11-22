using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.AcceptanceDTO;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Models.TransactionsModel;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AcceptancesController : CustomBaseController
    {
        private readonly IAcceptanceService _acceptanceService;
        private readonly ITransactionsService _transactionsService;
        public AcceptancesController(IAcceptanceService acceptanceService, ITransactionsService transactionsService)
        {
            _acceptanceService = acceptanceService;
            _transactionsService = transactionsService;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<AcceptanceDTO>>>> Get([FromQuery] AcceptanceFilterModel filterModel)
        {
            int[] allowTypes = { 1, 2, 3, 4, 5, 6, 7, 8, 11, 12 }; //transactionType
            if (!string.IsNullOrEmpty(filterModel.AcceptanceTypes))
            {

                var lstType = filterModel.AcceptanceTypes.Split(",");
                List<string> temp = lstType.Where(type => allowTypes.Contains(int.TryParse(type, out int typeOut) ? typeOut : -1)).ToList();
                filterModel.AcceptanceTypes = string.Join(",", temp);
            }
            else { filterModel.AcceptanceTypes = string.Join(",", allowTypes); }
            filterModel.SupporterId = UserIdentity.UniversalId;

            var actResponse = await _acceptanceService.GetList(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IActionResponse<TransactionDTO>>> GetDetail(int id)
        {
            var actResponse = await _acceptanceService.GetDetail(id);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }
    }
}
