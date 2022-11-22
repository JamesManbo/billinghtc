using ContractManagement.API.PolicyBasedAuthProvider;
using ContractManagement.Domain.Commands.ContractFormCommand;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ContractFormRepository;
using Global.Models.Filter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ContractFormsController : CustomBaseController
    {
        private readonly ILogger<ContractFormsController> _logger;
        private readonly IMediator _mediator;
        private readonly IContractFormQueries _contractFormQueries;
        private readonly IContractFormRepository _contractFormRepository;

        public ContractFormsController(ILogger<ContractFormsController> logger,
            IMediator mediator,
            IContractFormQueries ContractFormQueries, 
            IContractFormRepository contractFormRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _contractFormQueries = ContractFormQueries;
            _contractFormRepository = contractFormRepository;
        }

        [HttpGet]
        [PermissionAuthorize("VIEW_CONTRACT_FORM")]
        public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_contractFormQueries.GetSelectionList(filterModel));
            }

            if (filterModel.Type == RequestType.Autocomplete)
            {
                return Ok(_contractFormQueries.Autocomplete(filterModel));
            }

            return Ok(_contractFormQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        [PermissionAuthorize("VIEW_CONTRACT_FORM")]
        public IActionResult GetById(int id)
        {
            var contractForm = _contractFormQueries.Find(id);

            if (contractForm == null)
            {
                return NotFound();
            }

            return Ok(contractForm);
        }

        [HttpPost]
        [PermissionAuthorize("ADD_CONTRACT_FORM")]
        public async Task<IActionResult> Create([FromBody] CUContractFormCommand createContractFormCommand)
        {
            createContractFormCommand.CreatedBy = UserIdentity.UserName;
            var createContractFormCmdRsp = await _mediator.Send(createContractFormCommand);
            if (createContractFormCmdRsp.IsSuccess)
            {
                return Ok(createContractFormCmdRsp);
            }

            return BadRequest(createContractFormCmdRsp);
        }

        [HttpPut("{id}")]
        [PermissionAuthorize("EDIT_CONTRACT_FORM")]
        public async Task<IActionResult> Update(int id, [FromBody] CUContractFormCommand updateContractFormCommand)
        {
            if (updateContractFormCommand.Id != id)
            {
                return BadRequest();
            }

            updateContractFormCommand.UpdatedBy = UserIdentity.UserName;
            var updateContractFormCmdRsp = await _mediator.Send(updateContractFormCommand);
            if (updateContractFormCmdRsp.IsSuccess)
            {
                return Ok(updateContractFormCmdRsp);
            }

            return BadRequest(updateContractFormCmdRsp);
        }

        [HttpDelete("{id}")]
        [PermissionAuthorize("DELETE_CONTRACT_FORM")]
        public IActionResult Delete(int id)
        {
            var contractForm = _contractFormQueries.Find(id);
            if (contractForm == null)
            {
                return NotFound();
            }

            var deleteResponse = _contractFormRepository.DeleteAndSave(id);

            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }
    }
}
