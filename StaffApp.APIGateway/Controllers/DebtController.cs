using System;
using System.Threading.Tasks;
using AutoMapper;
using DebtManagement.API.Protos;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StaffApp.APIGateway.Models.CuReceiptVoucherCommands;
using StaffApp.APIGateway.Models.ReceiptVoucherModels;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DebtController : CustomBaseController
    {
        private readonly IReceiptVoucherService _receiptVoucherService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public DebtController(
            IReceiptVoucherService receiptVoucherService,
            IConfiguration configuration,
            IMapper mapper)
        {
            _receiptVoucherService = receiptVoucherService;
            _configuration = configuration;
            _mapper = mapper;
        }

        /// <summary>
        /// Danh sách phiếu thu
        /// </summary>
        /// <param name="filterModel"></param>
        /// <returns>Danh sách phiếu thu</returns>
        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<ReceiptVoucherGridDTO>>>> GetAsync(
            [FromQuery] ReceiptVoucherFilterModel filterModel)
        {
            filterModel.CashierUserId = UserIdentity.UniversalId;
            //filterModel.ServiceIds = _configuration.GetValue<int>("ServiceIdFTTH").ToString();
            var actResponse = await _receiptVoucherService.GetList(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IActionResponse<ReceiptVoucherDTO>>> GetAsync(string id)
        {
            var actResponse = await _receiptVoucherService.GetDetail(id);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ActionResponse>> Put(string id, [FromBody] ReceiptVoucherDTO rcptVchrDTO)
        {
            if (string.IsNullOrWhiteSpace(id) || !id.Equals(rcptVchrDTO.Id))
            {
                return BadRequest();
            }
            var updateRcptVchrCommand = _mapper.Map<CuReceiptVoucherCommand>(rcptVchrDTO);

            var actResponse = await _receiptVoucherService.Update(updateRcptVchrCommand);
            if (actResponse == null) return BadRequest();
            if (actResponse.IsSuccess != true)
            {
                if(actResponse.Errors!=null && actResponse.Errors.Count>0) return BadRequest(actResponse.Errors[0].ErrorMessage);
                return BadRequest(actResponse);
            }
            return Ok(actResponse);
        }

        [HttpGet("GetReceiptStatuses")]
        public async Task<IActionResult> GetOutContractStatuses()
        {
            var result = await _receiptVoucherService.GetReceiveStatuses();
            if (result == null)
            {
                return NotFound();
            }
            var allItem = new JObject();
            allItem.Add("Name", "Tất cả");
            allItem.Add("Id", 0);
            var rs = JArray.Parse(result);
            rs.AddFirst(allItem);
            return Ok(rs);
        }
    }
}