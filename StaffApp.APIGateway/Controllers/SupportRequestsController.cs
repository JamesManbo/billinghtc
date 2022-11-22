using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.Response;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.Feedback;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SupportRequestsController : CustomBaseController
    {
        private readonly ISupportService _supportService;
        private readonly IOutContractService _outContractService;
        public SupportRequestsController(ISupportService supportService, IOutContractService outContractService)
        {
            _supportService = supportService;
            _outContractService = outContractService;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<SupportDTO>>>> GetAsync(
    [FromQuery] RequestSupportFilterModel filterModel)
        {
            filterModel.CreatedBy = UserIdentity.UniversalId;
            var actResponse = await _supportService.GetList(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }

        [HttpPost]
        public async Task<ActionResult<IActionResponse<SupportCommand>>> Post(SupportCommand supportRequest)
        {
            if (supportRequest.OutContractServicePackageId <= 0 || string.IsNullOrEmpty(supportRequest.ContractCode))
            {
                return BadRequest("");
            }

            supportRequest.CreatedBy = UserIdentity.UniversalId;
            //supportRequest.CustomerId = UserIdentity.UniversalId;
            //supportRequest.CustomerName = UserIdentity.FullName;
            //supportRequest.CustomerPhone = UserIdentity.MobilePhoneNo;
            //supportRequest.CustomerEmail = UserIdentity.Email;
            //supportRequest.Address = UserIdentity.Address;
            supportRequest.DateRequested = DateTime.Now;
            supportRequest.GlobalId = Guid.NewGuid().ToString();

            //var customerModel = await _userService.FindUserByUId(supportRequest.CustomerId);
            //supportRequest.CustomerName = customerModel.FullName;
            //supportRequest.CustomerPhone = customerModel.MobilePhoneNo;
            //supportRequest.CustomerEmail = customerModel.Email;
            //supportRequest.CustomerCode = customerModel.Code;
            //supportRequest.Address = customerModel.Address;

            var contractsResponse = await _outContractService.GetDetailByCode(supportRequest.ContractCode);

            if (contractsResponse == null)
            {
                return BadRequest("");
            }

            var actResponse = await _supportService.CreateSupportRequest(supportRequest);
            if (actResponse)
            {
                return Ok(actResponse);

            }
            return BadRequest(actResponse);


        }


        [HttpGet("GetListSupportTitle")]
        public IActionResult GetListSupportTitle()
        {
            List<SelectionItem> listForm = new List<SelectionItem>() {
                new SelectionItem() { Text= "Yêu cầu khác", Value= -1 },
                new SelectionItem() { Text= "Đổi gói cước", Value= 0 },
                new SelectionItem() { Text= "Bảo hành thiết bị", Value= 1 },
                new SelectionItem() { Text= "Mạng chập chờn", Value= 2 },
                new SelectionItem() { Text= "Không truy cập được internet", Value= 3 },
            };
            listForm.Reverse();
            return Ok(listForm);
        }
    }
}