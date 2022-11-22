using ApplicationUserIdentity.API.Application.Commands.Industry;
using ApplicationUserIdentity.API.Infrastructure.Queries;
using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IndustriesController : CustomBaseController
    {
        private readonly IIndustryRepository _industryRepository;
        private readonly IIndustryQueries _industryQueries;

        public IndustriesController(IIndustryRepository industryRepository,
            IIndustryQueries industryQueries)
        {
            _industryRepository = industryRepository;
            _industryQueries = industryQueries;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_industryQueries.GetSelectionList(filterModel));
            }
            return Ok(_industryQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var industry = _industryQueries.FindById(id);

            if (industry == null)
            {
                return NotFound();
            }

            return Ok(industry);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CUIndustryCommand createCommand)
        {
            var validator = new CUIndustryValidator();
            var validateResult = await validator.ValidateAsync(createCommand);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            ActionResponse<IndustryDTO> rs = new ActionResponse<IndustryDTO>();
            var existName = _industryQueries.CheckExistName(createCommand.Id, createCommand.Name);
            var existCode = _industryQueries.CheckExistCode(createCommand.Id, createCommand.Code);
            if (existName)
            {
                rs.AddError("Tên lĩnh vực đã tồn tại", nameof(createCommand.Name));
            }
            if (existCode)
            {
                rs.AddError("Mã lĩnh vực đã tồn tại", nameof(createCommand.Code));
            }
            if (existName || existCode)
                return BadRequest(rs);

            var actResponse = await _industryRepository.CreateAndSave(createCommand);
            if (actResponse.IsSuccess)
            {
                return CreatedAtAction("GetById", new { id = actResponse.Result.Id }, actResponse);
            }
            return BadRequest(actResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CUIndustryCommand updateCommand)
        {
            if (id != updateCommand.Id)
            {
                return BadRequest();
            }

            var validator = new CUIndustryValidator();
            var validateResult = await validator.ValidateAsync(updateCommand);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            ActionResponse<IndustryDTO> rs = new ActionResponse<IndustryDTO>();
            var existName = _industryQueries.CheckExistName(updateCommand.Id, updateCommand.Name);
            var existCode = _industryQueries.CheckExistCode(updateCommand.Id, updateCommand.Code);
            if (existName)
            {
                rs.AddError("Tên lĩnh vực đã tồn tại", nameof(updateCommand.Name));
            }
            if (existCode)
            {
                rs.AddError("Mã lĩnh vực đã tồn tại", nameof(updateCommand.Code));
            }
            if (existName || existCode)
                return BadRequest(rs);

            var actionResponse = await _industryRepository.UpdateAndSave(updateCommand);

            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var industry = _industryQueries.FindById(id);
            if (industry == null)
            {
                return NotFound();
            }

            var deleteResponse = _industryRepository.DeleteAndSave(id);


            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }
    }
}
