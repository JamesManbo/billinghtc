using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Services;
using System.Threading.Tasks;
using StaffApp.APIGateway.Models.MarketAreaModels;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MarketAreasController : CustomBaseController
    {
        private readonly IMarketAreaService _marketAreasService;
        public MarketAreasController(IMarketAreaService marketAreasService)
        {
            _marketAreasService = marketAreasService;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<MarketAreaModelDTO>>>> Get([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.SimpleAll)
            {
                return Ok(await _marketAreasService.GetAll());
            }

            return Ok(await _marketAreasService.GetList(filterModel));
        }
    }
}