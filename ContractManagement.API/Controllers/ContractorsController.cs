using System;
using System.Threading.Tasks;
using ContractManagement.API.Application.Commands;
using ContractManagement.API.PolicyBasedAuthProvider;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.FilterModels.ReportsModel;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.OutContractRepository;
using Global.Models.Filter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContractorsController : CustomBaseController
    {
        private readonly ILogger<ContractsController> _logger;
        private readonly IMediator _mediator;
        private readonly IContractorQueries _contractorQueries;
        private readonly IContractorRepository _contractorRepository;
        private readonly IReportsQueries _reportQueries;

        public ContractorsController(ILogger<ContractsController> logger,
            IMediator mediator,
            IContractorQueries contractorQueries,
            IReportsQueries reportsQueries,
            IContractorRepository contractorRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _contractorQueries = contractorQueries;
            _reportQueries = reportsQueries;
            this._contractorRepository = contractorRepository;
        }

        [HttpGet]
        public IActionResult GetContracts([FromQuery] RequestFilterModel requestFilterModel)
        {
            if (requestFilterModel.Type == RequestType.Selection)
            {
                return Ok(_contractorQueries.GetSelectionList());
            }
            else if (requestFilterModel.Type == RequestType.Autocomplete)
            {
                return Ok(_contractorQueries.Autocomplete(requestFilterModel));
            }
            return Ok(_contractorQueries.GetList(requestFilterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetContractor(int id)
        {
            var contractor = _contractorQueries.FindById(id);

            if (contractor == null)
            {
                return NotFound();
            }

            return Ok(contractor);
        }

        [HttpGet("GetContractorInProjectOfOutContract")]
        public IActionResult GetContractorInProjectOfOutContract([FromQuery] ContractorInProjectFilterModel contractorInProjectFilterModel)
        {
            return Ok(_reportQueries.GetContractorInProjectOfOutContract(contractorInProjectFilterModel));
        }

        [Route("GetCount")]
        [HttpGet]
        public IActionResult GetCount(int projectId)
        {
            return Ok(_contractorQueries.GetContractorCountByProjectId(projectId));
        }

        [HttpPost]
        [PermissionAuthorize("ADD_CONTRACTOR_HTC")]
        public async Task<IActionResult> PostContract([FromBody] CUContractorHTCCommand createContractorHTCCommand)
        {
            createContractorHTCCommand.CreatedBy = UserIdentity.UserName;
            var createContracttorCmdRsp = await _mediator.Send(createContractorHTCCommand);
            if (createContracttorCmdRsp.IsSuccess)
            {
                return Ok(createContracttorCmdRsp);
            }

            return BadRequest(createContracttorCmdRsp);
        }

        [Route("{id}")]
        [HttpPut]
        [PermissionAuthorize("EDIT_CONTRACTOR_HTC")]
        public async Task<IActionResult> PutContract(int id, [FromBody] CUContractorHTCCommand updateContractorCommand)
        {
            if (updateContractorCommand.Id != id)
            {
                return BadRequest();
            }

            updateContractorCommand.UpdatedBy = UserIdentity.UserName;
            var updateContractCmdRsp = await _mediator.Send(updateContractorCommand);
            if (updateContractCmdRsp.IsSuccess)
            {
                return Ok(updateContractCmdRsp);
            }

            return BadRequest(updateContractCmdRsp);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var contractor = _contractorQueries.FindById(id);

            if (contractor == null)
            {
                return NotFound();
            }

            return Ok(this._contractorRepository.DeleteAndSave(id));
        }
    }
}