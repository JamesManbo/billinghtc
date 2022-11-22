using Location.API.Commands;
using Location.API.Location;
using Location.API.Model;
using Location.API.Reponsitory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Location.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SupportLocationController : CustomBaseController
    {
        private readonly ISupportLocationQueries _spLocationQueries;
        
        private readonly IMediator _mediator;


        public SupportLocationController(IMediator mediator, ISupportLocationQueries spLocationQueries)
        {
            _spLocationQueries = spLocationQueries;            
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CUSupportLocationCommand cuLocationCommand)
        {
            var validator = new SupportLocationValidator();
            var validateResult = validator.Validate(cuLocationCommand);
            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            var location = new SupportLocation()
            {
                Name = cuLocationCommand.Name,
                Code = cuLocationCommand.Code,
                Phone = cuLocationCommand.Phone,
                Path = cuLocationCommand.Path,
                Address = cuLocationCommand.Address,
                Level = cuLocationCommand.Level,
                X = cuLocationCommand.X,
                Y = cuLocationCommand.Y,
                DateCreated = DateTime.Now
            };
            await _spLocationQueries.Create(location);
            return Ok();
        }

        [HttpGet]        
        public async Task<IActionResult> GetAllSupportLocation([FromQuery] SupportLocationRequestFilterModel filterModel)
        {
            return Ok(await _spLocationQueries.GetList(filterModel));
        }
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetById(string id)
        {
            return Ok(await _spLocationQueries.GetById(id));
        }

        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] CUSupportLocationCommand cuLocationCommand)
        {

            var oldSLocation = await _spLocationQueries.GetById(cuLocationCommand.Id);

            if (oldSLocation == null)
            {
                return NotFound();
            }

            var validator = new SupportLocationValidator();
            var validateResult = validator.Validate(cuLocationCommand);
            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            var supportLocation = new SupportLocation()
            {
                Id = oldSLocation.Id,
                Name = cuLocationCommand.Name,
                Code = cuLocationCommand.Code,
                Phone = cuLocationCommand.Phone,
                Path = cuLocationCommand.Path,
                Address = cuLocationCommand.Address,
                Level = cuLocationCommand.Level,
                X = cuLocationCommand.X,
                Y = cuLocationCommand.Y,
                DateUpdated = DateTime.Now,
            };

            var response = await _spLocationQueries.Update(supportLocation);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var rs = await _spLocationQueries.Delete(id);
            if (rs)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
