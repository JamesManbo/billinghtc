using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Models.EndPointResponse
{
    public class EndPointResponse
    {
        public static EndPointResponse Success(string message = "Thành công")
        {
            return new EndPointResponse
            {
                IsSuccess = true,
                Message = message
            };
        }
        public static EndPointResponse Failed(string message, List<ErrorGeneric> errors = null)
        {
            return new EndPointResponse
            {
                IsSuccess = false,
                Errors = errors,
                Message = message
            };
        }

        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<ErrorGeneric> Errors { get; set; }
    }
}