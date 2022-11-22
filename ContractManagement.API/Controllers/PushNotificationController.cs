using ContractManagement.API.Grpc.Clients;
using ContractManagement.Domain.Models.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContractManagement.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PushNotificationController : CustomBaseController
    {
        private readonly INotificationGrpcService _notificationGrpcService;

        public PushNotificationController(INotificationGrpcService notificationGrpcService)
        {
            this._notificationGrpcService = notificationGrpcService;
        }

        [HttpPost]
        public async Task<IActionResult> Push([FromBody] PushNotificationRequest request)
        {
            if (await this._notificationGrpcService.PushNotificationByUids(request))
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
