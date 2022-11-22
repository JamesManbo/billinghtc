using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContractManagement.Domain.Commands.PromotionCommand;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.PromotionRepository;
using Global.Models.Filter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PromotionTypeController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly IPromotionTypeQueries _promotionTypeQueries;
       
        
        public PromotionTypeController( IMediator mediator, IPromotionTypeQueries promotionTypeQueries) { 
            _mediator = mediator;
            _promotionTypeQueries = promotionTypeQueries;
           
        }
        [HttpGet]
        public IActionResult GetAllPromotion([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_promotionTypeQueries.GetSelectionList(filterModel));
            }

            //if (filterModel.Type == RequestType.SimpleAll)
            //{
            //    return Ok(_promotionQueries.GetAllSimple(filterModel));
            //}

            return Ok(_promotionTypeQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetPromotionById(int id)
        {
            var promotion = _promotionTypeQueries.GetPromotionById(id);

            if (promotion == null)
            {
                return NotFound();
            }

          //  promotion.lstPromotionDetail = _promotionQueries.GetPackagePriceOfProject(id);

            return Ok(promotion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePromotion(int id, [FromBody] CUPromotionCommand promotionCommand)
        {
            if (id != promotionCommand.Id)
            {
                return BadRequest();
            }

            promotionCommand.UpdatedBy = UserIdentity.UserName;
            promotionCommand.UpdatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(promotionCommand);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePromotion([FromBody] CUPromotionCommand promotionCommand)
        {
            promotionCommand.CreatedBy = UserIdentity.UserName;
            promotionCommand.CreatedDate = DateTime.Now;

            var actResponse = await _mediator.Send(promotionCommand);
            if (actResponse.IsSuccess)
            {
                return Ok(actResponse);
            }
            else
            {
                return BadRequest(actResponse);
            }
        }
    }
}