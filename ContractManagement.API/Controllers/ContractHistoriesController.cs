using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Infrastructure.Repositories.ChangeHistoryRepository;
using Global.Models.Filter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContractHistoriesController : CustomBaseController
    {
        private readonly IContractHistoryRepository _changeHistoryRepository;

        public ContractHistoriesController(IContractHistoryRepository changeHistoryRepository)
        {
            _changeHistoryRepository = changeHistoryRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] RequestFilterModel filterModel)
        {
            return Ok(await _changeHistoryRepository.GetList(filterModel));
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _changeHistoryRepository.Get(id));
        }
        
    }
}