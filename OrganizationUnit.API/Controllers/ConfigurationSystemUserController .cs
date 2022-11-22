using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrganizationUnit.API.Application.Commands.ConfigurationSettingUser;
using OrganizationUnit.Domain.Models.ConfigurationSettingUser;
using OrganizationUnit.Infrastructure.Queries;
using OrganizationUnit.Infrastructure.Repositories.ConfigurationSettingUserRepository;
using OrganizationUnit.Infrastructure.Repositories.ConfigurationSystemUserRepository;

namespace OrganizationUnit.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConfigurationSystemUserController : CustomBaseController
    {
        private readonly ILogger<ConfigurationSystemUserController> _logger;
        private readonly IMediator _mediator;
        private readonly IConfigurationSystemUserRepository _configurationSystemUserRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IConfigurationSystemUserQueryRepository _queryRepository;

        public ConfigurationSystemUserController(
            ILogger<ConfigurationSystemUserController> logger,
            IMediator mediator,
            IConfigurationSystemUserQueryRepository queryRepository,
            IConfigurationSystemUserRepository configurationSystemUserRepository,
            IWrappedConfigAndMapper configAndMapper)
        {
            _logger = logger;
            _mediator = mediator;
            _configurationSystemUserRepository = configurationSystemUserRepository;
            _configAndMapper = configAndMapper;
            _queryRepository = queryRepository;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _configurationSystemUserRepository.GetByIdAsync(id);
            return Ok(entity.MapTo<ConfigurationSystemParameterDto>(_configAndMapper.MapperConfig));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CUConfigurationSystemParameterCommand configurationSystemParameter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var actResponse = await _configurationSystemUserRepository.CreateAndSave(configurationSystemParameter);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }

            return BadRequest(actResponse);
        }

        [HttpGet("GetCurrentConfigSystemUser")]
        public ActionResult<IActionResponse<ConfigurationSystemParameterDto>> GetCurrentConfigSystemUser()
        {
            var currentConfigSystemUser = _queryRepository.Find();
            if (currentConfigSystemUser != null)
            {
                return Ok(currentConfigSystemUser);
            }

            return BadRequest();
        }

        [HttpPut, Route("ChangeConfigSystemUser")]
        public async Task<IActionResult> ChangeConfigSystemUser([FromBody] CUConfigurationSystemParameterCommand changeConfigModel)
         {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var actResponse = await _configurationSystemUserRepository.UpdateAndSave(changeConfigModel);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }

            return BadRequest(actResponse);
        }

        [HttpGet("CheckAdminExportData")]
        public IActionResult CheckAdminExportData()
        {
            var isCheckAdminExportData =  _configurationSystemUserRepository.IsCheckAdminExportData();

            return Ok(isCheckAdminExportData);
        }
    }
}
