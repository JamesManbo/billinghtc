using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Notification.API.Grpc.Client;
using Notification.API.Models;
using Notification.API.Protos;
using Notification.API.Repositories;
using Notification.API.Services;

namespace Notification.API.Grpc
{
    public class NotificationGrpcService : NotificationGrpc.NotificationGrpcBase
    {
        private readonly ILogger<NotificationGrpcService> _logger;
        private readonly INotificationRepository _notificationRepository;
        private readonly IPushNotification _pushNotification;
        private readonly IUserGrpcService _userGrpcService;
        private readonly IApplicationUserGrpcService _applicationUserGrpcService;
        private readonly ISendSMS _sendSMS;
        private readonly ISendMail _sendMail;
        private readonly IMapper _mapper;

        public NotificationGrpcService(INotificationRepository notificationRepository,
            IPushNotification pushNotification,
            IUserGrpcService userGrpcService,
            IApplicationUserGrpcService applicationUserGrpcService,
            ISendMail sendMail,
            ISendSMS sendSMS,
            IMapper mapper,
            ILogger<NotificationGrpcService> logger)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _pushNotification = pushNotification;
            _sendSMS = sendSMS;
            _sendMail = sendMail;
            _userGrpcService = userGrpcService;
            _applicationUserGrpcService = applicationUserGrpcService;
            this._logger = logger;
        }

        public override async Task<NotificationPageListGrpcDTO> GetListByReceiver(NotificationFilterModelGrpc request, ServerCallContext context)
        {
            var result = await _notificationRepository.GetPageListByReceiver(_mapper.Map<NotificationFilterModel>(request));
            var rs = _mapper.Map<NotificationPageListGrpcDTO>(result);
            return await Task.FromResult(rs);
        }

        public override async Task<NotificationDTOGrpc> GetById(StringValue request, ServerCallContext context)
        {
            var result = await _notificationRepository.GetById(request.Value);
            var rs = _mapper.Map<NotificationDTOGrpc>(result);
            return await Task.FromResult(rs);
        }

        public override async Task<NotificationDTOGrpc> Update(UpdateNotificationRequestGrpc request, ServerCallContext context)
        {
            var result = await _notificationRepository.Update(request.Id, _mapper.Map<Models.Notification>(request.NotificationModel));
            var rs = _mapper.Map<NotificationDTOGrpc>(result);
            return await Task.FromResult(rs);
        }

        public override async Task<NotificationDTOGrpc> UpdateViewedNotification(StringValue request, ServerCallContext context)
        {
            var exist = await _notificationRepository.GetById(request.Value);
            if (exist != null)
            {
                if (exist.IsRead) return await Task.FromResult(_mapper.Map<NotificationDTOGrpc>(exist));
                exist.IsRead = true;

                var result = await _notificationRepository.Update(request.Value, exist);
                var rs = _mapper.Map<NotificationDTOGrpc>(result);
                return await Task.FromResult(rs);
            }

            return null;
        }

        public override async Task<PushNotificationResponseGrpc> PushNotificationByDepartment(PushNotificationRequestGrpc request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.DepartmentCode)) return new PushNotificationResponseGrpc { Success = false };

            var notification = _mapper.Map<Models.Notification>(request);
            notification.CreatedDate = DateTime.UtcNow;

            //var userToKens = await _userGrpcService.GetListTokenByDepartment(request.DepartmentCode);

            var listUser = await _userGrpcService.GetAllUserByDepartmentCode(request.DepartmentCode);
            if (listUser != null && listUser.Count > 0)
            {
                var lstNotification = new List<Models.Notification>();
                foreach (UserModel user in listUser)
                {
                    var addObject = (Models.Notification)notification.Clone();
                    addObject.ReceiverId = user.IdentityGuid;
                    addObject.Receiver = user.FullName;
                    lstNotification.Add(addObject);

                }
                await _notificationRepository.BulkInsert(lstNotification);
            }


            return new PushNotificationResponseGrpc()
            {
                Success = PushNotificationTopic(notification, request.DepartmentCode)
            };
        }

        public override async Task<PushNotificationResponseGrpc> PushNotificationByRole(PushNotificationRequestGrpc request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.RoleCode)) return new PushNotificationResponseGrpc { Success = false };

            var notification = _mapper.Map<Models.Notification>(request);
            notification.CreatedDate = DateTime.UtcNow;

            // var userToKens = await _userGrpcService.GetListTokenByRoleUser(request.RoleCode);
            var listUser = await _userGrpcService.GetAllUserByRoleCode(request.RoleCode);
            if (listUser != null && listUser.Count > 0)
            {
                var lstNotification = new List<Models.Notification>();
                foreach (UserModel user in listUser)
                {
                    var addObject = (Models.Notification)notification.Clone();
                    addObject.ReceiverId = user.IdentityGuid;
                    addObject.Receiver = user.FullName;
                    lstNotification.Add(addObject);
                }
                await _notificationRepository.BulkInsert(lstNotification);
            }


            return new PushNotificationResponseGrpc()
            {
                Success = PushNotificationTopic(notification, request.RoleCode)
            };
        }

        private bool PushNotificationTopic(Models.Notification notificationTemplate, string topics)
        {
            var handlingResult = false;
            if (!string.IsNullOrEmpty(topics))
            {
                var lstTopic = topics.Split(",");
                foreach (string topic in lstTopic)
                {
                    if(!string.IsNullOrEmpty(topic))
                        _pushNotification.Push(notificationTemplate, topic);
                }

                handlingResult = true;
            }

            return handlingResult;

        }

        public override async Task<PushNotificationResponseGrpc> PushNotificationByUids(PushNotificationRequestGrpc request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.Uids)) return new PushNotificationResponseGrpc { Success = false };

            var notification = _mapper.Map<Models.Notification>(request);
            notification.CreatedDate = DateTime.UtcNow;

            var userToKens = await _userGrpcService.GetFCMTokensByUids(request.Uids);

            return new PushNotificationResponseGrpc()
            {
                Success = await PushNotificationHandler(userToKens, notification)
            };
        }
        public override async Task<PushNotificationResponseGrpc> PushNotificationByCustomerUids(PushNotificationRequestGrpc request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.Uids)) return new PushNotificationResponseGrpc { Success = false };

            var notification = _mapper.Map<Models.Notification>(request);
            notification.CreatedDate = DateTime.UtcNow;

            var userToKens = await _applicationUserGrpcService.GetFCMTokensByUids(request.Uids);

            return new PushNotificationResponseGrpc()
            {
                Success = await PushNotificationHandler(userToKens, notification)
            };
        }

        private async Task<bool> PushNotificationHandler(List<UserTokenModel> userTokens, Models.Notification notificationTemplate)
        {
            var handlingResult = false;
            var distinctUsers = userTokens.Select(u => new
            {
                u.ReceiverId,
                u.Receiver
            }).ToHashSet();

            if (userTokens != null && userTokens.Count > 0)
            {
                // Push notification via FCM handler
                var chunkedTokens = userTokens.ChunkBy(10);
                try
                {
                    Parallel.ForEach(chunkedTokens,
                    async chunk =>
                    {
                        for (int i = 0; i < chunk.Count(); i++)
                        {
                            var pushObject = (Models.Notification)notificationTemplate.Clone();
                            pushObject.ReceiverId = userTokens.ElementAt(i).ReceiverId;
                            pushObject.Receiver = userTokens.ElementAt(i).Receiver;
                            pushObject.ReceiverToken = userTokens.ElementAt(i).Token;
                            pushObject.Platform = userTokens.ElementAt(i).Platform;
                            await _pushNotification.Push(pushObject);
                        }
                    });
                }
                catch (Exception e)
                {
                    this._logger.LogError(e.Message);
                }

                var lstNotification = new List<Models.Notification>();
                // Insert new notification to system's notification
                for (int i = 0; i < distinctUsers.Count; i++)
                {
                    var saveObject = (Models.Notification)notificationTemplate.Clone();
                    saveObject.ReceiverId = distinctUsers.ElementAt(i).ReceiverId;
                    saveObject.Receiver = distinctUsers.ElementAt(i).Receiver;
                    lstNotification.Add(saveObject);
                }
                await _notificationRepository.BulkInsert(lstNotification);
            }

            return handlingResult;
        }

        public override async Task<SendSmsResponseGrpc> SendSms(SendSmsRequestGrpc request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.PhoneNumbers)) return new SendSmsResponseGrpc { Success = false };
            var rs = true;

            var listPhoneNumber = request.PhoneNumbers.Split(",");
            foreach (string number in listPhoneNumber)
            {
                var smsModel = new SendSMSRequest()
                {
                    Message = request.Message,
                    PhoneNumber = number
                };
                var smsRs = await _sendSMS.SendSms(smsModel);
                if (!smsRs) rs = false;
            }

            return new SendSmsResponseGrpc { Success = rs };
        }

        public override async Task<SendMailResponseGrpc> SendMail(SendMailRequestGrpc request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.Emails)) return new SendMailResponseGrpc { Success = false };
            var rs = true;

            var listEmail = request.Emails.Split(",");
            foreach (string email in listEmail)
            {
                var emailModel = new SendMailRequest()
                {
                    Email = email,
                    Body = request.Body,
                    Subject = request.Subject
                };
                var emailRs = await _sendMail.SendEmail(emailModel);
                if (!emailRs) rs = false;
            }

            return new SendMailResponseGrpc { Success = rs };
        }
    }

}