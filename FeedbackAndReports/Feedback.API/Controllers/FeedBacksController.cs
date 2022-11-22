using System;
using System.Threading.Tasks;
using Feedback.API.Models;
using Feedback.API.Queries;
using Feedback.API.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Feedback.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FeedbacksController : CustomBaseController
    {
        private readonly IFeedbackAndRequestQueries _feedbackQueries;
        private readonly IFeedbackAndRequestRepository _feedbackAndRequestRepository;
        public FeedbacksController(IFeedbackAndRequestQueries feedbackService, 
            IFeedbackAndRequestRepository feedbackAndRequestRepository)
        {
            _feedbackQueries = feedbackService;
            this._feedbackAndRequestRepository = feedbackAndRequestRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] FeedbackAndRequestFilterModel filterModel)
        {
            return Ok(await _feedbackQueries.GetList(filterModel));
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            return Ok(await _feedbackQueries.Get(id));
        }

        [HttpGet("GetAllUnhandledByCIds/{ids}")]
        public IActionResult GetByCId(string ids)
        {
            return Ok(_feedbackQueries.GetUnresolvedFeedbacksByCIds(ids));
        }
        
        [HttpGet("GetAllUnhandledByCIdsNotReductionYet/{cIds}")]
        public IActionResult GetAllUnhandledByCIdsNotReductionYet(string cIds, int includeReceiptLineId)
        {
            return Ok(_feedbackQueries.GetAllUnhandledByCIdsNotReductionYet(cIds, includeReceiptLineId));
        }

        [HttpGet("GetByIds/{ids}")]
        public IActionResult GetByIds(string ids)
        {
            return Ok(_feedbackQueries.GetByIds(ids));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFeedbackCommand createFeedbackCommand)
        {
            var feedback = new FeedbackAndRequest()
            {
                CustomerCode = createFeedbackCommand.CustomerCode,
                CustomerName = createFeedbackCommand.CustomerName,
                CustomerPhone = createFeedbackCommand.PhoneNumber,
                DateCreated = DateTime.Now,
                Description = createFeedbackCommand.Description,
                Content = createFeedbackCommand.Content,
                ChargeReduction = createFeedbackCommand.ChargeReduction
            };
            feedback.CreatedBy = UserIdentity.UniversalId;

            var actResponse = await _feedbackAndRequestRepository.Create(feedback);
            return Ok(actResponse);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update([FromBody] UpdateFeedbackCommand updateFeedbackCommand)
        {
            var oldFeedback = _feedbackQueries.Get(updateFeedbackCommand.Id);

            if (oldFeedback == null)
            {
                return NotFound();
            }

            var feedback = new FeedbackAndRequest()
            {
                Id = updateFeedbackCommand.Id,
                ContractId = updateFeedbackCommand.ContractId,
                CustomerName = updateFeedbackCommand.CustomerName,
                DateCreated = DateTime.Now,
                Description = updateFeedbackCommand.Description,
                Content = updateFeedbackCommand.Content,
                CustomerPhone = updateFeedbackCommand.PhoneNumber,
                ChargeReduction = updateFeedbackCommand.ChargeReduction
            };
            var response = await _feedbackAndRequestRepository.Update(feedback);

            return Ok(response);

        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var feedback = _feedbackQueries.Get(id);

            if (feedback == null)
            {
                return NotFound();
            }

            try
            {
                await _feedbackAndRequestRepository.Remove(id);
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }
    }
}