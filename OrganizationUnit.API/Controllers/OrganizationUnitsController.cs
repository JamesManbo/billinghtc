using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrganizationUnit.API.Application.Commands.OrganizationUnit;
using OrganizationUnit.Domain.Models.OrganizationUnit;
using OrganizationUnit.Infrastructure.Queries;
using OrganizationUnit.Infrastructure.Repositories.OrganizationUnitRepository;

namespace OrganizationUnit.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrganizationUnitsController : CustomBaseController
    {
        private readonly ILogger<OrganizationUnitsController> _logger;
        private readonly IMediator _mediator;
        private readonly IOrganizationUnitRepository _organizationUnitRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IOrganizationUnitQueryRepository _organizationUnitQueries;

        public OrganizationUnitsController(
            ILogger<OrganizationUnitsController> logger,
            IMediator mediator,
            IOrganizationUnitQueryRepository queryRepository,
            IOrganizationUnitRepository organizationUnitRepository,
            IWrappedConfigAndMapper configAndMapper)
        {
            _logger = logger;
            _mediator = mediator;
            _organizationUnitRepository = organizationUnitRepository;
            _configAndMapper = configAndMapper;
            _organizationUnitQueries = queryRepository;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_organizationUnitQueries.GetSelectionList());
            }
            else if (filterModel.Type == RequestType.Hierarchical)
            {
                return Ok(_organizationUnitQueries.GetHierarchicalList());
            }

            return Ok(_organizationUnitQueries.GetList(filterModel));
        }

        [HttpGet("GetAutocompleteParents")]
        public IActionResult GetAutocompleteParents(string name)
        {
           return Ok(_organizationUnitQueries.GetAutocompleteParents(name));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(_organizationUnitQueries.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrganizationUnitCommand createOrganizationUnitCommand)
        {
            var actionResponse = new ActionResponse<OrganizationUnitDTO>();
            createOrganizationUnitCommand.IdentityGuid = Guid.NewGuid().ToString();
            actionResponse.CombineResponse(await _mediator.Send(createOrganizationUnitCommand));
            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);          
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateOrganizationUnitCommand updateOrganizationUnitCommand)
        {
            if (id != updateOrganizationUnitCommand.Id)
            {
                return BadRequest();
            }

            var actionResponse = new ActionResponse<OrganizationUnitDTO>();
            actionResponse.CombineResponse(await _mediator.Send(updateOrganizationUnitCommand));
            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_organizationUnitRepository.IsExisted(id)) return BadRequest();

            return Ok(_organizationUnitRepository.DeleteAndSave(id));
        }

        [HttpGet("GetOrganizationUnitIds/{parentId}")]
        public IActionResult GetOrganizationUnitIds(int? parentId)
        {
            return Ok(_organizationUnitRepository.GetOrganizationUnitIds(parentId));
        }
    }
}