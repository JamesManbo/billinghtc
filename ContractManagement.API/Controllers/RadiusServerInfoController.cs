using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.API.PolicyBasedAuthProvider;
using ContractManagement.Domain.Commands.RadiusAndBrasCommand;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.RadiusAndBrasRepository;
using Global.Models.Filter;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RadiusServerInfoController : CustomBaseController
    {
        private readonly ILogger<RadiusServerInfoController> _logger;
        private readonly IMediator _mediator;

        private readonly IRadiusServerInfoQueries _radiusServerInfoQueries;
        private readonly IRadiusServerInfoRepository _radiusServerInfoRepository;

        public RadiusServerInfoController(ILogger<RadiusServerInfoController> logger, 
            IMediator mediator, 
            IRadiusServerInfoQueries radiusServerInfoQueries, 
            IRadiusServerInfoRepository radiusServerInfoRepository)
        {
            _logger = logger;
            _mediator = mediator;
            _radiusServerInfoQueries = radiusServerInfoQueries;
            _radiusServerInfoRepository = radiusServerInfoRepository;
        }

        // GET: api/RadiusServerInfo
        [HttpGet]
        [PermissionAuthorize("VIEW_RADIUS_SERVER_INFO")]
        public IActionResult Get([FromQuery] RequestFilterModel requestFilterModel)
        {
            if (requestFilterModel.Type == RequestType.Selection)
            {
                return Ok(_radiusServerInfoQueries.GetSelectionList());
            }

            return Ok(_radiusServerInfoQueries.GetList(requestFilterModel));
        }

        // GET: api/RadiusServerInfo/5
        [HttpGet("{id}")]
        [PermissionAuthorize("VIEW_RADIUS_SERVER_INFO")]
        public IActionResult GetDetail(int id)
        {
            var equipment = _radiusServerInfoQueries.Find(id);

            if (equipment == null)
            {
                return NotFound();
            }

            return Ok(equipment);
        }

        // POST: api/RadiusServerInfo
        [HttpPost]
        [PermissionAuthorize("ADD_RADIUS_SERVER_INFO")]
        public async Task<IActionResult> Post([FromBody] CuRadiusServerInfoCommand command)
        {
            var actionResponse = await _mediator.Send(command);

            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        // PUT: api/RadiusServerInfo/5
        [HttpPut("{id}")]
        [PermissionAuthorize("EDIT_RADIUS_SERVER_INFO")]
        public async Task<IActionResult> Put(int id, [FromBody] CuRadiusServerInfoCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            var actionResponse  = await _mediator.Send(command);
            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }
            else
            {
                return BadRequest(actionResponse);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        [PermissionAuthorize("DELETE_RADIUS_SERVER_INFO")]
        public IActionResult Delete(int id)
        {
            return Ok(_radiusServerInfoRepository.DeleteAndSave(id));
        }
    }
}
