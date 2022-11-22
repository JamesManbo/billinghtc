using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.ProjectModels;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProjectsController : CustomBaseController
    {
        private readonly IProjectService _projectService;
        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<ProjectDTO>>>> Get([FromQuery] RequestFilterModel filterModel)
        {
            var actResponse = await _projectService.GetList(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }

            //if (actResponse.Subset!=null && actResponse.Subset.Count>0) {
            //    for (int i = 0; i < actResponse.Subset.Count; i++)
            //    {
            //        var model = actResponse.Subset.ElementAt(i);
            //        model.NumberOfOutContracts = await _projectService.GetOutContactCount(model.Id);
            //    }
            //}
           
            return Ok(actResponse);
        }
    }
}