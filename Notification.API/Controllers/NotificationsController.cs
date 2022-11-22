using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Global.Models;
using Global.Models.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Notification.API.Grpc.Client;
using Notification.API.Models;
using Notification.API.Repositories;
using Notification.API.Services;

namespace Notification.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : CustomBaseController
    {

        private readonly ILogger<NotificationsController> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly IPushNotification _pushNotification;
        private readonly ISendSMS _sendSMS;
        private readonly ISendMail _sendMail;
        private readonly IFcmService _fcmService;
        private readonly IUserGrpcService _userGrpcService;
        private readonly IApplicationUserGrpcService _applicationUserGrpcService;

        public NotificationsController(ILogger<NotificationsController> logger,
            INotificationRepository notificationRepository,
            IPushNotification pushNotification,
            IUserGrpcService userGrpcService,
            IApplicationUserGrpcService applicationUserGrpcService,
            ISendMail sendMail,
            IFcmService fcmService,
            ISendSMS sendSMS)
        {
            _logger = logger;
            _notificationRepository = notificationRepository;
            _pushNotification = pushNotification;
            _userGrpcService = userGrpcService;
            _sendSMS = sendSMS;
            _sendMail = sendMail;
            _fcmService = fcmService;
            _applicationUserGrpcService = applicationUserGrpcService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _notificationRepository.CountUnread(UserIdentity.UniversalId));
        }

        [HttpGet]
        [Route("GetNotifications")]
        public async Task<IActionResult> GetNotifications(int? take = 10,
            int? skip = 0)
        {
            return Ok(await _notificationRepository.GetByReceiver(UserIdentity.UniversalId, take, skip));
        }

        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetByReceiver(string id)
        {
            return Ok(await _notificationRepository.GetById(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post(Models.Notification notification)
        {
            notification.CreatedDate = DateTime.UtcNow;
            return Ok(await _notificationRepository.Add(notification));
        }

        [HttpPut("UpdateReadNotifications")]
        public async Task<IActionResult> Put([FromBody] string[] ids)
        {
            return Ok(await _notificationRepository.UpdateByIds(ids));
        }

        [HttpPost]
        [Route("SendSMS")]
        public async Task<IActionResult> SendSMS(SendSMSRequest request)
        {
            var rs = await _sendSMS.SendSms(request);
            if (rs)
                return Ok("ok");
            return BadRequest();
        }

        [HttpPost]
        [Route("SendEmail")]
        public async Task<IActionResult> SendEmail(SendMailRequest request)
        {
            var rs = await _sendMail.SendEmail(request);
            if (rs)
                return Ok("ok");
            return BadRequest();
        }
        
        [HttpPost]
        [Route("TestNoti")]
        [AllowAnonymous]
        public async Task<IActionResult> TestNoti(Models.Notification request)
        {
            var rs = await _pushNotification.Push(request, "");
            if (rs)
                return Ok("ok");
            return BadRequest();
        }
        
        [HttpPost]
        [Route("SubscribeToTopic")]
        public async Task<IActionResult> SubscribeToTopic(SubscribeTopicRequest request)
        {
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.Topics))
             return BadRequest();
            var lstToken = new List<string>();
            lstToken.Add(request.Token);

            var lstTopic = request.Topics.Split(",");
            foreach (string topic in lstTopic)
            {
                if (!string.IsNullOrEmpty(topic))
                {
                    await _fcmService.SubscribeToTopic(topic, lstToken);
                }
                
            }

            return Ok("ok");
        }

        [HttpPost]
        [Route("UnSubscribeToTopic")]
        [AllowAnonymous]
        public async Task<IActionResult> UnSubscribeToTopic(SubscribeTopicRequest request)
        {
            if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.Topics))
                return BadRequest();
            var lstToken = new List<string>();
            lstToken.Add(request.Token);

            var lstTopic = request.Topics.Split(",");
            foreach(string topic in lstTopic)
            {
                if (!string.IsNullOrEmpty(topic))
                {
                    await _fcmService.UnSubscribeToTopic(topic, lstToken);
                }
            }

            return Ok("ok");

        }
    }
}
