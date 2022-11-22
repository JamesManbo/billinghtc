using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerApp.APIGateway.Models.NotificationModels;
using CustomerApp.APIGateway.Models.RequestModels;
using CustomerApp.APIGateway.Services;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustomerApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationsController : CustomerBaseController
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        public NotificationsController(INotificationService notificationService, IMapper mapper)
        {
            _notificationService = notificationService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IActionResponse<IPagedList<NotificationDTO>>>> GetAsync(
            [FromQuery] NotificationFilterModel filterModel)
        {
            filterModel.ReceiverId = UserIdentity.UniversalId;
            var actResponse = await _notificationService.GetListByReceiver(filterModel);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }

        [HttpPut]
        [Route("UpdateViewed")]
        public async Task<ActionResult<IActionResponse<NotificationDTO>>> Put(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var actResponse = await _notificationService.UpdateViewed(id);
            if (actResponse == null)
            {
                return NotFound();
            }
            return Ok(actResponse);
        }
        //[HttpPost]
        //[Route("test")]
        //public async Task<ActionResult<IActionResponse<bool>>> test()
        //{
        //    await _notificationService.Test();
        //    return Ok("ok");
        //}
    }
}
