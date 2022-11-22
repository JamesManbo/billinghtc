using System;
using System.Collections.Generic;
using System.Text;
using DebtManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;

namespace DebtManagement.Domain.Commands.BaseVoucherCommand
{
    public class CUVoucherTargetCommand : IRequest<ActionResponse<VoucherTargetDTO>>
    {
        public int Id { get; set; }
        public string IdentityGuid { get; set; }
        public string TargetFullName { get; set; }
        public string TargetCode { get; set; }
        public string TargetAddress { get; set; }
        public string TargetPhone { get; set; }
        public string TargetEmail { get; set; }
        public string TargetFax { get; set; }
        public string TargetIdNo { get; set; }
        public string TargetTaxIdNo { get; set; }
        public bool IsEnterprise { get; set; }
        public bool IsPayer { get; set; }
        public bool IsPartner { get; set; }
        public string UserIdentityGuid { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }
    }
}
