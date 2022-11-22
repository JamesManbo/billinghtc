using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Models.AccountViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ApplicationUserIndustriesController : CustomBaseController
    {
        private readonly IApplicationUserIndustryRepository _industryRepository;

        public ApplicationUserIndustriesController(IApplicationUserIndustryRepository industryRepository)
        {
            _industryRepository = industryRepository;
        }

        [HttpDelete()]
        [Route("DeleteMapIndustryByUserIdAndIndustryId")]
        public async Task<IActionResult> DeleteMapIndustryByUserIdAndIndustryId(int userId, int industryId)
        {

            await _industryRepository.DeleteMapIndustryByUserIdAndIndustryId(userId, industryId);

            return Ok();
        }

        [HttpPost()]
        [Route("AddMapUsersIndustry")]
        public async Task<IActionResult> AddMapUsersIndustry(AddMapUsersAndIndustryModel req)
        {
            if (req.UserIds == null || req.UserIds.Count == 0 || req.IndustryId <= 0)
            {
                return BadRequest();
            }

            await _industryRepository.AddMapUsersIndustry(req.UserIds, req.IndustryId);

            return Ok();
        }
    }
}
