using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Models.EndPointResponse
{
    public class EndPointHasResultResponse : EndPointResponse
    {
        public static EndPointHasResultResponse Success(object result, string message = "Thành công")
        {
            return new EndPointHasResultResponse()
            {
                Result = result,
                Message = message,
                IsSuccess = true
            };
        }

        public object Result { get; set; }
    }
}
