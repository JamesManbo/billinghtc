using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DebtManagement.Domain.AggregatesModel.BaseVoucher
{
    [Table("VoucherPaymentMethods")]
    public class VoucherPaymentMethod : Enumeration
    {
        public bool IsPassive { get; set; }
        public static VoucherPaymentMethod Cash = new VoucherPaymentMethod(0, "Tiền mặt");
        public static VoucherPaymentMethod Transfer = new VoucherPaymentMethod(1, "Chuyển khoản");
        public static VoucherPaymentMethod Clearing = new VoucherPaymentMethod(2, "Bù trừ", true);

        public VoucherPaymentMethod(int id, string name, bool passive = false) : base(id, name)
        {
            this.IsPassive = passive;
        }
        
        public VoucherPaymentMethod(int id, string name) : base(id, name)
        {
            this.IsPassive = false;
        }


        public static VoucherPaymentMethod[] List()
        {
            return new[]
            {
                Cash,
                Transfer,
                Clearing
            };
        }

        public static VoucherPaymentMethod[] ActiveList()
        {
            return new[]
            {
                Cash,
                Transfer
            };
        }
    }
}
