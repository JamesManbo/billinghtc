using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.Commands.TransactionEquipmentCommand;
using ContractManagement.Infrastructure.Queries;
using Global.Models.Filter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionEquipmentsController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly ITransactionEquipmentQueries _transactionEquipmentQueries;


        public TransactionEquipmentsController(IMediator mediator, ITransactionEquipmentQueries transactionEquipmentQueries)
        {
            _mediator = mediator;
            _transactionEquipmentQueries = transactionEquipmentQueries;

        }
        [HttpGet]
        public IActionResult GetAllTransactionEquipment([FromQuery] RequestFilterModel filterModel)
        {

            return Ok(_transactionEquipmentQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetTransactionEquipmentById(int id)
        {
            var transactionEquipment = _transactionEquipmentQueries.Find(id);

            if (transactionEquipment == null)
            {
                return NotFound();
            }

            return Ok(transactionEquipment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransactionEquipment(int id, [FromBody] CUTransactionEquipmentCommand transactionEquipmentCommand)
        {
            if (id != transactionEquipmentCommand.Id)
            {
                return BadRequest();
            }

            transactionEquipmentCommand.UpdatedBy = UserIdentity.UserName;
            transactionEquipmentCommand.UpdatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(transactionEquipmentCommand);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransactionEquipment([FromBody] CUTransactionEquipmentCommand transactionEquipmentCommand)
        {
            transactionEquipmentCommand.CreatedBy = UserIdentity.UserName;
            transactionEquipmentCommand.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(transactionEquipmentCommand);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }

        }
    }
}