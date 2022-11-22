
using System.Collections.Generic;
using System.Threading.Tasks;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.ArticleModels;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArticlesController : CustomBaseController
    {
        private readonly IArticlesService _articlesService;
        public ArticlesController(IArticlesService articlesService)
        {
            _articlesService = articlesService;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<ArticleDTO>>>> Get([FromQuery] ArticleFilterModel filterModel)
        {
            var actResponse = await _articlesService.GetList(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }

        [HttpGet]
        [Route("GetArticleTypes")]
        
        public async Task<ActionResult<IActionResponse<IEnumerable<ArticleTypeDTO>>>> GetArticleTypes()
        {
            var actResponse = await _articlesService.GetListArticleType();
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }
    }
}
