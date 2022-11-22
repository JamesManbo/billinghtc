using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.BaseVoucherCommand;
using DebtManagement.Domain.Commands.DebtCommand;
using DebtManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.ReceiptVoucherCommand
{
    public class UpdateReceiptVoucherCommand : CUVoucherBaseCommand, IRequest<ActionResponse<ReceiptVoucherDTO>>
    {
        public UpdateReceiptVoucherCommand()
        {
            OpeningDebtPayments = new List<CuOpeningDebtPaymentCommand>();
            IncurredDebtPayments = new List<CuOpeningDebtPaymentCommand>();

            PromotionForReceiptNews = new List<PromotionForReceiptVoucherCommand>();
            PromotionForReceiptDels = new List<PromotionForReceiptVoucherCommand>();
        }

        public int OutContractId { get; set; }
        public string PayerIdId { get; set; }
        public DateTime? CashierReceivedDate { get; set; }
        public string CancellationReason { get; set; }
        public bool? IsBadDebt { get; set; }
        /// <summary>
        /// Phí thu tiền tận nơi
        /// </summary>
        public bool IsHasCollectionFee { get; set; }
        public decimal CODCollectionFee { get; set; }
        public List<CUReceiptVoucherDetailCommand> ReceiptLines { get; set; }
        public List<CuOpeningDebtPaymentCommand> IncurredDebtPayments { get; set; }
        public List<CuOpeningDebtPaymentCommand> OpeningDebtPayments { get; set; }
        public List<PromotionForReceiptVoucherCommand> PromotionForReceiptNews { get; set; }
        public List<PromotionForReceiptVoucherCommand> PromotionForReceiptDels { get; set; }
    }
}
