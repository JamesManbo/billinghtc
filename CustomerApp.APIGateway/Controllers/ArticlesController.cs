using System.Threading.Tasks;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Mvc;
using CustomerApp.APIGateway.Models.ArticleModels;
using CustomerApp.APIGateway.Models.RequestModels;
using CustomerApp.APIGateway.Services;


namespace CustomerApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ArticlesController : CustomerBaseController
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
    }
}
