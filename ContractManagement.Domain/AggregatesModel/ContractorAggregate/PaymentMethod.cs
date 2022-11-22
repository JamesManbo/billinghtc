using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.ContractorAggregate
{
    public class PaymentMethod : ValueObject
    {
        public int Form { get; set; } // {'Trả trước': 1, 'Trả sau': 0}
        public int Method { get; set; } // phương thức thanh toán {'Chuyển khoản': 1, 'Tiền mặt': 0}
        public string Address { get; set; } // địa chỉ thanh toán

        protected PaymentMethod() { }

        public PaymentMethod(int form, int method, string address)
        {
            if(form < 0 && form > 1) throw new ContractDomainException("Payment form is not valid");
            if(method < 0 && method > 1) throw new ContractDomainException("Payment method is not valid");
            this.Form = form;
            this.Method = method;
            this.Address = address;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Form;
            yield return Method;
            yield return Address;
        }
    }

    public enum PaymentMethodForm
    {
        Prepaid = 1, // trả trước
        Postpaid = 0 // trả sau
    }
}
