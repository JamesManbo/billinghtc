using System;
using System.Collections.Generic;

namespace ContractManagement.RadiusDomain.Models
{
    public partial class RmInvoices
    {
        public int Id { get; set; }
        public sbyte Invgroup { get; set; }
        public string Invnum { get; set; }
        public string Managername { get; set; }
        public string Username { get; set; }
        public DateTime Date { get; set; }
        public long Bytesdl { get; set; }
        public long Bytesul { get; set; }
        public long Bytescomb { get; set; }
        public long Downlimit { get; set; }
        public long Uplimit { get; set; }
        public long Comblimit { get; set; }
        public int Time { get; set; }
        public long Uptimelimit { get; set; }
        public int Days { get; set; }
        public DateTime Expiration { get; set; }
        public sbyte Capdl { get; set; }
        public sbyte Capul { get; set; }
        public sbyte Captotal { get; set; }
        public sbyte Captime { get; set; }
        public sbyte Capdate { get; set; }
        public string Service { get; set; }
        public string Comment { get; set; }
        public string Transid { get; set; }
        public decimal Amount { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Fullname { get; set; }
        public string Taxid { get; set; }
        public DateTime Paymentopt { get; set; }
        public sbyte Invtype { get; set; }
        public sbyte Paymode { get; set; }
        public DateTime Paid { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public decimal Vatpercent { get; set; }
        public string Remark { get; set; }
        public decimal Balance { get; set; }
        public string Gwtransid { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
    }
}
