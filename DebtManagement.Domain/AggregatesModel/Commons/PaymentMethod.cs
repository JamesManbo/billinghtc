using DebtManagement.Domain.Exceptions;
using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DebtManagement.Domain.AggregatesModel.Commons
{
    public class PaymentMethod : ValueObject
    {
        public PaymentMethodForm Form { get; set; }
        public int Method { get; set; } // phương thức thanh toán
        public string Address { get; set; } // địa chỉ thanh toán
        public string BankAccount { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }

        protected PaymentMethod() { }

        public PaymentMethod(PaymentMethodForm form, int method, string address)
        {
            if((int) form < 0 && (int)form > 1) throw new DebtDomainException("Payment form is not valid");
            if(method < 0 && method > 1) throw new DebtDomainException("Payment method is not valid");
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
