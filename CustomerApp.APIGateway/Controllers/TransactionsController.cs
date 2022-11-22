using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerApp.APIGateway.Models.RequestModels;
using CustomerApp.APIGateway.Models.TransactionModels;
using CustomerApp.APIGateway.Models.TransactionModels.RequestApp;
using CustomerApp.APIGateway.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CustomerApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransactionsController : CustomerBaseController
    {
        private readonly ITransactionsService _transactionsService;
        private readonly IContractService _contractService;
        private readonly IMapper _mapper;
        public TransactionsController(ITransactionsService transactionsService, IContractService contractService, IMapper mapper)
        {
            _transactionsService = transactionsService;
            _contractService = contractService;
            _mapper = mapper;
        }

        [HttpPost("AddNewServicePackage")]
        public async Task<IActionResult> Post([FromBody] CUAddNewServicePackageTransactionApp reqApp)
        {
            var command = _mapper.Map<CUAddNewServicePackageTransaction>(reqApp);
            var contract = await _contractService.GetDetail(command.OutContractId);
            if (contract == null) return BadRequest();

            command.CreatedBy = UserIdentity.UserName;
            command.ContractorId = contract.ContractorId;
            command.ContractType = contract.ContractTypeId;
            command.MarketAreaId = contract.MarketAreaId;
            command.MarketAreaName = contract.MarketAreaName;
            command.ProjectId = contract.ProjectId;
            command.ProjectName = contract.ProjectName;

            if (contract.ServicePackages!=null && contract.ServicePackages.Count>0)
            {
                command.CurrencyUnitId = contract.ServicePackages.ElementAt(0).CurrencyUnitId;
                command.CurrencyUnitCode = contract.ServicePackages.ElementAt(0).CurrencyUnitCode;
            }
            if (command.TransactionServicePackages !=null && command.TransactionServicePackages.Count>0)
            {
                command.TransactionServicePackages.ForEach(e =>
                {
                    e.PaymentTarget = contract.Contractor;
                    e.CreateFromCustomer = true;
                });
                    
            }
            

            command.StatusId = 1; //WaitApprove
            command.TransactionDate = DateTime.Now;
            command.Type = 1; //AddNewServicePackage

            var request = new RequestAddNewTransaction();
            request.TransactionJSON = JsonConvert.SerializeObject(command);
            request.TypeId = 1; //AddNewServicePackage

            var actResponse = await _transactionsService.AddNewTransaction(request);
            if (actResponse!=null && actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                if (actResponse.Errors != null && actResponse.Errors.Count > 0)
                {
                    return BadRequest(actResponse.Errors[0].ErrorMessage);
                }
                return BadRequest(actResponse);
            }
        }
    }
}
