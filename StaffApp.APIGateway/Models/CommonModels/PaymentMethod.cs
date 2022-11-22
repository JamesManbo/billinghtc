using System.Collections.Generic;

namespace StaffApp.APIGateway.Models.CommonModels
{
    public class PaymentMethod
    {
        public int Form { get; set; } // hình thức thanh toán(0: trả sau, 1: trả trước)
        public int Method { get; set; } // phương thức thanh toán
        public string MethodName { get { return Method == 0 ? "Tiền mặt" : Method == 1 ? "Chuyển khoản" : Method == 9 ? "Bù trừ thanh toán" : ""; } }
        public string Address { get; set; } // địa chỉ thanh toán

        protected PaymentMethod() { }

        public PaymentMethod(int form, int method, string address)
        {
            this.Form = form;
            this.Method = method;
            this.Address = address;
        }
    }
}
