using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DebtManagement.API.Infrastructure.Helpers;
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Models.PaymentModels;
using DebtManagement.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DebtManagement.API.Controllers
{
    public class CallbackController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CallbackController> _logger;
        private readonly IReceiptVoucherRepository _receiptVoucherRepository;
        public CallbackController(IConfiguration configuration,
            IReceiptVoucherRepository receiptVoucherRepository,
            ILogger<CallbackController> logger)
        {
            _configuration = configuration;
            _receiptVoucherRepository = receiptVoucherRepository;
            _logger = logger;
        }
        public async Task<ActionResult> MomoAndroid()
        {
            //foreach (var item in Request.Headers.AllKeys)
            //{
            //    logger.Info("HEADER|" + item + "|" + Request.Headers[item]);
            //}

            var _dic = new Dictionary<string, string>();

            foreach (var item in Request.Query.Keys)
            {
                _dic.Add(item, Request.Query[item]);
            }
            _logger.LogInformation("Momo callback: " + JsonConvert.SerializeObject(_dic));

            await Task.Delay(5000);

            return View();
        }

        public async Task<ActionResult> MomoIos()
        {
            //foreach (var item in Request.Headers.AllKeys)
            //{
            //    logger.Info("HEADER|" + item + "|" + Request.Headers[item]);
            //}

            var _dic = new Dictionary<string, string>();

            foreach (var item in Request.Query.Keys)
            {
                _dic.Add(item, Request.Query[item]);
            }

            _logger.LogInformation("Momo callback: " + JsonConvert.SerializeObject(_dic));

            await Task.Delay(5000);

            return View();
        }

        public async Task<ActionResult> MomoPush()
        {
            var _dic = new Dictionary<string, string>();

            //foreach (var item in Request.Headers.AllKeys)
            //{
            //    logger.Info("HEADER|" + item + "|" + Request.Headers[item]);
            //}

            foreach (var item in Request.Form.Keys)
            {
                Request.Form.TryGetValue(item, out var valu);
                _dic.Add(item, string.Join(",", valu));
            }

            _logger.LogInformation("MomoPush: " + JsonConvert.SerializeObject(_dic));

            var _napasHash = _dic["signature"];

            var _hash = "partnerCode=" + _dic["partnerCode"] +
                "&accessKey=" + _dic["accessKey"] +
                "&requestId=" + _dic["requestId"] +
                "&amount=" + _dic["amount"] +
                "&orderId=" + _dic["orderId"] +
                "&orderInfo=" + _dic["orderInfo"] +
                "&orderType=" + _dic["orderType"] +
                "&transId=" + _dic["transId"] +
                "&message=" + _dic["message"] +
                "&localMessage=" + _dic["localMessage"] +
                "&responseTime=" + _dic["responseTime"] +
                "&errorCode=" + _dic["errorCode"] +
                "&payType=" + _dic["payType"] +
                "&extraData=" + _dic["extraData"];

            _hash = EncryptionHelper.EncryptSHA256(_hash, _configuration.GetValue<string>("MomoSecretKey"));

            if (_hash.ToUpper() != _napasHash.ToUpper())
            {

                return Content("Sai thông tin dữ liệu: " + JsonConvert.SerializeObject(_dic));
            }

            var jData = await UpdatePayment(_dic);

            var responseTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var signatureResponse = "partnerCode" + _dic["partnerCode"] +
                "&accessKey=" + _dic["accessKey"] +
                "&requestId=" + _dic["requestId"] +
                "&orderId=" + _dic["orderId"] +
                "&errorCode=0" +
                "&message=Nhận thông tin thành công" +
                "&responseTime=" + responseTime +
                "&extraData=";

            signatureResponse = EncryptionHelper.EncryptSHA256(signatureResponse, _configuration.GetValue<string>("MomoSecretKey"));

            var dataResonse = new
            {
                partnerCode = _dic["partnerCode"],
                accessKey = _dic["accessKey"],
                requestId = _dic["requestId"],
                orderId = _dic["orderId"],
                errorCode = 0,
                message = "Nhận thông tin thành công",
                responseTime = responseTime,
                extraData = string.Empty,
                signature = signatureResponse
            };

            return Json(dataResonse, new JsonSerializerSettings());
        }


        public async Task<ActionResult> NapasCancel()
        {
            var _dic = new Dictionary<string, string>();

            foreach (var item in Request.Form.Keys)
            {
                Request.Form.TryGetValue(item, out var valu);
                _dic.Add(item, string.Join(",", valu));
            }

            _logger.LogInformation("NapasCancel: " + JsonConvert.SerializeObject(_dic));

            var jData = await UpdatePayment(_dic);

            return View();
        }

        public async Task<ActionResult> Napas()
        {
            //foreach (var item in Request.Headers.AllKeys)
            //{
            //    logger.Info("HEADER|" + item + "|" + Request.Headers[item]);
            //}

            var _dic = new Dictionary<string, string>();

            var _str = "AF0EAFF75EA44DB0AAE472BE55177CC9";

            var _napasHash = string.Empty;

            var _responseCode = string.Empty;

            foreach (var item in Request.Query.Keys)
            {
                _dic.Add(item, Request.Query[item]);
                if (item != "vpc_SecureHash")
                {
                    _str += Request.Query[item];
                }
                else
                {
                    _napasHash = Request.Query[item];
                }

                if (item == "vpc_ResponseCode")
                {
                    _responseCode = Request.Query[item];
                }
            }

            var _hash = MD5Helper.GetMd5Hash(_str);

            //logger.Info("Napas|HASH|" + _hash);
            _logger.LogInformation("Napas: " + JsonConvert.SerializeObject(_dic));

            if (_hash.ToUpper() != _napasHash.ToUpper())
            {
                return View();
            }

            if (_responseCode == "0")
            {
                await Task.Delay(5000);

                return View();
            }

            var jData = await UpdatePayment(_dic);

            return View();
        }

        public ActionResult PG()
        {
            var _dic = new Dictionary<string, string>();

            foreach (var item in Request.Query.Keys)
            {
                _dic.Add(item, Request.Query[item]);
            }

            _logger.LogInformation("PG: " + JsonConvert.SerializeObject(_dic));

            return View();
        }


        public async Task<object> UpdatePayment(Dictionary<string, string> data)
        {

            //using (var client = new HttpClient())
            //{
            //    var _content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            //    using (var res = client.PostAsync(_configuration.GetValue<string>("APIUpdatePayment"), _content))
            //    {
            //        var strResult = await res.Result.Content.ReadAsStringAsync();

            //        return JObject.Parse(strResult);
            //    }
            //}

            if (data["partnerCode"] != null)
            {
                return await UpdatePaymentMomo(data);
            }
            return await UpdatePaymentNapas(data);

        }

        public async Task<object> UpdatePaymentNapas(Dictionary<string, string> data)
        {
            var logPayment = new PaymentResultModel()
            {
                //Type = StaticEnumApi.REQUEST_TYPE_VNMAPI.JSON,
                DateStartRequest = DateTime.Now,
                IsRequestSucess = true,
                Number = 1,
                Action = "UpdatePayment",
                Post = JsonConvert.SerializeObject(data),
                IsSuccess = false
            };

            var _str = "AF0EAFF75EA44DB0AAE472BE55177CC9";

            var _napasHash = string.Empty;

            foreach (KeyValuePair<string, string> x in data)
            {
                if (x.Key != "vpc_SecureHash")
                {
                    _str += x.Value.ToString();
                }
                else
                {
                    _napasHash = x.Value.ToString();
                }
            }

            var _hash = MD5Helper.GetMd5Hash(_str);

            if (_hash.ToUpper() != _napasHash.ToUpper())
            {
                logPayment.Data = 1;
                logPayment.DateEndRequest = DateTime.Now;
                _logger.LogInformation("UpdatePaymentNapas|Sai thông tin: " + JsonConvert.SerializeObject(logPayment));

                return new
                {
                    StatusCode = 1,
                    Message = "Sai thông tin."
                };
            }

            var isSuccessPay = data["vpc_ResponseCode"] != null && data["vpc_ResponseCode"].ToString() == "0";
            logPayment.IsSuccess = isSuccessPay;

            if (isSuccessPay)
            {
                //var _order = orderService.GetByOrderNumber<Order>(data["orderNumber"] != null ? data["orderNumber"].ToString() : data["vpc_OrderInfo"].ToString());
                var _order = await _receiptVoucherRepository.GetByIdAsync(data["orderNumber"] != null ? data["orderNumber"].ToString() : data["vpc_OrderInfo"].ToString());

                if (_order == null)
                {
                    logPayment.Data = 1;
                    logPayment.DateEndRequest = DateTime.Now;
                    _logger.LogInformation("UpdatePaymentNapas|Sai thông tin đơn hàng: " + JsonConvert.SerializeObject(logPayment));

                    return new
                    {
                        StatusCode = 1,
                        Message = "Sai thông tin đơn hàng"
                    };
                }

                _order.SetStatusId(ReceiptVoucherStatus.Success.Id);
                _order.UpdatedDate = DateTime.Now;
                _order.PaymentDate = DateTime.Now;
                _order.Source = "Napas";

                await _receiptVoucherRepository.UpdateAndSave(_order);

                logPayment.DateEndRequest = DateTime.Now;
                _logger.LogInformation("UpdatePaymentNapas|Cập nhật thông tin thành công: " + JsonConvert.SerializeObject(logPayment));
                return new
                {
                    StatusCode = 0,
                    Message = "Cập nhật thông tin thành công"
                };
            }
            else
            {
                logPayment.DateEndRequest = DateTime.Now;
                _logger.LogInformation("UpdatePaymentNapas|Thanh toán không thành công thành công: " + JsonConvert.SerializeObject(logPayment));
                return new
                {
                    StatusCode = 1,
                    Message = "Thanh toán không thành công thành công"
                };
            }

        }

        public async Task<object> UpdatePaymentMomo(Dictionary<string, string> data)
        {
            var logPayment = new PaymentResultModel()
            {
                //Type = StaticEnumApi.REQUEST_TYPE_VNMAPI.JSON,
                DateStartRequest = DateTime.Now,
                IsRequestSucess = true,
                Number = 1,
                Action = "UpdatePayment",
                Post = JsonConvert.SerializeObject(data),
                IsSuccess = false
            };

            var _signature = data["signature"].ToString();

            var _hashSignature = "partnerCode=" + data["partnerCode"].ToString() +
                "&accessKey=" + data["accessKey"].ToString() +
                "&requestId=" + data["requestId"].ToString() +
                "&amount=" + data["amount"].ToString() +
                "&orderId=" + data["orderId"].ToString() +
                "&orderInfo=" + data["orderInfo"].ToString() +
                "&orderType=" + data["orderType"].ToString() +
                "&transId=" + data["transId"].ToString() +
                "&message=" + data["message"].ToString() +
                "&localMessage=" + data["localMessage"].ToString() +
                "&responseTime=" + data["responseTime"].ToString() +
                "&errorCode=" + data["errorCode"].ToString() +
                "&payType=" + data["payType"].ToString() +
                "&extraData=" + data["extraData"].ToString();

            _hashSignature = EncryptionHelper.EncryptSHA256(_hashSignature, _configuration.GetValue<string>("MomoSecretKey"));

            if (_signature.ToUpper() != _hashSignature.ToUpper())
            {
                logPayment.Data = 1;
                logPayment.DateEndRequest = DateTime.Now;
                _logger.LogInformation("UpdatePaymentMomo|Sai thông tin: " + JsonConvert.SerializeObject(logPayment));

                return new
                {
                    StatusCode = 1,
                    Message = "Sai thông tin."
                };
            }

            var isSuccessPay = data["errorCode"] != null && data["errorCode"].ToString() == "0";
            logPayment.IsSuccess = isSuccessPay;

            if (isSuccessPay)
            {
                var _order = await _receiptVoucherRepository.GetByIdAsync(data["orderId"].ToString());

                if (_order == null)
                {
                    logPayment.Data = 1;
                    logPayment.DateEndRequest = DateTime.Now;
                    _logger.LogInformation("UpdatePaymentMomo|Sai thông tin đơn hàng: " + JsonConvert.SerializeObject(logPayment));

                    return new
                    {
                        StatusCode = 1,
                        Message = "Sai thông tin đơn hàng"
                    };
                }

                _order.SetStatusId(ReceiptVoucherStatus.Success.Id);
                _order.UpdatedDate = DateTime.Now;
                _order.PaymentDate = DateTime.Now;
                _order.Source = "Momo";

                await _receiptVoucherRepository.UpdateAndSave(_order);

                logPayment.DateEndRequest = DateTime.Now;
                _logger.LogInformation("UpdatePaymentMomo|Cập nhật thông tin thành công: " + JsonConvert.SerializeObject(logPayment));
                return new
                {
                    StatusCode = 0,
                    Message = "Cập nhật thông tin thành công"
                };
            }

            else
            {
                logPayment.DateEndRequest = DateTime.Now;
                _logger.LogInformation("UpdatePaymentMomo|Thanh toán không thành công thành công: " + JsonConvert.SerializeObject(logPayment));
                return new
                {
                    StatusCode = 1,
                    Message = "Thanh toán không thành công thành công"
                };
            }


        }
    }
}