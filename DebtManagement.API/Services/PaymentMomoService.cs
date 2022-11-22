using DebtManagement.API.Infrastructure.Helpers;
using DebtManagement.Domain.Models.PaymentModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DebtManagement.API.Services
{
    public interface IPaymentMomoService
    {
        Task<PaymentResultModel> GetPayment(string phone, int amount, string orderNumber, string os);
    }
    public class PaymentMomoService : IPaymentMomoService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentMomoService> _logger;
        public PaymentMomoService(IConfiguration configuration, ILogger<PaymentMomoService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<PaymentResultModel> GetPayment(string phone, int amount, string orderNumber, string os)
        {
            var callback = string.Empty;
            if (os == "android")
            {
                callback = _configuration.GetValue<string>("MomoURLAndroidCallback");
            }
            else if (os == "ios")
            {
                callback = _configuration.GetValue<string>("MomoURLIosCallback");
            }
            else
            {
                callback = _configuration.GetValue<string>("MomoURLIosCallback");
                _logger.LogInformation("Error find Os");
            }
            var result = new PaymentResultModel();

            try
            {

                var dataPost = new Dictionary<string, object>()
                {
                    { "partnerCode", _configuration.GetValue<string>("MomoPartnerCode")},
                    { "accessKey", _configuration.GetValue<string>("MomoAccessKey") },
                    { "requestId", orderNumber },
                    { "amount", amount.ToString() },
                    { "orderId", orderNumber },
                    { "orderInfo", orderNumber },
                    { "returnUrl",callback },
                    { "notifyUrl", _configuration.GetValue<string>("MomoURLPush") },
                    { "extraData", string.Empty }
                };

                _logger.LogInformation(JsonConvert.SerializeObject(dataPost));

                var rawData = string.Empty;

                foreach (var item in dataPost)
                {
                    rawData += (string.IsNullOrEmpty(rawData) ? string.Empty : "&") + item.Key + "=" + item.Value.ToString();
                }

                var signature = EncryptionHelper.EncryptSHA256(rawData, _configuration.GetValue<string>("MomoSecretKey"));

                dataPost.Add("requestType", "captureMoMoWallet");
                dataPost.Add("signature", signature);

                _logger.LogInformation(JsonConvert.SerializeObject(dataPost));

                result = await GetPaymentFromMomo(dataPost);

                var _jData = JObject.Parse(result.Result);

                result.StatusCode = _jData["errorCode"].ToString();

                result.IsSuccess = result.StatusCode == "0";

                result.Message = _jData["localMessage"].ToString();

                if (result.IsSuccess)
                {
                    result.Data = _jData["payUrl"].ToString();

                    result.TransactionId = _jData["requestId"].ToString();
                }

                result.Result = JsonConvert.SerializeObject(_jData);
            }
            catch (Exception ex)
            {
                result.Excetion = JsonConvert.SerializeObject(ex);
            }
            _logger.LogInformation(JsonConvert.SerializeObject(result));
            return result;
        }

        public async Task<PaymentResultModel> GetPaymentFromMomo(object data, int number = 1)
        {
            var result = new PaymentResultModel()
            {
                //Type = REQUEST_TYPE_VNMAPI.JSON,
                DateStartRequest = DateTime.Now,
                IsRequestSucess = false,
                Number = number,
                Action = "CapturePayment"
            };

            try
            {
                result.Post = JsonConvert.SerializeObject(data);

                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Clear();

                    var postContent = new StringContent(result.Post, Encoding.UTF8, "application/json");

                    using (var response = await client.PostAsync("https://test-payment.momo.vn/gw_payment/transactionProcessor", postContent))
                    {
                        result.IsRequestSucess = response.IsSuccessStatusCode;

                        result.RequestStatusCode = (int)response.StatusCode;

                        result.Result = await response.Content.ReadAsStringAsync();
                    }
                }

                result.DateEndRequest = DateTime.Now;
            }
            catch (Exception ex)
            {
                result.DateEndRequest = DateTime.Now;
                result.Excetion = JsonConvert.SerializeObject(ex);
                
                if (number <= 3)
                {
                    await Task.Delay(2000);

                    return await GetPaymentFromMomo(data, number++);
                }
            }
            _logger.LogInformation("GetPaymentFromMomo: "+JsonConvert.SerializeObject(result));
            return result;
        }
    }
}
