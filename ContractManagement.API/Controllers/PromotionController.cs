using System;
using System.Threading.Tasks;
using ContractManagement.Domain.Commands.PromotionCommand;
using ContractManagement.Domain.Models;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.PromotionRepository;
using Global.Models.Filter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContractManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PromotionController : CustomBaseController
    {
        private readonly IMediator _mediator;
        private readonly IPromotionQueries _promotionQueries;
        private readonly IPromotionRepository _promotionRepository;
        
        public PromotionController( IMediator mediator, IPromotionQueries promotionQueries
                                    , IPromotionRepository promotionRepository) { 
            _mediator = mediator;
            _promotionQueries = promotionQueries;
            _promotionRepository = promotionRepository;
        }
        [HttpGet]
        public IActionResult GetAllPromotion([FromQuery] RequestFilterModel filterModel)
        {
            if (filterModel.Type == RequestType.Selection)
            {
                return Ok(_promotionQueries.GetSelectionList(filterModel));
            }

            //if (filterModel.Type == RequestType.SimpleAll)
            //{
            //    return Ok(_promotionQueries.GetAllSimple(filterModel));
            //}

            return Ok(_promotionQueries.GetList(filterModel));
        }

        [HttpGet("{id}")]
        public IActionResult GetPromotionById(int id)
        {
            var promotion = _promotionQueries.GetPromotionById(id);

            if (promotion == null)
            {
                return NotFound();
            }

            return Ok(promotion);
        }

        [HttpGet]
        [Route("GetAvailablePromotions")]
        public IActionResult GetAvailablePromotions(int serviceId,int servicePackageId)
        {
            var promotion = _promotionQueries.GetAvailablePromotions(serviceId, servicePackageId);

            if (promotion == null)
            {
                return NotFound();
            }

            return Ok(promotion);
        }

        [HttpGet]
        [Route("GetAvailablePromotionByContractId")]
        public IActionResult GetAvailablePromotionByContractId(int outContractId)
        {
            var promotion = _promotionQueries.GetAvailablePromotionByContractId(outContractId);

            if (promotion == null)
            {
                return NotFound();
            }

            return Ok(promotion);
        } 
        
        [HttpGet]
        [Route("GetAvailablePromotionForContract")]
        public IActionResult GetAvailablePromotionForContract([FromQuery] AvailablePromotionModelFilter promotionFilter)
        {
           // AvailablePromotionModelFilter promotionFilter = new AvailablePromotionModelFilter();
            var promotion = _promotionQueries.GetAvailablePromotionForContract(promotionFilter);

            if (promotion == null)
            {
                return NotFound();
            }

            return Ok(promotion);
        }
        //public IActionResult GetAvailablePromotionForContract(int serviceId,int servicePackageId,int outContractServicePackageId,  string cityId, string districtId)
        //{
        //    var promotion = _promotionQueries.GetAvailablePromotionForContract(serviceId, servicePackageId, outContractServicePackageId,  cityId,  districtId);

        //    if (promotion == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(promotion);
        //}

        [HttpPut("{id}")]
        [Route("UpdatePromotion")]
        public async Task<IActionResult> UpdatePromotion(int id, [FromBody] CUPromotionCommand promotionCommand)
        {
            //if (id != promotionCommand.Id)
            //{
            //    return BadRequest();
            //}

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
        [HttpPut("{id}")]
        [Route("DeletePromotion")]
        public async Task<IActionResult> DeletePromotion(int id, [FromBody] DeletePromotionCommand promotionCommand)
        {
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