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
using StaffApp.APIGateway.Models.TaxCategoryModels;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaxCategoriesController : CustomBaseController
    {
        private readonly ITaxCategoryService _taxCategoryService;
        public TaxCategoriesController(ITaxCategoryService taxCategoryService)
        {
            _taxCategoryService = taxCategoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<TaxCategoryDTO>>>> Get([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.SimpleAll)
            {
                return Ok(await _taxCategoryService.GetAll());
            }
            else
            {
                return Ok(await _taxCategoryService.GetList(filterModel));
            }
        }
    }
}