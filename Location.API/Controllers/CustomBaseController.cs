using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using Global.Models;
using Global.Models.Auth;
using Global.Models.EndPointResponse;
using Global.Models.StateChangedResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace Location.API.Controllers
{
    [ApiController]
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
            return BadRequest((object)modelState);
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
                case string errorMessage:
                    return base.BadRequest(new EndPointResponse()
                    {
                        IsSuccess = false,
                        Message = errorMessage
                    });
                case IList<ValidationFailure> validationResults:
                    validationResults = validationResults.GroupBy(x => x.PropertyName)
                        .Select(v => v.First())
                        .ToList();
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
    }
}
