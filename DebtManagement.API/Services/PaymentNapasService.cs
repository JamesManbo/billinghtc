using DebtManagement.API.Infrastructure.Helpers;
using DebtManagement.Domain.Models.PaymentModels;
using HtmlAgilityPack;
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
    public interface IPaymentNapasService
    {
        Task<PaymentResultModel> GetPayment(string phone, int amount, string orderNumber);
    }
    public class PaymentNapasService : IPaymentNapasService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentNapasService> _logger;
        public PaymentNapasService(IConfiguration configuration, ILogger<PaymentNapasService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<PaymentResultModel> GetPayment(string phone, int amount, string orderNumber)
        {
            var result = new PaymentResultModel();

            try
            {
                var obj = new
                {
                    channel = "WEB",
                    orderId = orderNumber,
                    requestTime = DateTime.Now.ToString("ddMMyyyy HH:mm:ss:fff"),
                    mobileNumber = phone,
                    amount = amount,
                    mid = "942022",
                    languageId = "VN",
                    callBackUrl = _configuration.GetValue<string>("NapasURLCallbackPG"),
                    orderInfo = orderNumber,
                    ipAddress = "10.6.1.43",
                    checkSumHash = EncryptionHelper.Sha256(amount + "942022" + orderNumber),
                    napasCallBackURL = _configuration.GetValue<string>("NapasURLCallback"),
                    napasCancelURL = _configuration.GetValue<string>("NapasURLCallbackCancel") + "?orderNumber=" + orderNumber
                };

                result = await GetPayment(obj);

                var _jData = JObject.Parse(result.Result);

                result.StatusCode = _jData["resultCode"].ToString();

                result.IsSuccess = result.StatusCode == "0";

                result.Message = _jData["responseMsg"].ToString();

                using (var client = new HttpClient())
                {
                    using (var res = await client.GetAsync(_jData["redirectionURL"].ToString()))
                    {
                        var _strData = await res.Content.ReadAsStringAsync();

                        var docWeb = new HtmlDocument();
                        docWeb.LoadHtml(_strData);

                        var _link = docWeb.DocumentNode.SelectSingleNode("//form[@class='redirectForm']").Attributes["action"].Value + "?";

                        var query = string.Empty;

                        var _listInput = docWeb.DocumentNode.SelectNodes("//input");

                        _jData["napasData"] = JObject.FromObject(new { });

                        foreach (HtmlNode item in _listInput)
                        {
                            query += (string.IsNullOrEmpty(query) ? string.Empty : "&") + item.Attributes["name"].Value + "=" + item.Attributes["value"].Value;

                            if (item.Attributes["name"].Value == "vpc_MerchTxnRef")
                            {
                                result.TransactionId = item.Attributes["value"].Value;
                            }

                            _jData["napasData"][item.Attributes["name"].Value] = item.Attributes["value"].Value;
                        }

                        result.Data = _link + query;
                    }
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

        public async Task<PaymentResultModel> GetPayment(object data, int number = 1)
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

                    client.DefaultRequestHeaders.Add("Authorization", "Basic c2l4ZGVlfHNpeGRlZQ==");

                    var postContent = new StringContent(result.Post, Encoding.UTF8, "application/json");

                    using (var response = await client.PostAsync("http://10.6.2.11:19092/vnmbl/VNM/capturePayment", postContent))
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
                //vnmLogs.AddLogPayment(result);

                if (number <= 3)
                {
                    await Task.Delay(2000);

                    return await GetPayment(data, number++);
                }
            }

            return result;
        }
    }
}
