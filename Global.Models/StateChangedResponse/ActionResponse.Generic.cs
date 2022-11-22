using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Models.StateChangedResponse
{
    public class ActionResponse<T> : ActionResponse, IActionResponse<T>
    {
        public static ActionResponse<T> Success(T result)
        {
            var response = new ActionResponse<T>();
            response.SetResult(result);
            return response;
        }

        public static ActionResponse<T> Failed(string message, string errorMember = "")
        {
            var actResponse = new ActionResponse<T>();
            actResponse.AddError(message, errorMember);
            return actResponse;
        }

        public static ActionResponse<T> Failed(ErrorGeneric err)
        {
            var actResponse = new ActionResponse<T>();
            actResponse.AddError(err);
            return actResponse;
        }

        private T _result;

        /// <summary>
        /// This is the returned result
        /// </summary>
        public T Result => _result;

        public ActionResponse()
        {
        }

        public override ActionResponse ClearResult()
        {
            _result = default(T);
            var actionResponse = new ActionResponse();
            actionResponse.CombineResponse(this);
            return actionResponse;
        }

        /// <summary>
        /// This sets the result to be returned
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public ActionResponse<T> SetResult(T result)
        {
            _result = result;
            return this;
        }
    }
}