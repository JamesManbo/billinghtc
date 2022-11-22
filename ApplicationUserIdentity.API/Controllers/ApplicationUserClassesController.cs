using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApplicationUserIdentity.API.Infrastructure;
using ApplicationUserIdentity.API.Infrastructure.Queries;
using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using Global.Models.Filter;
using Global.Models.PagedList;
using ApplicationUserIdentity.API.Infrastructure.Validations;
using Global.Models.StateChangedResponse;
using ApplicationUserIdentity.API.IntegrationEvents;

namespace ApplicationUserIdentity.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationUserClassesController : CustomBaseController
    {
        private readonly IUserClassQueries _userClassQueries;
        private readonly IUserClassRepository _userClassRepository;

        public ApplicationUserClassesController(IUserClassQueries userClassQueries, 
            IUserClassRepository userClassRepository)
        {
            _userClassQueries = userClassQueries;
            _userClassRepository = userClassRepository;
        }

        // GET: api/ApplicationUserClasses
        [HttpGet]
        public IActionResult GetApplicationUsers([FromQuery] RequestFilterModel filterModel)
        {
            if(filterModel.Type == RequestType.Selection)
                return Ok(_userClassQueries.GetSelectionList());

            return Ok(_userClassQueries.GetList(filterModel));
        }

        // GET: api/ApplicationUserClasses/5
        [HttpGet("{id}")]
        public IActionResult GetApplicationUserClass(int id)
        {
            var applicationUser = _userClassQueries.Find(id);

            if (applicationUser == null)
            {
                return NotFound();
            }

            return Ok(applicationUser);
        }

        // PUT: api/ApplicationUserClasses/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplicationUserClass(int id, UserClassViewModel userClass)
        {
            if (id != userClass.Id)
            {
                return BadRequest();
            }

            var validator = new ApplicationUserClassesValidator();
            var validateResult = await validator.ValidateAsync(userClass);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            ActionResponse<UserClassViewModel> rs = new ActionResponse<UserClassViewModel>();
            var existName = _userClassQueries.CheckExistName(id, userClass.ClassName);
            var existCode = _userClassQueries.CheckExistCode(id, userClass.ClassCode);

            if (existName)
            {
                rs.AddError("Tên hạng khách hàng đã tồn tại", nameof(userClass.ClassName));
            }


            if (existCode)
            {
                rs.AddError("Mã hạng khách hàng đã tồn tại", nameof(userClass.ClassCode));
            }

            if (existName || existCode)
                return BadRequest(rs);

            var actionResponse = await _userClassRepository.UpdateAndSave(userClass);
            if (actionResponse.IsSuccess)
            {
                return Ok(actionResponse);
            }

            return BadRequest(actionResponse);
        }

        // POST: api/ApplicationUserClasses
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ApplicationUserClass>> PostApplicationUserClass(UserClassViewModel userClass)
        {
            var validator = new ApplicationUserClassesValidator();
            var validateResult = await validator.ValidateAsync(userClass);

            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult.Errors);
            }

            ActionResponse<UserClassViewModel> rs = new ActionResponse<UserClassViewModel>();
            var existName = _userClassQueries.CheckExistName(userClass.Id, userClass.ClassName);
            var existCode = _userClassQueries.CheckExistCode(userClass.Id, userClass.ClassCode);
            if (existName)
            {
                rs.AddError("Tên hạng khách hàng đã tồn tại", nameof(userClass.ClassName));
            }
            if (existCode)
            {
                rs.AddError("Mã hạng khách hàng đã tồn tại", nameof(userClass.ClassCode));
            }
            if (existName || existCode)
                return BadRequest(rs);
            var actionResponse = await _userClassRepository.CreateAndSave(userClass);
            if (actionResponse.IsSuccess)
            {
                return CreatedAtAction("GetApplicationUserClass", new { id = actionResponse.Result.Id }, actionResponse);
            }

            return BadRequest(actionResponse);
        }

        // DELETE: api/ApplicationUserClasses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApplicationUserClass>> DeleteApplicationUserClass(int id)
        {
            var applicationUser = _userClassQueries.Find(id);
            if (applicationUser == null)
            {
                return NotFound();
            }

            var deleteResponse = _userClassRepository.DeleteAndSave(id);

            if (deleteResponse.IsSuccess)
            {
                return Ok(deleteResponse);
            }

            return BadRequest(deleteResponse);
        }
    }
}
