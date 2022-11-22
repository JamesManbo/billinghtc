using CustomerApp.APIGateway.Models.CommonModels;
using CustomerApp.APIGateway.Models.OutContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerApp.APIGateway.Models.ReceiptVoucherModels
{
    public class ReceiptVoucherGridDTO
    {
        public string Id { get; set; }
        public ReceiptVoucherGridDTO()
        {
            ReceiptLines = new List<ReceiptVoucherDetailGridDTO>();
        }

        public string MarketAreaName { get; set; }
        public string ProjectName { get; set; }
        public int TypeId { get; set; }
        public int StatusId { get; set; }
        public string VoucherCode { get; set; }
        public int OutContractId { get; set; }
        public string ContractCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateFormat { get { return CreatedDate.ToString("dd/MM/yyyy"); } }
        public DateTime IssuedDate { get; set; }
        public string IssuedDateFormat { get { return IssuedDate.ToString("dd/MM/yyyy"); } }
        public string StatusName { get; set; }
        public string InvoiceCode { get; set; }
        public string Content { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public MoneyDTO ReductionFreeTotal { get; set; }
        public MoneyDTO CashTotal { get; set; }
        public MoneyDTO SubTotal { get; set; }
        public MoneyDTO GrandTotal { get; set; }
        public MoneyDTO GrandTotalIncludeDebt { get; set; }
        public MoneyDTO PaidTotal { get; set; }
        public MoneyDTO RemainingTotal { get; set; }
        public MoneyDTO PayerName { get; set; }
        public MoneyDTO DiscountAmount { get; set; }
        public string TargetFullName { get; set; }
        public bool IsEnterprise { get; set; }
        public PaymentMethodDTO Payment { get; set; }
        public List<ReceiptVoucherDetailGridDTO> ReceiptLines { get; set; }
    }
}
