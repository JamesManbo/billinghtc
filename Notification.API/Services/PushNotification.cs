using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Notification.API.Models;
using Notification.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Notification.API.Services
{
    public interface IPushNotification
    {
        Task<bool> Push(Models.Notification notificationModel, string topic = "");
        Task<bool> PushNotificationTest(Models.Notification notificationModel);
    }
    public class PushNotification : IPushNotification
    {
        private readonly ILogger<PushNotification> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly INotificationRepository _notificationRepository;
        private readonly ITopicRepository _topicRepository;

        public PushNotification(
            INotificationRepository notificationRepository,
            ITopicRepository topicRepository,
            IHttpClientFactory httpClientFactory,
            ILogger<PushNotification> logger)
        {
            this._notificationRepository = notificationRepository;
            this._topicRepository = topicRepository;
            this._httpClientFactory = httpClientFactory;
            this._logger = logger;
        }
        public async Task<bool> Push(Models.Notification notificationModel, string topic="")
        {
            if (string.IsNullOrEmpty(notificationModel.Content)) return false;

            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var fcmRequestBody = new
            {
                notification = new
                {
                    title = notificationModel.Title,
                    body = notificationModel.Content,
                    //click_action = notificationModel.Platform.Equals("web", StringComparison.OrdinalIgnoreCase)?"/transaction-management":""
                },

                to = string.IsNullOrEmpty(topic)?notificationModel.ReceiverToken: $"/topics/{topic}",
                priority = "high",
                restricted_package_name = ""
            };

            JObject body = JObject.FromObject(fcmRequestBody);
            //body.Add("content-available", true);
            JObject dataWithMessage = new JObject();

            if (!string.IsNullOrWhiteSpace(notificationModel.Payload)
                && !string.IsNullOrEmpty(notificationModel.Platform))
            {
                if (notificationModel.Platform.Equals("web", StringComparison.OrdinalIgnoreCase))
                {
                    dataWithMessage = JObject.Parse(notificationModel.Payload);
                    if (dataWithMessage.GetValue("id").Type == JTokenType.Null)
                    {
                        dataWithMessage["id"] = notificationModel.Id;
                    }
                }
                else
                {
                    dataWithMessage.Add("click_action", "FLUTTER_NOTIFICATION_CLICK");
                    dataWithMessage.Add("title", notificationModel.Title);
                    dataWithMessage.Add("message", notificationModel.Content);
                    dataWithMessage.Add("payload", JObject.Parse(notificationModel.Payload));
                }
            }

            body.Add("data", dataWithMessage);

            if (!string.IsNullOrEmpty(topic))
            {
                var topicExist = await _topicRepository.GetByName(topic);
                if (topicExist == null)
                {
                    await _topicRepository.Add(new Topic { Name = topic });
                }
            }

            return await this.SendNotify(body);
        }


        public async Task<bool> PushNotificationTest(Models.Notification notificationModel)
        {
            if (string.IsNullOrEmpty(notificationModel.Content)) return false;

            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            if (notificationModel != null)
            {
                notificationModel.CreatedDate = DateTime.UtcNow;
                await _notificationRepository.Add(notificationModel);
            }

            var temp = new
            {
                notification = new
                {
                    title = notificationModel.Title,
                    body = notificationModel.Content,
                    click_action = "dashboard"
                },
                to = notificationModel.ReceiverToken,
                priority = "high",
                restricted_package_name = ""
            };

            return await this.SendNotify(temp);
        }

        private async Task<bool> SendNotify(object contentBody)
        {
            var httpClient = this._httpClientFactory.CreateClient("fcmapi");
            JObject body;
            if (!(contentBody is JObject))
            {
                body = JObject.FromObject(contentBody);
            }
            else
            {
                body = (JObject)contentBody;
            }
            var content = new StringContent(body.ToString(), Encoding.UTF8, "application/json");

            using var httpResponse =
                await httpClient.PostAsync(@"/fcm/send", content);

            //_logger.LogInformation("FCM Send Notificaiton Response Status: {0}. Reponse: {1}", httpResponse.StatusCode, httpResponse.Content);

            httpResponse.EnsureSuccessStatusCode();

            if (httpResponse.Content != null)
            {
                var responseString = await httpResponse.Content.ReadAsStringAsync();
                //_logger.LogInformation("FCM Send Notificaiton Response Body: {0}", responseString);
                var rs = JsonConvert.DeserializeObject<FCMResponse>(responseString);
                if (rs.success == 1 || !string.IsNullOrEmpty(rs.message_id))
                    return true;
            }

            return false;
        }
    }
}
