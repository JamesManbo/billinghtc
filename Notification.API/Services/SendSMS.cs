using Microsoft.Extensions.Options;
using Notification.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Xml;

namespace Notification.API.Services
{
    public interface ISendSMS
    {
        Task<bool> SendSms(SendSMSRequest request);
    }
    public class SendSMS : ISendSMS
    {
        private readonly SMSOptions _smsOptions;
        public SendSMS(IOptions<SMSOptions> smsOptions)
        {
            _smsOptions = smsOptions.Value;
        }

        public async Task<bool> SendSms(SendSMSRequest request)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_smsOptions.Host);
                client.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                var rq = new HttpRequestMessage(HttpMethod.Post, _smsOptions.Path);
                rq.Version = HttpVersion.Version11;

                var keyValues = new List<KeyValuePair<string, string>>();
                keyValues.Add(new KeyValuePair<string, string>("dest", request.PhoneNumber));
                keyValues.Add(new KeyValuePair<string, string>("name", _smsOptions.Name));
                keyValues.Add(new KeyValuePair<string, string>("msgBody", request.Message));
                keyValues.Add(new KeyValuePair<string, string>("contentType", "text"));
                keyValues.Add(new KeyValuePair<string, string>("serviceID", _smsOptions.ServiceID));
                keyValues.Add(new KeyValuePair<string, string>("mtID", _smsOptions.MtID.ToString()));
                keyValues.Add(new KeyValuePair<string, string>("cpID", _smsOptions.CpID));
                keyValues.Add(new KeyValuePair<string, string>("username", _smsOptions.Username));
                keyValues.Add(new KeyValuePair<string, string>("password", _smsOptions.Password));

                rq.Content = new FormUrlEncodedContent(keyValues);
                var response = await client.SendAsync(rq);

                var responseString = await response.Content.ReadAsStringAsync();
                XmlDocument doc = new XmlDocument(); 
                doc.LoadXml(responseString);
                XmlNodeList elemList = doc.GetElementsByTagName("int");
                if (elemList != null && elemList.Count > 0)
                {
                    if (elemList[0].InnerText == "200")
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}
