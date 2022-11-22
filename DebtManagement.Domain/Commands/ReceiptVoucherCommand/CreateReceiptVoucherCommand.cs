
using DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate;
using DebtManagement.Domain.Commands.BaseVoucherCommand;
using DebtManagement.Domain.Commands.DebtCommand;
using DebtManagement.Domain.Models;
using DebtManagement.Domain.Models.DebtModels.OutDebts;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.ReceiptVoucherCommand
{
    public class CreateReceiptVoucherCommand : CUVoucherBaseCommand, IRequest<ActionResponse<ReceiptVoucherDTO>>
    {
        public CreateReceiptVoucherCommand()
        {
            ReceiptLines = new List<CUReceiptVoucherDetailCommand>();
            IncurredDebtPayments = new List<CuOpeningDebtPaymentCommand>();
            OpeningDebtPayments = new List<CuOpeningDebtPaymentCommand>();

            PromotionForReceiptNews = new List<PromotionForReceiptVoucherCommand>();
            PromotionForReceiptDels = new List<PromotionForReceiptVoucherCommand>();
        }

        public int OutContractId { get; set; }
        public DateTime? CashierReceivedDate { get; set; }
        public bool IsFirstVoucherOfContract { get; set; }
        public bool InvalidIssuedDate { get; set; }
        /// <summary>
        /// Phí thu tiền tận nơi
        /// </summary>
        public bool IsHasCollectionFee { get; set; }
        public decimal CODCollectionFee { get; set; }
        public List<CUReceiptVoucherDetailCommand> ReceiptLines { get; set; }
        public List<CuOpeningDebtPaymentCommand> IncurredDebtPayments { get; set; }
        public List<CuOpeningDebtPaymentCommand> OpeningDebtPayments { get; set; }        
        public CreateReceiptVoucherCommand Copy() {
            var clone = (CreateReceiptVoucherCommand) this.MemberwiseClone();
            clone.ReceiptLines = new List<CUReceiptVoucherDetailCommand>();
            clone.IncurredDebtPayments = new List<CuOpeningDebtPaymentCommand>();
            clone.OpeningDebtPayments = new List<CuOpeningDebtPaymentCommand>();
            clone.PromotionForReceiptNews = new List<PromotionForReceiptVoucherCommand>();
            clone.PromotionForReceiptDels = new List<PromotionForReceiptVoucherCommand>();
            return clone;
        }

        public List<PromotionForReceiptVoucherCommand> PromotionForReceiptNews { get; set; }
        public List<PromotionForReceiptVoucherCommand> PromotionForReceiptDels { get; set; }
    }
}
