using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationUserIdentity.API.Infrastructure.Queries;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.NotificationModels;
using ApplicationUserIdentity.API.Services.GRPC.Clients;
using Global.Models;
using Global.Models.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ApplicationUserIdentity.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SendMessageController : CustomBaseController
    {
        private readonly IContractorGrpcService _contractorGrpcService;
        private readonly IUserQueries _accountQueries;
        private readonly IFCMTokenQueries _fcmTokenQueries;
        private readonly INotificationGrpcService _notificationGrpcService;
        public SendMessageController(
            IUserQueries accountQueries,
            IFCMTokenQueries fcmTokenQueries,
               INotificationGrpcService notificationGrpcService,
            IContractorGrpcService contractorGrpcService)
        {
            _contractorGrpcService = contractorGrpcService;
            _accountQueries = accountQueries;
            _fcmTokenQueries = fcmTokenQueries;
            _notificationGrpcService = notificationGrpcService;
        }


        [HttpPost]
        [Route("SendMessageFilter")]
        public async Task<IActionResult> SendMessageFilter(SendNotificationFilterRequest request)
        {
            if (!request.IsSendEmail && !request.IsSendNotification && !request.IsSendSMS) 
                return BadRequest();
            
            if (request.FilterSelect == null) 
                return BadRequest();
            
            if (string.IsNullOrEmpty(request.Message))
                return BadRequest(new ErrorGeneric("Nội dung là bắt buộc", "Message"));

            var filterModel = request.FilterSelect;
            var users = await NotificationHelper.GetListApplicationUser(filterModel, _contractorGrpcService, _accountQueries);

            if (users == null || users.Subset == null || users.Subset.Count == 0) return NotFound();
            if (request.UnSelectIds != null)
            {
                foreach (int unId in request.UnSelectIds)
                {
                    users.Subset = users.Subset.Where(u => u.Id != unId).ToList();
                }
            }
            if (users.Subset.Count == 0) return NotFound();

            if (request.IsSendNotification)
            {
                var uids = string.Join(",", users.Subset.Select(u => u.IdentityGuid).ToArray());
                var notification = new PushNotificationRequest()
                {
                    Zone = NotificationZone.ApplicationUser,
                    Type = NotificationType.App,
                    Category = NotificationCategory.None,
                    Title = request.Title,
                    Content = request.Message,
                    Payload = JsonConvert.SerializeObject(new { Category = NotificationCategory.None }, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    })
                };
                await _notificationGrpcService.PushNotificationByUids(notification, uids);
                
            }

            if (request.IsSendEmail)
            {
                var emails = string.Join(",",users.Subset.Where(u => !string.IsNullOrEmpty(u.Email)).Select(u => u.Email).ToList());
                await _notificationGrpcService.SendEmail(new SendMailRequest()
                {
                    Body = request.Message,
                    Emails = emails,
                    Subject = request.Title
                });
            }

            if (request.IsSendSMS)
            {
                var phones = string.Join(",", users.Subset.Where(u => !string.IsNullOrEmpty(u.MobilePhoneNo)).Select(u => u.MobilePhoneNo).ToList());
                await _notificationGrpcService.SendSms(new SendSMSRequest()
                {
                    Message = request.Message,
                    PhoneNumbers = phones
                });
            }

            return Ok("Thành công");

        }
    }
}
