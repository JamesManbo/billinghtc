using CMS.APIGateway.Services.FeedbackAndRequest;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AutoMapper;
using CMS.APIGateway.Infrastructure.Helpers;
using CMS.APIGateway.Infrastructure.Validation;
using CMS.APIGateway.Models;
using CMS.APIGateway.Models.FeedbackAndRequest;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CMS.APIGateway.Controllers
{
    [Route("api/integration/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly ILogger<FeedbacksController> _logger;
        private IFeedbackAndRequestService _feedbackAndRequest;
        private IMapper _mappep;
        private readonly AppSettings _appSettings;
        public FeedbacksController(IFeedbackAndRequestService feedbackAndRequest,
            IMapper mapper,
            IOptions<AppSettings> appSettingOption, ILogger<FeedbacksController> logger)
        {
            _feedbackAndRequest = feedbackAndRequest;
            _mappep = mapper;
            _appSettings = appSettingOption.Value;
            this._logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> PostApplicationUser(CUFeedbackAndRequest feedback)
        {
            var headers = Request.Headers;
            if (!headers.TryGetValue(_appSettings.TokenHeader, out var tokenKey))
            {
                return Unauthorized();
            }

            if (!ValidateTokenKey(tokenKey, feedback.RequestCode))
            {
                return Unauthorized();
            }            

            var feedbackValidator = new FeedbackValidator();
            var validationResult = feedbackValidator.Validate(feedback);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            feedback.GlobalId = feedback.Guid;
            var actResponse = await _feedbackAndRequest.CreateFeedbackOrRequest(feedback);
            if (!actResponse)
            {
                return BadRequest("Đã có lỗi xảy ra, vui lòng thử lại");
            }

            return Ok("Thêm mới báo cáo sự cố thành công");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFeedbackAndRequest(CUFeedbackAndRequest cUFeedback)
        {
            _logger.LogInformation("UPDATE Feedback request body: {0}", JsonConvert.SerializeObject(cUFeedback));
            var headers = Request.Headers;
            if (!headers.TryGetValue(_appSettings.TokenHeader, out var tokenKey))
            {
                return Unauthorized();
            }

            if (!ValidateTokenKey(tokenKey, cUFeedback.RequestCode))
            {
                return Unauthorized();
            }

            cUFeedback.GlobalId = cUFeedback.Guid;
            cUFeedback.UpdateFrom = "HTC_ITC_Ticket_System";
            var actResponse = await _feedbackAndRequest.UpdateFeedbackOrRequest(cUFeedback);
            if (!actResponse)
            {
                _logger.LogError("UPDATE Feedback has an exception");
                return BadRequest("Đã có lỗi xảy ra, vui lòng thử lại sau");
            }

            return Ok("Cập nhật báo cáo sự cố thành công");
        }

        private bool ValidateTokenKey(string tokenKey, string requestCode)
        {
            if (string.IsNullOrWhiteSpace(tokenKey) || string.IsNullOrWhiteSpace(requestCode)) return false;

            return requestCode.EncryptSHA256(_appSettings.MD5TokenCryptoKey).Equals(tokenKey);
        }
    }
}
