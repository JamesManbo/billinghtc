using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using FluentValidation.Results;
using Global.Configs.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Global.Models;
using Global.Models.Auth;
using Global.Models.EndPointResponse;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Authorization;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;
using System.Threading.Tasks;
using News.API.Models.Domain;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using News.API.Common;
using System.Net;
using News.API.Models.Domain.Picture;
using News.API.Models.Domain.FileAttachment;
using Global.Configs.ResourceConfig;

namespace News.API.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = Global.Configs.Authentication.AuthenticationSchemes.CmsApiIdentityKey)]
    public abstract class CustomBaseController : ControllerBase
    {
        public UserIdentity UserIdentity => new UserIdentity(HttpContext);
        public new OkObjectResult Ok()
        {
            return new OkObjectResult(EndPointResponse.Success());
        }

        public override OkObjectResult Ok(object result)
        {
            var resultType = result.GetType();

            if (resultType.IsGenericType)
            {
                if (resultType.GetGenericTypeDefinition() == typeof(IActionResponse<>) ||
                    resultType.GetGenericTypeDefinition() == typeof(ActionResponse<>))
                {
                    var message = resultType.GetProperty("Message")?.GetValue(result, null)?.ToString();
                    var resultValue = resultType.GetProperty("Result")?.GetValue(result, null);
                    return new OkObjectResult(EndPointHasResultResponse.Success(resultValue, message));
                }
            }

            if (result is IActionResponse actionResponse)
            {
                return new OkObjectResult(EndPointResponse.Success(actionResponse.Message));
            }

            return new OkObjectResult(EndPointHasResultResponse.Success(result));
        }

        public new BadRequestObjectResult BadRequest()
        {
            return new BadRequestObjectResult(EndPointResponse.Failed("Yêu cầu không hợp lệ"));
        }

        public override BadRequestObjectResult BadRequest(ModelStateDictionary modelState)
        {
            return BadRequest((object) modelState);
        }

        public override BadRequestObjectResult BadRequest(object error)
        {
            switch (error)
            {
                case IActionResponse response:
                    response.ClearResult();
                    return base.BadRequest(response);
                case ModelStateDictionary modelStateDic:
                    {
                        var commonErrors = new List<ErrorGeneric>();
                        foreach (var modelState in modelStateDic)
                        {
                            var validateError =
                                new ValidationResult(string.Join(',', modelState.Value.Errors.Select(r => r.ErrorMessage)),
                                    new[] { modelState.Key });
                            commonErrors.Add(new ErrorGeneric(validateError));
                        }

                        return base.BadRequest(new EndPointResponse()
                        {
                            IsSuccess = false,
                            Message = $"Thất bại, phát hiện {commonErrors.Count} lỗi",
                            Errors = commonErrors
                        });
                    }
                case ErrorGeneric errorGeneric:
                    return base.BadRequest(new EndPointResponse()
                    {
                        IsSuccess = false,
                        Message = "Thất bại, phát hiện có lỗi xảy ra",
                        Errors = new List<ErrorGeneric>() { errorGeneric }
                    });
                case string errorMessage:
                    return base.BadRequest(new EndPointResponse()
                    {
                        IsSuccess = false,
                        Message = errorMessage
                    });
                case IList<ValidationFailure> validationResults:
                    return base.BadRequest(new EndPointResponse()
                    {
                        IsSuccess = false,
                        Message = $"Thất bại, phát hiện {validationResults.Count} lỗi.",
                        Errors = validationResults.Select(e => new ErrorGeneric(e.ErrorMessage, e.PropertyName))
                            .ToList()
                    });
                default:
                    return base.BadRequest(new EndPointResponse()
                    {
                        IsSuccess = false,
                        Message = "Yêu cầu không hợp lệ"
                    });
            }
        }

        protected async Task<List<PictureViewModel>> StoreAndSavePicture(List<string> tempFilePaths)
        {
            var httpClientHandler = new HttpClientHandler();
            using (HttpClient client = new HttpClient(httpClientHandler, true))
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("ContentType", "application/json; charset=utf-8");

                var content = new StringContent(
                    JsonConvert.SerializeObject(tempFilePaths),
                    Encoding.UTF8,
                    "application/json");

                var uploadResponse = await client.PutAsync(ResourceGlobalConfigs.MediaSourceURL + "api/Upload/images", content);
                if (uploadResponse.IsSuccessStatusCode && uploadResponse.StatusCode == HttpStatusCode.OK)
                {
                    var responseContent = await uploadResponse.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<PictureViewModel>>(responseContent);
                }

                return null;
            }
        }

        protected async Task<PictureViewModel> StoreAndSavePicture(string tempFilePaths)
        {
            var httpClientHandler = new HttpClientHandler();
            using (HttpClient client = new HttpClient(httpClientHandler, true))
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("ContentType", "application/json; charset=utf-8");

                var content = new StringContent(
                    JsonConvert.SerializeObject(new List<string>() { tempFilePaths }),
                    Encoding.UTF8,
                    "application/json");

                var uploadResponse = await client.PutAsync(ResourceGlobalConfigs.MediaSourceURL + "api/Upload/images", content);
                if (uploadResponse.IsSuccessStatusCode && uploadResponse.StatusCode == HttpStatusCode.OK)
                {
                    var responseContent = await uploadResponse.Content.ReadAsStringAsync();
                    var responseValue = JsonConvert.DeserializeObject<List<PictureViewModel>>(responseContent);
                    return responseValue.Any() ? responseValue.First() : null;
                }

                return null;
            }
        }

        protected async Task<List<PictureViewModel>> StoreAndSavePictures(List<PictureArticleContentItem> listPictureArticleContent)
        {
            var httpClientHandler = new HttpClientHandler();
            using (HttpClient client = new HttpClient(httpClientHandler, true))
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("ContentType", "application/json; charset=utf-8");

                var content = new StringContent(
                    JsonConvert.SerializeObject(listPictureArticleContent),
                    Encoding.UTF8,
                    "application/json");

                var uploadResponse = await client.PutAsync(ResourceGlobalConfigs.MediaSourceURL + "api/Upload/imagesArticleContent", content);
                if (uploadResponse.IsSuccessStatusCode && uploadResponse.StatusCode == HttpStatusCode.OK)
                {
                    var responseContent = await uploadResponse.Content.ReadAsStringAsync();
                    var responseValue = JsonConvert.DeserializeObject<List<PictureViewModel>>(responseContent);
                    return responseValue;
                }
                return new List<PictureViewModel>();
            }
        }

        protected async Task<List<FileAttachmentItem>> StoreAndSaveFiles(List<FileArticleContentItem> listFileArticleContentItem)
        {
            var httpClientHandler = new HttpClientHandler();
            using (HttpClient client = new HttpClient(httpClientHandler, true))
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("ContentType", "application/json; charset=utf-8");

                var content = new StringContent(
                    JsonConvert.SerializeObject(listFileArticleContentItem),
                    Encoding.UTF8,
                    "application/json");

                var uploadResponse = await client.PutAsync(ResourceGlobalConfigs.MediaSourceURL + "api/UploadFiles/filesArticleContent", content);
                if (uploadResponse.IsSuccessStatusCode && uploadResponse.StatusCode == HttpStatusCode.OK)
                {
                    var responseContent = await uploadResponse.Content.ReadAsStringAsync();
                    var responseValue = JsonConvert.DeserializeObject<List<FileAttachmentItem>>(responseContent);
                    return responseValue;
                }
                return new List<FileAttachmentItem>();
            }
        }
    }
}