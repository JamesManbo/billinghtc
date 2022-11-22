using DebtManagement.Domain.AggregatesModel.PaymentVoucherAggregate;
using DebtManagement.Domain.Commands.BaseVoucherCommand;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;

namespace DebtManagement.Domain.Commands.PaymentVoucherCommand
{
    public class CreatePaymentVoucherCommand : CUVoucherBaseCommand, IRequest<ActionResponse<PaymentVoucherDTO>>
    {
        public CreatePaymentVoucherCommand()
        {
            PaymentVoucherDetails = new List<CUPaymentVoucherDetailCommand>();
            PaymentVoucherTaxes = new List<CreatePaymentVoucherTaxCommand>();
            ReceiptVouchers = new List<ReceiptVoucherInPaymentVoucher>();
        }
        public List<CUPaymentVoucherDetailCommand> PaymentVoucherDetails { get; set; }
        public List<CreatePaymentVoucherTaxCommand> PaymentVoucherTaxes { get; set; }
        public List<CuPaymentVoucherPaymentDetailCommand> PaymentDetails { get; set; }

        public List<ReceiptVoucherInPaymentVoucher> ReceiptVouchers { get; set; }
    }
}
