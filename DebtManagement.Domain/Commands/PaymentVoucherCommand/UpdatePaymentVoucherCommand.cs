using DebtManagement.Domain.Commands.BaseVoucherCommand;
using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Models;
using Global.Models.StateChangedResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace DebtManagement.Domain.Commands.PaymentVoucherCommand
{
    public class UpdatePaymentVoucherCommand : CUVoucherBaseCommand, IRequest<ActionResponse<PaymentVoucherDTO>>
    {
        public List<CUPaymentVoucherDetailCommand> PaymentVoucherDetails { get; set; }
        public List<CuPaymentVoucherPaymentDetailCommand> PaymentDetails { get; set; }
        public bool isUpdateNextBillingDate { get; set; }

        public UpdatePaymentVoucherCommand()
        {
            PaymentVoucherDetails = new List<CUPaymentVoucherDetailCommand>();
            PaymentDetails = new List<CuPaymentVoucherPaymentDetailCommand>();
        }

    }
}