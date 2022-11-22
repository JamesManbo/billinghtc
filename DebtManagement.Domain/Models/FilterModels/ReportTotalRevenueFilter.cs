using Global.Models.Filter;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Models.FilterModels
{
    public class ReportTotalRevenueFilter : RequestFilterModel
    {
        private string _orderBy;
        //Mã hđ, ngày nghiệm thu tính cước
        
        public DateTime? TimeLine_Effective_StartDate { get; set; }
        public DateTime? TimeLine_Effective_EndDate { get; set; }
        public string CustomerInfor { get; set; } //Tên, sđt, địa chỉ khách hàng, mã khách hàng
        public string OutContractCode { get; set; } // Mã hợp đồng đầu ra
        public string InvoiceCode { get; set; }
        public override string OrderBy
        {
            get => !string.IsNullOrWhiteSpace(_orderBy) ? _orderBy : "Id";

            set => _orderBy = value;
        }
    }
}
