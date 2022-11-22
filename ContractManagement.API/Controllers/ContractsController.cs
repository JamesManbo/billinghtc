using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.API.PolicyBasedAuthProvider;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.FilterModels;
using ContractManagement.Domain.FilterModels.ReportsModel;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Utility;
using Global.Models.Filter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContractsController : CustomBaseController
    {
        private readonly ILogger<ContractsController> _logger;
        private readonly IMediator _mediator;
        private readonly IOutContractQueries _outContractQueries;
        private readonly IOutContractServicePackageQueries _channelQueries;
        private readonly IAttachmentFileQueries _attachmentFileQueries;
        private readonly IReportsQueries _reportsQueries;

        public ContractsController(
            ILogger<ContractsController> logger,
            IMediator mediator,
            IAttachmentFileQueries attachmentFileQueries,
            IOutContractQueries outContractQueries,
            IReportsQueries reportsQueries,
            IOutContractServicePackageQueries channelQueries)
        {
            this._logger = logger;
            this._mediator = mediator;
            this._outContractQueries = outContractQueries;
            this._attachmentFileQueries = attachmentFileQueries;
            this._reportsQueries = reportsQueries;
            this._channelQueries = channelQueries;
        }

        [HttpGet]
        [PermissionAuthorize("VIEW_OUT_CONTRACT")]
        public async Task<IActionResult> GetContracts([FromQuery] ContactsFilterModel requestFilterModel)
        {
            if (requestFilterModel.Type == RequestType.Autocomplete)
            {
                return Ok(await _outContractQueries.Autocomplete(requestFilterModel));
            }

            return Ok(await _outContractQueries.GetPagedList(requestFilterModel));
        }

        [HttpGet("AutoCompletePayable")]
        public IActionResult AutoCompletePayable([FromQuery] ContactsFilterModel requestFilterModel)
        {
            return Ok(_outContractQueries.AutocompletePayable(requestFilterModel));
        }

        [HttpGet("{id}")]
        [PermissionAuthorize("VIEW_OUT_CONTRACT")]
        public IActionResult GetContract(int id)
        {
            var contract = _outContractQueries.FindById(id);

            if (contract == null)
            {
                return NotFound();
            }

            return Ok(contract);
        }

        [HttpGet("GetOutContractSimpleAllByInContractId")]
        [PermissionAuthorize("VIEW_OUT_CONTRACT")]
        public IActionResult GetOutContractSimpleAllByInContractId(int id, int currencyUnitId)
        {
            return Ok(_outContractQueries.GetOutContractSimpleAllByInContractId(id, currencyUnitId));
        }

        [HttpPost]
        [PermissionAuthorize("ADD_NEW_OUT_CONTRACT")]
        public async Task<IActionResult> PostContract([FromBody] CreateContractCommand createContractCommand)
        {
            createContractCommand.SalesmanIdentityGuid = UserIdentity.UniversalId;
            createContractCommand.CreatedBy = UserIdentity.UserName;
            var createContractCmdRsp = await _mediator.Send(createContractCommand);
            if (createContractCmdRsp.IsSuccess)
            {
                return Ok(createContractCmdRsp);
            }

            return BadRequest(createContractCmdRsp);
        }

        [Route("{id}")]
        [HttpPut]
        [PermissionAuthorize("EDIT_OUT_CONTRACT")]
        public async Task<IActionResult> PutContract(int id, [FromBody] UpdateContractCommand updateContractCommand)
        {
            var permissions = UserIdentity.Permissions;
            var currentContractStatus = this._outContractQueries.FindStatusById(updateContractCommand.Id);
            if ((currentContractStatus == ContractStatus.Draft.Id) || permissions.Contains("EDIT_OUT_CONTRACT_SIGNED"))
            {
                if (updateContractCommand.Id != id)
                {
                    return BadRequest();
                }

                updateContractCommand.UpdatedBy = UserIdentity.UserName;
                var createContractCmdRsp = await _mediator.Send(updateContractCommand);
                if (createContractCmdRsp.IsSuccess)
                {
                    return Ok(createContractCmdRsp);
                }

                return BadRequest(createContractCmdRsp);
            }
            else
            {
                return BadRequest("Bạn không có quyền thực hiện chức năng này.");
            }
        }

        [Route("GetOrderNumberByNow")]
        [HttpGet]
        public IActionResult GetOrderNumberByNow()
        {
            return Ok(_outContractQueries.GetOrderNumberByNow());
        }

        [Route("GenerateContractCode")]
        [HttpGet]
        public IActionResult GenerateContractCode(bool isEnterprise,
            string customerFullName,
            int? marketAreaId = null,
            int? projectId = null,
            string srvIds = "")
        {
            return Ok(_outContractQueries
                .GenerateContractCode(isEnterprise, customerFullName, srvIds.SplitToInt(','), marketAreaId, projectId));
        }

        [Route("GetAttachmentFiles/{take}")]
        [HttpGet]
        public IActionResult GetAttachmentFiles(int take)
        {
            var requestFilterModel = new RequestFilterModel
            {
                Skip = take - 10,
                Take = 10
            };
            return Ok(_attachmentFileQueries.GetList(requestFilterModel));
        }

        [Route("getDataTotalRevenue")]
        [HttpGet]
        public IActionResult GetDataTotalRevenue([FromQuery] ReportTotalRevenueFillter reportFilter)
        {
            return Ok(_reportsQueries.GetDataTotalRevenue(reportFilter));
        }

        [Route("GetReportIncreasementOutContract")]
        [HttpGet]
        public IActionResult GetReportIncreasementOutContract([FromQuery] ReportTotalRevenueFillter reportFilter)
        {
            return Ok(_reportsQueries.GetReportIncreasementOutContract(reportFilter));
        }

        [PermissionAuthorize("IMPORT_SEED_CONTRACT")]
        [HttpPost("Import")]
        public async Task<IActionResult> ImportOutContract([FromForm] IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return BadRequest();
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest();
            }

            var importOutContractCommand = new ImportOutContractCommand(formFile);

            var actionResponse = await _mediator.Send(importOutContractCommand);
            if (!actionResponse.IsSuccess)
            {
                return BadRequest(actionResponse);
            }
            return Ok(actionResponse);
        }

        //[PermissionAuthorize("IMPORT_SEED_EQUIPMENTS")]
        [HttpPost("ImportContractEquips")]
        public async Task<IActionResult> ImportContractEquips([FromForm] IFormFile formFile)
        {
            if (formFile == null || formFile.Length <= 0)
            {
                return BadRequest();
            }

            if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest();
            }

            var importOutContractCommand = new ImportContractEquipmentCommand(formFile);

            var actionResponse = await _mediator.Send(importOutContractCommand);
            if (!actionResponse.IsSuccess)
            {
                return BadRequest(actionResponse);
            }
            return Ok(actionResponse);
        }

        [HttpGet("channel/{channelId}")]
        public IActionResult GetChannelInfo(int channelId)
        {
            return Ok(this._channelQueries.FindById(channelId));
        }
    }
}