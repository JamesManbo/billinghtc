using System.Threading.Tasks;
using Global.Models.Filter;
using Location.API.Commands;
using Location.API.Location;
using Location.API.Reponsitory;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Location.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LocationsController : CustomBaseController
    {
        private readonly ISupportLocationQueries _spLocationQueries;
        private readonly ILocationQueries _locationQueries;
        private readonly IMediator _mediator;

        public LocationsController(IMediator mediator, ISupportLocationQueries spLocationQueries, ILocationQueries locationQueries)
        {
            _spLocationQueries = spLocationQueries;
            _locationQueries = locationQueries;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] LocationRequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection || filterModel.Type == RequestType.Hierarchical)
            {
                return Ok(await _locationQueries.GetSelectionLocation(filterModel));
            }

            if(filterModel.Type == RequestType.SimpleAll)
            {
                return Ok(await _locationQueries.GetAll());
            }

            return Ok(await _locationQueries.GetListLocation(filterModel));
        }

        [HttpGet("GetAllLocation")]
        public async Task<IActionResult> GetAllLocation()
        {
            return Ok(await _locationQueries.GetAll());
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _locationQueries.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> PostContract([FromBody] CULocationCommand cuLocationCommand)
        {
            var validator = new CULocationValidator();
            var validateResult = validator.Validate(cuLocationCommand);
            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            var respone = await _mediator.Send(cuLocationCommand);

            if (respone != null)
                return Ok(respone);
            else
                return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] CULocationCommand cuLocationCommand)
        {
            if (id != cuLocationCommand.Id)
            {
                return BadRequest();
            }

            var validator = new CULocationValidator();
            var validateResult = validator.Validate(cuLocationCommand);
            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            var respone = await _mediator.Send(cuLocationCommand);
            if (respone != null)
                return Ok(respone);
            else
                return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            string message = await _locationQueries.DeleteLocation(id);
            if (message == "Success")
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        #region Support location

        [HttpGet]
        [Route("SupportLocation")]
        public async Task<IActionResult> GetAllSupportLocation([FromQuery] SupportLocationRequestFilterModel filterModel)
        {
            return Ok(await _spLocationQueries.GetList(filterModel));
        }

        #endregion
    }
}