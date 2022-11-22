using DebtManagement.Domain.AggregatesModel.Commons;
using DebtManagement.Domain.Commands.Commons;
using DebtManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.ClearingCommand
{
    public class CUClearingCommand: IRequest<ActionResponse<ClearingDTO>>
    {
        public string Id { get; set; }
        public int CurrencyUnitId { get; set; }
        public string CurrencyUnitCode { get; set; }
        public string CodeClearing { get; set; }
        public string CalculatorUserId { get; set; }
        public int PaymentConfirmerUserId { get; set; }
        public DateTime ClearingDate { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int StatusId { get; set; }
        public string Description { get; set; }
        public int CustomerId { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public PaymentMethod Payment { get; set; }
        public int TargetId { get; set; }
        public string CreatedUserId { get; set; }
        public string OrganizationUnitId { get; set; }
        public string ApprovalUserId { get; set; }
        public int MarketAreaId { get; set; }
        public string MarketAreaName { get; set; }
        public decimal ClearingTotal { get; set; }
        public decimal TotalReceipt { get; set; }
        public decimal TotalPayment { get; set; }
        public List<int> SelectionReceiptIds { get; set; }
        public List<int> SelectionPaymentIds { get; set; }
        public List<AttachmentFileCommand> AttachmentFiles { get; set; }
        public string CancelReason { get; set; }
    }
}
