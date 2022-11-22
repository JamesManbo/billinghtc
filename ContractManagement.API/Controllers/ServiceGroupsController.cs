using System.Threading.Tasks;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Validations;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ServiceGroupRepository;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServiceGroupsController : CustomBaseController
    {
        private readonly IServiceGroupsQueries _serviceGroupsQueries;
        private readonly IServiceGroupsRepository _serviceGroupsRepository;
        public ServiceGroupsController(IServiceGroupsQueries serviceGroupsQueries, IServiceGroupsRepository serviceGroupsRepository)
        {
            _serviceGroupsQueries = serviceGroupsQueries;
            _serviceGroupsRepository = serviceGroupsRepository;
            //_serviceGroupsQueries.RestrictByOrganization();
        }

        [HttpGet]
        public ActionResult<IPagedList<ServiceGroupDTO>> GetServiceGroups([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_serviceGroupsQueries.GetSelectionList(filterModel));
            }
            return Ok(_serviceGroupsQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetServiceGroup(int id)
        {
            var serviceGroup = _serviceGroupsQueries.Find(id);

            if (serviceGroup == null)
            {
                return NotFound();
            }

            return Ok(serviceGroup);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutServiceGroup(int id, ServiceGroupDTO serviceGroup)
        {
            if (id != serviceGroup.Id)
            {
                return BadRequest();
            }

            var validator = new ServiceGroupsValidator();
            var validateResult = await validator.ValidateAsync(serviceGroup);
            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            ActionResponse<ServiceGroupDTO> rs = new ActionResponse<ServiceGroupDTO>();

            var existName = _serviceGroupsQueries.CheckExistName(id, serviceGroup.GroupName);
            var existCode = _serviceGroupsQueries.CheckExistCode(id, serviceGroup.GroupCode);
            if (existName)
            {
                rs.AddError("Tên nhóm dịch vụ đã tồn tại", nameof(serviceGroup.GroupName));
            }

            if (existCode)
            {
                rs.AddError("Mã nhóm dịch vụ đã tồn tại", nameof(serviceGroup.GroupCode));
            }

            if (existName || existCode)
                return BadRequest(rs);

            var actionResponse = await _serviceGroupsRepository.UpdateAndSave(serviceGroup);
            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpPost]
        public async Task<IActionResult> PostServiceGroup(ServiceGroupDTO serviceGroup)
        {
            var validator = new ServiceGroupsValidator();
            var validateResult = await validator.ValidateAsync(serviceGroup);
            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }


            ActionResponse<ServiceGroupDTO> rs = new ActionResponse<ServiceGroupDTO>();
            var existName = _serviceGroupsQueries.CheckExistName(serviceGroup.Id, serviceGroup.GroupName);
            var existCode = _serviceGroupsQueries.CheckExistCode(serviceGroup.Id, serviceGroup.GroupCode);
            if (existName)
            {
                rs.AddError("Tên nhóm dịch vụ đã tồn tại", nameof(serviceGroup.GroupName));
            }
            if (existCode)
            {
                rs.AddError("Loại nhóm dịch vụ đã tồn tại", nameof(serviceGroup.GroupCode));
            }
            if(existName || existCode)
                return BadRequest(rs);

            var actionResponse = await _serviceGroupsRepository.CreateAndSave(serviceGroup);
            if (actionResponse.IsSuccess)
            {
                return CreatedAtAction("GetServiceGroup", new { id = actionResponse.Result.Id }, actionResponse);
            }

            return BadRequest(actionResponse);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteServiceGroup(int id)
        {
            var serviceGroup = _serviceGroupsQueries.Find(id);
            if (serviceGroup == null)
            {
                return NotFound();
            }

            var deleteResponse = _serviceGroupsRepository.DeleteAndSave(id);

            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }
    }
}