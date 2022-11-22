using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.PromotionModels;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PromotionsController : CustomBaseController
    {
        private readonly IPromotionService _promotionService;
        public PromotionsController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpGet]
        [Route("GetAvailablePromotionForContract")]
        public async Task<ActionResult<IActionResponse<List<AvailablePromotionDTO>>>> GetAvailablePromotionForContract(int ServiceId, int ServicePackageId, int OutContractServicePackageId)
        {
            var rs = await _promotionService.GetAvailablePromotionForContract(ServiceId, ServicePackageId, OutContractServicePackageId);
            return Ok(rs);
        }
    }
}