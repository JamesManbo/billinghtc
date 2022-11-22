using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Configs.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrganizationUnit.Domain.AggregateModels.FCMAggregate;
using OrganizationUnit.Domain.Commands.FCMToken;
using OrganizationUnit.Infrastructure.Queries;
using OrganizationUnit.Infrastructure.Repositories.FCMRepository;

namespace OrganizationUnit.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = AuthenticationSchemes.CmsApiIdentityKey)]
    [Route("[controller]")]
    public class FCMTokensController : CustomBaseController
    {
        private readonly ILogger<FCMTokensController> _logger;
        private readonly IFCMTokenQueries _fcmTokenQueries;
        private readonly IFCMTokenRepository _fcmTokenRepository;

        public FCMTokensController(
            ILogger<FCMTokensController> logger, 
            IFCMTokenQueries fcmTokenQueries, 
            IFCMTokenRepository fcmTokenRepository)
        {
            _logger = logger;
            _fcmTokenQueries = fcmTokenQueries;
            _fcmTokenRepository = fcmTokenRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFCMTokenCommand createFCMTokenCommand)
        {
            if (!string.IsNullOrEmpty(createFCMTokenCommand.Token))
            {
                var fcmToken = _fcmTokenQueries.FindByReceiverIdAndFcmToken(UserIdentity.UniversalId, createFCMTokenCommand.Token);
                if (fcmToken == null)
                {
                    await _fcmTokenRepository.CreateAndSave(new FCMToken()
                    {
                        ReceiverId = UserIdentity.UniversalId,
                        Token = createFCMTokenCommand.Token,
                        Platform = "Web"
                    });
                }

                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("UnRegisterFCM")]
        public async Task<IActionResult> UnRegisterFCM(CreateFCMTokenCommand request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest();
            }
            var existRecord = await _fcmTokenRepository.GetByToken(request.Token);

            if (existRecord != null)
            {
                await _fcmTokenRepository.RemoveAndSave(existRecord);
                return Ok("");
            }
            return BadRequest();
        }
    }
}
