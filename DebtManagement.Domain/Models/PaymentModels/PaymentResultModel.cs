using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.PaymentModels
{
    public class PaymentResultModel
    {
        public string Action { get; set; }
        public string StatusCode { get; set; }
        public string TransactionId { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public string Post { get; set; }
        public string Result { get; set; }
        public int Number { get; set; }
        public string Excetion { get; set; }
        public int RequestStatusCode { get; set; }
        public bool IsRequestSucess { get; set; }
        public bool IsSuccess { get; set; }
        public DateTime DateStartRequest { get; set; }
        public DateTime DateEndRequest { get; set; }
        public int SecondExecute { get; set; }
    }
}
