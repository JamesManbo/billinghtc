using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.API.Grpc.Clients.Organizations;
using ContractManagement.Domain.Commands.ProjectCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ProjectRepository;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContractManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProjectsController : CustomBaseController
    {
        private readonly ILogger<ProjectsController> _logger;
        private readonly IMediator _mediator;
        private readonly IProjectQueries _queries;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IProjectRepository _projectRepository;
        private readonly IUserGrpcService _userGrpcService;


        public ProjectsController(ILogger<ProjectsController> logger,
            IMediator mediator,
            IProjectQueries queries,
            IProjectRepository projectRepository,
            IWrappedConfigAndMapper configAndMapper,
            IUserGrpcService userGrpcService)
        {
            _logger = logger;
            _mediator = mediator;
            _projectRepository = projectRepository;
            _queries = queries;
            _configAndMapper = configAndMapper;
            _userGrpcService = userGrpcService;
        }

        [HttpGet]
        public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_queries.GetSelectionList());
            }

            return Ok(_queries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var project = _queries.GetById(id);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        [HttpGet("GetAvaliableSupporterInProjectByProjectId/{projectId}")]        
        public async Task<IActionResult> GetAvaliableSupporterInProjectByProjectId(int projectId)
        {            
            var x = _queries.GetAvaliableSupporterInProjectByProjectId(projectId);
            if (x != null && x.Any())
            {
                return Ok(await _userGrpcService.GetUserByIds(string.Join(",", x)));
            }
            return Ok();
        }
       

        //[HttpGet("{id}")]
        //public IActionResult GetProject(int id)
        //{
        //    var serviceGroup = _queries.Find(id);

        //    if (serviceGroup == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(serviceGroup);
        //}

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CUProjectCommand createProjectCommand)
        {
            var actionResponse = await _mediator.Send(createProjectCommand);
            if (actionResponse.IsSuccess)
            {
                return CreatedAtAction("GetById", new {id = actionResponse.Result.Id}, actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CUProjectCommand updateProjectCommand)
        {
            if (id != updateProjectCommand.Id)
            {
                return BadRequest();
            }

            var actionResponse = await _mediator.Send(updateProjectCommand);
            if (actionResponse.IsSuccess)
            {
                return CreatedAtAction("GetById", new { id = id }, actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok(_projectRepository.DeleteAndSave(id));
        }
    }
}