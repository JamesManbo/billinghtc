using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Global.Models.Filter;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StaffApp.APIGateway.Models.NotificationModels;
using StaffApp.APIGateway.Models.RequestModels;
using StaffApp.APIGateway.Services;

namespace StaffApp.APIGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NotificationsController : CustomBaseController
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
    }
}