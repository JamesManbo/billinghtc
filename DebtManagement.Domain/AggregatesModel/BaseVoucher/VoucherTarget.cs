using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using DebtManagement.Domain.Seed;

namespace DebtManagement.Domain.AggregatesModel.BaseVoucher
{
    [Table("VoucherTargets")]
    public class VoucherTarget : Entity
    {
        [StringLength(256)]
        public string TargetFullName { get; set; }
        [StringLength(256)]
        public string TargetCode { get; set; }
        [StringLength(1000)]
        public string TargetAddress { get; set; }
        [StringLength(256)]
        public string TargetPhone { get; set; }
        [StringLength(256)]
        public string TargetEmail { get; set; }
        [StringLength(256)]
        public string TargetFax { get; set; }
        [StringLength(256)]
        public string TargetIdNo { get; set; }
        [StringLength(256)]
        public string TargetTaxIdNo { get; set; }
        public string TargetBRNo { get; set; } // Số đăng ký kinh doanh

        public string City { get; set; }
        public string CityId { get; set; }
        public string District { get; set; }
        public string DistrictId { get; set; }

        public bool IsEnterprise { get; set; }
        public bool IsPayer { get; set; } // true: buyer, false: seller, provider
        public bool IsPartner { get; set; }
        public string UserIdentityGuid { get; set; }
        public string ApplicationUserIdentityGuid { get; set; }
        /// <summary>
        /// Phục vụ tăng hiệu suất sắp xếp khi thống kê công nợ của khách hàng
        /// Giá trị tương đối, không hoàn toàn chính xác 100%
        /// </summary>
        public decimal CurrentDebt { get; set; }
    }
}
