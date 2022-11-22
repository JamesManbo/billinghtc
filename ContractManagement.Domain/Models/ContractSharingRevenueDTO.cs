using System;
using System.Collections.Generic;
using System.Text;

namespace ContractManagement.Domain.Models
{
    public class ContractSharingRevenueDTO : BaseDTO
    {
        public ContractSharingRevenueDTO()
        {
            ContractSharingRevenueLines = new List<ContractSharingRevenueLineDTO>();
        }
        //public int Id { get; set; }
        public string Uid { get; set; }
        //public int Year { get; set; }
        public string ChannelTemporaryId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelCid { get; set; }
        public int OutChannelId { get; set; }
        public int InContractId { get; set; }
        public string InContractCode { get; set; }
        public int? OutContractId { get; set; }
        public string OutContractCode { get; set; }
        public int SharingType { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ContractSharingRevenueLineDTO> ContractSharingRevenueLines { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int CostTerm { get; set; } //Thời hạn chi phí
        public decimal TaxMoney { get; set; } //Tiền thuế
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }
    }
}
