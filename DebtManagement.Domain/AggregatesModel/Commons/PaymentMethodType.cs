using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using DebtManagement.Domain.Seed;

namespace DebtManagement.Domain.AggregatesModel.Commons
{
    [Table("PaymentMethodTypes")]
    public class PaymentMethodType : Enumeration
    {
        public static PaymentMethodType COD = new PaymentMethodType(1, "Tiền mặt");
        public static PaymentMethodType BankTransfer = new PaymentMethodType(2, "Chuyển khoản");
        public static PaymentMethodType ClearingDebt = new PaymentMethodType(3, "Bù trừ");

        public PaymentMethodType(int id, string name) : base(id, name)
        {
        }

        public static List<PaymentMethodType> List()
        {
            return new List<PaymentMethodType>
            {
                COD, BankTransfer, ClearingDebt
            };
        }
    }
}