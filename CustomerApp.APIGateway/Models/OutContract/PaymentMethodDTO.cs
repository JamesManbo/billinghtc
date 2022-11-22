using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.OutContract
{
    public class PaymentMethodDTO
    {
        public int Form { get; set; }
        public int Method { get; set; }
        public string MethodName { get { return Method == 0 ? "Tiền mặt" : Method == 1 ? "Chuyển khoản" : Method == 9 ? "Bù trừ thanh toán" : ""; } }
        public string Address { get; set; }
    }
}
