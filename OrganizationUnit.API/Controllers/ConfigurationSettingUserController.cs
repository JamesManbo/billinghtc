using System.Threading.Tasks;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrganizationUnit.API.Application.Commands.ConfigurationSettingUser;
using OrganizationUnit.Domain.Models.ConfigurationSettingUser;
using OrganizationUnit.Infrastructure.Queries;
using OrganizationUnit.Infrastructure.Repositories.ConfigurationSettingUserRepository;

namespace OrganizationUnit.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ConfigurationSettingUserController : CustomBaseController
    {
        private readonly ILogger<ConfigurationSettingUserController> _logger;
        private readonly IMediator _mediator;
        private readonly IConfigurationSettingUserRepository _configurationSettingUserRepository;
        private readonly IWrappedConfigAndMapper _configAndMapper;
        private readonly IConfigurationSettingUserQueryRepository _queryRepository;

        public ConfigurationSettingUserController(
            ILogger<ConfigurationSettingUserController> logger,
            IMediator mediator,
            IConfigurationSettingUserQueryRepository queryRepository,
            IConfigurationSettingUserRepository configurationSettingUserRepository,
            IWrappedConfigAndMapper configAndMapper)
        {
            _logger = logger;
            _mediator = mediator;
            _configurationSettingUserRepository = configurationSettingUserRepository;
            _configAndMapper = configAndMapper;
            _queryRepository = queryRepository;
        }

        //[HttpGet]
        //public IActionResult GetList([FromQuery] RequestFilterModel filterModel)
        //{
        //    return Ok(_queryRepository.GetList(filterModel));
        //}

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var entity = await _configurationSettingUserRepository.GetByIdAsync(id);
            return Ok(entity.MapTo<CUConfigurationSettingUserCommand>(_configAndMapper.MapperConfig));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CUConfigurationSettingUserCommand configurationSettingUserCommand)
        {
            var actionResponse = new ActionResponse<ConfigurationSettingUserDto>();
                
            actionResponse.CombineResponse(await _mediator.Send(configurationSettingUserCommand));
            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpGet("GetCurrentConfigAccount")]
        public ActionResult<IActionResponse<ConfigurationSettingUserDto>> GetCurrentConfigAccount()
        {
            var currentConfigAccount = _queryRepository.FindById(UserIdentity.Id); 
            if (currentConfigAccount != null)
            {
                return Ok(currentConfigAccount);
            }

            return BadRequest();
        }

        [HttpPut, Route("ChangeConfigAccount")]
        public async Task<IActionResult> ChangeConfigAccount([FromBody] CUConfigurationSettingUserCommand changeConfigModel)
         {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var actResponse = await _configurationSettingUserRepository.UpdateAndSave(changeConfigModel);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }

            return BadRequest(actResponse);
        } 

        public bool IsCheckAdminExportData()
        {
            return true;
        }
    }
}
