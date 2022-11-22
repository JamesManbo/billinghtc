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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BrasInfoController : CustomBaseController
    {
        private readonly ILogger<BrasInfoController> _logger;
        private readonly IMediator _mediator;

        private readonly IBrasInfoQueries _radiusServerInfoQueries;
        private readonly IBrasInfoRepository _radiusServerInfoRepository;
        private readonly IConfiguration _config;

        public BrasInfoController(ILogger<BrasInfoController> logger,
            IMediator mediator,
            IBrasInfoQueries radiusServerInfoQueries,
            IBrasInfoRepository radiusServerInfoRepository, 
            IConfiguration config)
        {
            _logger = logger;
            _mediator = mediator;
            _radiusServerInfoQueries = radiusServerInfoQueries;
            _radiusServerInfoRepository = radiusServerInfoRepository;
            this._config = config;
        }

        // TEST
        [HttpGet("get-configuration")]
        public IActionResult Get()
        {
            var result = _config.GetValue<string>("ConnectionStrings:DefaultConnection");
            var supporterRole = _config.GetValue<string>("DepartmentCode:BusinessDepartmentCode");
            var salesmanRole = _config.GetValue<string>("DepartmentCode:CustomerCareDepartmentCode");
            var cskhRole = _config.GetValue<string>("DepartmentCode:BoardOfDirectorsCode");
            var serviceProviderRole = _config.GetValue<string>("DepartmentCode:ServiceProviderDepartmentCode");
            return Ok($"{supporterRole} | {salesmanRole} | {cskhRole} | {serviceProviderRole}");
        }

        // GET: api/RadiusServerInfo
        [HttpGet]
        [PermissionAuthorize("VIEW_BRAS_INFO")]
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
        [PermissionAuthorize("VIEW_BRAS_INFO")]
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
        [PermissionAuthorize("ADD_BRAS_INFO")]
        public async Task<IActionResult> Post([FromBody] CuBrasInfoCommand command)
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
        [PermissionAuthorize("EDIT_BRAS_INFO")]
        public async Task<IActionResult> Put(int id, [FromBody] CuBrasInfoCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            var actionResponse = await _mediator.Send(command);
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
        [PermissionAuthorize("DELETE_BRAS_INFO")]
        public IActionResult Delete(int id)
        {
            var serviceModel = _radiusServerInfoQueries.Find(id);
            if (serviceModel == null)
            {
                return NotFound();
            }

            var deleteResponse = _radiusServerInfoRepository.DeleteAndSave(id);

            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }
    }
}
