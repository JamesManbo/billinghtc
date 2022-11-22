using FirebaseAdmin.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.API.Services
{
    public interface IFcmService
    {
        Task<bool> SubscribeToTopic(string topic, List<string> registrationTokens);
        Task<bool> UnSubscribeToTopic(string topic, List<string> registrationTokens);
    }
    public class FcmService : IFcmService
    {
        public async Task<bool> SubscribeToTopic(string topic, List<string> registrationTokens)
        {
            if (string.IsNullOrEmpty(topic) || registrationTokens == null|| registrationTokens.Count==0) return false;

            var response = await FirebaseMessaging.DefaultInstance.SubscribeToTopicAsync(registrationTokens, topic);
            if (response!=null && response.FailureCount == 0) return true;
            return false;
        }

        public async Task<bool> UnSubscribeToTopic(string topic, List<string> registrationTokens)
        {
            if (string.IsNullOrEmpty(topic) || registrationTokens == null || registrationTokens.Count == 0) return false;

            var response = await FirebaseMessaging.DefaultInstance.UnsubscribeFromTopicAsync(registrationTokens, topic);
            if (response != null && response.FailureCount == 0) return true;
            return false;
        }
    }
}
