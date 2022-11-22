using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.ReceiptVoucherModels
{
    public class VoucherTargetDTO
    {
        public int Id { get; set; }
        public string TargetFullName { get; set; }
        public string TargetAddress { get; set; }
        public string TargetPhone { get; set; }
        public string TargetEmail { get; set; }
        public string TargetCode { get; set; }
        public string TargetFax { get; set; }
        public string TargetIdNo { get; set; }
        public string TargetTaxIdNo { get; set; }
        public bool IsEnterprise { get; set; }
        public string UserIdentityGuid { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }
    }
}
