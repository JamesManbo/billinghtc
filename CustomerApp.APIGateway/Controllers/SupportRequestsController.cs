using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerApp.APIGateway.Models.OutContract;
using CustomerApp.APIGateway.Models.RequestModels;
using CustomerApp.APIGateway.Models.SupportModels;
using CustomerApp.APIGateway.Services;
using Global.Models.PagedList;
using Global.Models.Response;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SupportRequestsController : CustomerBaseController
    {
        private readonly ISupportService _supportService;
        private readonly IUserService _userService;
        private readonly IContractService _contractService;
        private readonly IContractorService _contractorService;
        private readonly IMapper _mapper;
        public SupportRequestsController(ISupportService supportService,
            IContractService contractService,
            IContractorService contractorService,
            IMapper mapper,
            IUserService userService)
        {
            _supportService = supportService;
            _userService = userService;
            _contractService = contractService;
            _contractorService = contractorService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<SupportDTO>>>> GetAsync(
[FromQuery] SupportRequestFilterModel filterModel)
        {
            filterModel.CreatedBy = UserIdentity.UniversalId;
            var actResponse = await _supportService.GetListByIdentityGuid(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            return Ok(await _supportService.GetById(id));
        }

        [HttpPost]
        public async Task<ActionResult<IActionResponse<SupportRequest>>> Post(SupportRequest supportRequest)
        {
            if (supportRequest.OutContractServicePackageId <= 0)
            {
                return BadRequest("");
            }

            supportRequest.CreatedBy = UserIdentity.UniversalId;
            supportRequest.CustomerIdentityGuid = UserIdentity.UniversalId;
            //supportRequest.CustomerName = UserIdentity.FullName;
            //supportRequest.CustomerPhone = UserIdentity.MobilePhoneNo;
            //supportRequest.CustomerEmail = UserIdentity.Email;
            //supportRequest.Address = UserIdentity.Address;
            supportRequest.DateRequested = DateTime.Now;
            supportRequest.GlobalId = Guid.NewGuid().ToString();

            var customerModel = await _userService.FindUserByUId(supportRequest.CustomerIdentityGuid);
            supportRequest.CustomerName = customerModel.FullName;
            supportRequest.CustomerPhone = customerModel.MobilePhoneNo;
            supportRequest.CustomerEmail = customerModel.Email;
            supportRequest.CustomerCode = customerModel.Code;
            supportRequest.Address = customerModel.Address;

            var contractor = await _contractorService.GetContactorByIdentity(UserIdentity.UniversalId);
            if (contractor == null) return NotFound();

            supportRequest.ContractorId = contractor.Id;
            var filter = new ContractFilterModel();
            filter.Paging = false;
            filter.ContractorId = contractor.Id;
            var contractsResponse = await _contractService.GetListContact(filter);

            if (contractsResponse != null && contractsResponse.Subset != null && contractsResponse.Subset.Count > 0)
            {
                //var codes = string.Join(",", contractsResponse.Subset.Select(c => c.ContractCode).ToList());
                //if (!string.IsNullOrEmpty(codes))
                //{
                //    supportRequest.ContractCode = codes;
                //}

                //var ids = string.Join(",", contractsResponse.Subset.Select(c => c.Id).ToList());
                //if (!string.IsNullOrEmpty(ids))
                //{
                //    supportRequest.ContractId = ids;
                //}

                //var lstServicePackage = new List<OutContractServicePackageSimpleDTO>();
                //contractsResponse.Subset.ForEach(e =>
                //{
                //    if (e.ServicePackages != null && e.ServicePackages.Count > 0)
                //    {
                //        lstServicePackage.AddRange(e.ServicePackages);
                //    }
                //});
                //if (lstServicePackage.Count > 0)
                //{
                //    var cIds = string.Join(",", lstServicePackage.Select(c => c.CId).ToList().Distinct());
                //    if (!string.IsNullOrEmpty(ids))
                //    {
                //        supportRequest.CId = cIds;
                //    }
                //}

                if (!contractsResponse.Subset.Any(c => c.ContractCode == supportRequest.ContractCode))
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

            return BadRequest("");
        }

        [Route("Feedback/{id}")]
        [HttpPut]
        public async Task<IActionResult> CustomerRate(string id, [FromBody] SupportRequest updateCommand)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var existModel = await _supportService.GetById(id);
            if (existModel == null) return NotFound();
            var rq = _mapper.Map<SupportRequest>(existModel);
            rq.CustomerRate = updateCommand.CustomerRate;
            rq.CustomerComment = updateCommand.CustomerComment;

            var isSuccess = await _supportService.CustomerRate(rq);
            if (isSuccess) return Ok(new ActionResponse { Message = "Cảm ơn bạn đã gửi đánh giá!" });
            return BadRequest();

        }

        [HttpGet("GetListSupportTitle")]
        public async Task<IActionResult> GetListSupportTitle()
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
