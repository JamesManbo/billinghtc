using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DebtManagement.Infrastructure.Queries;
using Global.Models.Filter;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DebtManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class VoucherTargetsController : CustomBaseController
    {
        private readonly IVoucherTargetQueries _voucherTargetQueries;

        public VoucherTargetsController(
            IVoucherTargetQueries voucherTargetQueries)
        {
            _voucherTargetQueries = voucherTargetQueries;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Autocomplete)
            {
                return Ok(_voucherTargetQueries.AutoComplete(filterModel));
            }
            return Ok(_voucherTargetQueries.GetList(filterModel));
        }

        [HttpGet("partner")]
        public IActionResult GetPartners([FromQuery] RequestFilterModel filterModel)
        {
            return Ok(_voucherTargetQueries.AutoComplete(filterModel, false));
        }

        [HttpGet("for-clearing")]
        public IActionResult AutocompleteForClearing([FromQuery] RequestFilterModel filterModel)
        {
            return Ok(_voucherTargetQueries.AutoCompleteForClearing(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetVoucherTargetById(int id)
        {
            var voucherTarget = _voucherTargetQueries.Find(id);

            if (voucherTarget == null)
            {
                return NotFound();
            }

            return Ok(voucherTarget);
        }
    }
}
