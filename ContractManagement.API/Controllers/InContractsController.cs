using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.API.PolicyBasedAuthProvider;
using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.InContractAggregate;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.InContractRepository;
using Global.Models.Filter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InContractsController : CustomBaseController
    {
        private readonly ILogger<InContractsController> _logger;
        private readonly IMediator _mediator;
        private readonly IInContractQueries _inContractQueries;
        private readonly IInContractRepository _contractRepository;

        public InContractsController(ILogger<InContractsController> logger,
            IMediator mediator,
            IInContractQueries inContractQueries,
            IInContractRepository contractRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _inContractQueries = inContractQueries;
            _contractRepository = contractRepository;
        }

        [HttpGet]
        [PermissionAuthorize("VIEW_IN_CONTRACT")]
        public IActionResult GetContracts([FromQuery] RequestFilterModel requestFilterModel)
        {
            if (requestFilterModel.Type == RequestType.SimpleAll)
            {
                return Ok(_inContractQueries.GetSimpleAll(requestFilterModel));
            }

            if (requestFilterModel.Type == RequestType.Autocomplete)
            {
                return Ok(_inContractQueries.Autocomplete(requestFilterModel));
            }

            if (requestFilterModel.Type == RequestType.AutocompleteSimple)
            {
                return Ok(_inContractQueries.AutocompleteSimple(requestFilterModel));
            }

            return Ok(_inContractQueries.GetPagedList(requestFilterModel));
        }

        [HttpGet("{id}")]
        [PermissionAuthorize("VIEW_IN_CONTRACT")]
        public IActionResult GetContract(int id)
        {
            var contract = _inContractQueries.FindById(id);

            if (contract == null)
            {
                return NotFound();
            }

            return Ok(contract);
        }

        [HttpPost]
        [PermissionAuthorize("ADD_NEW_IN_CONTRACT")]
        public async Task<IActionResult> PostContract([FromBody] CreateInContractCommand createInContractCommand)
        {
            createInContractCommand.SalesmanIdentityGuid = UserIdentity.UniversalId;
            createInContractCommand.CreatedBy = UserIdentity.UserName;
            var createContractCmdRsp = await _mediator.Send(createInContractCommand);
            if (createContractCmdRsp.IsSuccess)
            {
                return Ok(createContractCmdRsp);
            }

            return BadRequest(createContractCmdRsp);
        }

        [HttpPut("{id}")]
        [PermissionAuthorize("EDIT_IN_CONTRACT")]
        public async Task<IActionResult> PutContract(int id, [FromBody] UpdateInContractCommand updateInContractCommand)
        {
            if (id != updateInContractCommand.Id)
                return BadRequest();

            updateInContractCommand.SalesmanIdentityGuid = UserIdentity.UniversalId;
            updateInContractCommand.UpdatedBy = UserIdentity.UserName;

            var createContractCmdRsp = await _mediator.Send(updateInContractCommand);
            
            if (createContractCmdRsp.IsSuccess)
            {
                return Ok(createContractCmdRsp);
            }

            return BadRequest(createContractCmdRsp);
        }

        [HttpDelete("{id}")]
        [PermissionAuthorize("TERMINATE_IN_CONTRACT")]
        public async Task<IActionResult> CanceledContract(int id)
        {
            var inContract = await _contractRepository.GetByIdAsync(id);
            if (inContract == null)
            {
                return NotFound();
            }

            inContract.SetCancelledStatus();

            return Ok(await _contractRepository.SaveChangeAsync());
        }

        [Route("GetOutContractSharingRevenues")]
        [HttpGet]
        [PermissionAuthorize("VIEW_IN_CONTRACT")]
        public IActionResult GetOutContractSharingRevenues(int inContractId, int? inContractType, int currencyUnitId)
        {
            return Ok(_inContractQueries.GetOutContractSharingRevenues(inContractId, inContractType, currencyUnitId));
        }

        [Route("GetOrderNumberByNow")]
        [HttpGet]
        public IActionResult GetOrderNumberByNow()
        {
            return Ok(_inContractQueries.GetOrderNumberByNow());
        }

        [Route("GenerateContractCode")]
        [HttpGet]
        public IActionResult GenerateContractCode(int contractType, string contractorFullName, int? marketAreaId = null)
        {
            return Ok(_inContractQueries.GenerateContractCode(contractType, contractorFullName, marketAreaId));
        }
    }
}
