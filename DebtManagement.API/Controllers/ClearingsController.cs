using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtManagement.Domain.Commands.ClearingCommand;
using DebtManagement.Infrastructure.Queries;
using DebtManagement.Infrastructure.Repositories;
using Global.Models.Filter;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DebtManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClearingsController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly IClearingRepository _clearingRepository;
        private readonly IClearingQueries _clearingQueries;

        public ClearingsController(
            IMediator mediator,
            IClearingRepository clearingRepository,
            IClearingQueries clearingQueries)
        {
            _mediator = mediator;
            _clearingRepository = clearingRepository;
            _clearingQueries = clearingQueries;
        }
        [HttpGet]
        public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        {
            return Ok(_clearingQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetClearing(int id)
        {
            var clearing = _clearingQueries.Find(id);
            if (clearing == null)
            {
                return NotFound();
            }

            return Ok(clearing);
        }

        [HttpGet, Route("GetLatestIndex")]
        public IActionResult GetLatestIndex()
        {
            return Ok(_clearingQueries.GetOrderNumberByNow());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CUClearingCommand createClearingCommand)
        {
            createClearingCommand.CreatedBy = UserIdentity.UserName;
            var actResponse = await _mediator.Send(createClearingCommand);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            return BadRequest(actResponse);
        }

        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update(string id, [FromBody] CUClearingCommand updateClearingCommand)
        //{
        //    if (id != updateClearingCommand.Id)
        //    {
        //        return BadRequest();
        //    }
        //    updateClearingCommand.UpdatedBy = UserIdentity.UserName;
        //    var actionResponse = await _mediator.Send(updateClearingCommand);

        //    if (actionResponse.IsSuccess)
        //    {
        //        return Ok(actionResponse);
        //    }

        //    return BadRequest(actionResponse);
        //}

        //[HttpPut("UpdateClearingStatus/{id}")]
        //public async Task<IActionResult> UpdateClearingStatus(string id, [FromBody] UpdateClearingStatusCommand updateClearingStatusCommand)
        //{
        //    if (id != updateClearingStatusCommand.Id)
        //    {
        //        return BadRequest();
        //    }

        //    updateClearingStatusCommand.UpdatedBy = UserIdentity.UserName;
        //    var actionResponse = await _mediator.Send(updateClearingStatusCommand);

        //    if (actionResponse.IsSuccess)
        //    {
        //        return Ok(actionResponse);
        //    }

        //    return BadRequest(actionResponse);
        //}

        [HttpPut("Approve/{id}")]
        public async Task<IActionResult> Approve(int id, [FromBody] ApproveClearingVoucherCommand approvalCommand)
        {
            if (id != approvalCommand.Id)
            {
                return BadRequest();
            }

            approvalCommand.ApprovedBy = UserIdentity.UserName;
            approvalCommand.ApprovedByUserId = UserIdentity.UniversalId;
            var actionResponse = await _mediator.Send(approvalCommand);

            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpPut("Reject/{id}")]
        public async Task<IActionResult> Reject(int id, [FromBody] RejectClearingVoucherCommand rejectCommand)
        {
            if (id != rejectCommand.Id)
            {
                return BadRequest();
            }

            rejectCommand.ApprovedBy = UserIdentity.UserName;
            rejectCommand.ApprovedByUserId = UserIdentity.UniversalId;

            var actionResponse = await _mediator.Send(rejectCommand);

            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var clearing = _clearingQueries.Find(id);
            if (clearing == null)
            {
                return NotFound();
            }

            var deleteResponse = _clearingRepository.DeleteAndSave(id);

            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }
    }
}
