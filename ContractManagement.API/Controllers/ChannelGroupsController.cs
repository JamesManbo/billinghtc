using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Validations;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ChannelGroupRepository;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChannelGroupsController : CustomBaseController
    {
        private readonly ILogger<ChannelGroupsController> _logger;
        private readonly IMediator _mediator;
        public readonly IChannelGroupRepository _channelGroupRepository;
        public readonly IChannelGroupQueries _channelGroupQueries;

        public ChannelGroupsController(IChannelGroupRepository channelGroupRepository, IChannelGroupQueries channelGroupQueries)
        {
            _channelGroupRepository = channelGroupRepository;
            _channelGroupQueries = channelGroupQueries;
        }

        [HttpGet]
        public IActionResult GetChannelGroups([FromQuery] RequestFilterModel requestFilterModel)
        {
            if (requestFilterModel.Type == RequestType.Selection)
            {
                return Ok(_channelGroupQueries.GetSelectionList(requestFilterModel));
            }

            return Ok(_channelGroupQueries.GetList(requestFilterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var channelGroup = _channelGroupQueries.Find(id);

            if (channelGroup == null)
            {
                return NotFound();
            }

            return Ok(channelGroup);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutService(int id, ChannelGroupDTO channelGroupModel)
        {
            if (id != channelGroupModel.Id)
            {
                return BadRequest();
            }

            //var validator = new ServicesValidator();
            //var validateResult = await validator.ValidateAsync(serviceModel);
            //if (!validateResult.IsValid)
            //{
            //    return BadRequest(validateResult.Errors);
            //}

            ActionResponse<ChannelGroupDTO> rs = new ActionResponse<ChannelGroupDTO>();
            var existName = _channelGroupQueries.CheckExistName(id, channelGroupModel.ChannelGroupName);
            var existCode = _channelGroupQueries.CheckExistCode(id, channelGroupModel.ChannelGroupCode);
            if (existName)
            {
                rs.AddError("Tên chùm kênh đã tồn tại", nameof(channelGroupModel.ChannelGroupName));
            }
            if (existCode)
            {
                rs.AddError("Mã chùm kênh đã tồn tại", nameof(channelGroupModel.ChannelGroupCode));
            }
            if (existName || existCode)
                return BadRequest(rs);
            
            var actionResponse = await _channelGroupRepository.UpdateAndSave(channelGroupModel);
            if (actionResponse.IsSuccess)
            {
                return CreatedAtAction("GetById", new { id = actionResponse.Result.Id }, actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpPost]
        public async Task<IActionResult> PostService(ChannelGroupDTO channelGroupModel)
        {
            var validator = new ChannelGroupsValidator();
            var validateResult = await validator.ValidateAsync(channelGroupModel);
            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            ActionResponse<ChannelGroupDTO> rs = new ActionResponse<ChannelGroupDTO>();
            var existName = _channelGroupQueries.CheckExistName(channelGroupModel.Id, channelGroupModel.ChannelGroupName);
            var existCode = _channelGroupQueries.CheckExistCode(channelGroupModel.Id, channelGroupModel.ChannelGroupCode);
            if (existName)
            {
                rs.AddError("Tên chùm kênh đã tồn tại", nameof(channelGroupModel.ChannelGroupName));
            }
            if (existCode)
            {
                rs.AddError("Mã chùm kênh đã tồn tại", nameof(channelGroupModel.ChannelGroupCode));
            }
            if (existName || existCode)
                return BadRequest(rs);

            var actionResponse = await _channelGroupRepository.CreateAndSave(channelGroupModel);

            if (actionResponse.IsSuccess)
            {
                return CreatedAtAction("GetById", new { id = actionResponse.Result.Id }, actionResponse);
            }
            return BadRequest(actionResponse);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteService(int id)
        {
            var serviceModel = _channelGroupQueries.Find(id);
            if (serviceModel == null)
            {
                return NotFound();
            }

            var deleteResponse = _channelGroupRepository.DeleteAndSave(id);

            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }

        [Route("GenerateChannelGroupName")]
        [HttpGet]
        public IActionResult GenerateChannelGroupName(string serviceCode, string contractorIdGuid, string contractorFullName, int channelGroupType)
        {
            return Ok(_channelGroupQueries.GenerateChannelGroupName(serviceCode, contractorIdGuid, contractorFullName, channelGroupType));
        }
    }
}
