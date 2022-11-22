using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeedbackAndReports.API.Model;
using FeedbackAndReports.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FeedbackAndReports.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackController(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        [HttpPost]
        public   ActionResult CreateNewFeedback(FeedbackModel feedbackModel)
        {
            _feedbackRepository.Add(feedbackModel);
            return Ok();
        }
    }   
}