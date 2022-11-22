using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.Commands.TransactionServicePackageCommand;
using ContractManagement.Infrastructure.Queries;
using Global.Models.Filter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionServicePackagesController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly ITransactionServicePackageQueries _transactionServicePackageQueries;


        public TransactionServicePackagesController(IMediator mediator, ITransactionServicePackageQueries transactionServicePackageQueries)
        {
            _mediator = mediator;
            _transactionServicePackageQueries = transactionServicePackageQueries;

        }
        [HttpGet]
        public IActionResult GetAllTransactionServicePackage([FromQuery] RequestFilterModel filterModel)
        {

            return Ok(_transactionServicePackageQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetTransactionServicePackageById(int id)
        {
            var transactionServicePackage = _transactionServicePackageQueries.Find(id);

            if (transactionServicePackage == null)
            {
                return NotFound();
            }


            return Ok(transactionServicePackage);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransactionServicePackage(int id, [FromBody] CUTransactionServicePackageCommand promotionCommand)
        {
            if (id != promotionCommand.Id)
            {
                return BadRequest();
            }

            promotionCommand.UpdatedBy = UserIdentity.UserName;
            promotionCommand.UpdatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(promotionCommand);
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
        public async Task<IActionResult> CreateTransactionServicePackage([FromBody] CUTransactionServicePackageCommand transactionServicePackageCommand)
        {
            transactionServicePackageCommand.CreatedBy = UserIdentity.UserName;
            transactionServicePackageCommand.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(transactionServicePackageCommand);
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