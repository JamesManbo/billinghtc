using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StaffApp.APIGateway.Models.CuReceiptVoucherCommands
{
    public class CuReceiptVoucherCommand : CUVoucherBaseCommand
    {
        public CuReceiptVoucherCommand()
        {
            ReceiptLines = new List<CUReceiptVoucherDetailCommand>();
            ReceiptVoucherTaxes = new List<CreateReceiptVoucherTaxCommand>();
            IncurredDebtPayments = new List<CuOpeningDebtPaymentCommand>();
            OpeningDebtPayments = new List<CuOpeningDebtPaymentCommand>();
        }

        public int OutContractId { get; set; }
        public decimal InstallationFee { get; set; }
        public decimal PromotionTotalAmount { get; set; }
        public decimal EquipmentTotalAmount { get; set; }
        public DateTime? CashierReceivedDate { get; set; }
        public bool IsFirstVoucherOfContract { get; set; }
        public bool InvalidIssuedDate { get; set; }
        public List<CUReceiptVoucherDetailCommand> ReceiptLines { get; set; }
        public List<CreateReceiptVoucherTaxCommand> ReceiptVoucherTaxes { get; set; }
        public List<CuOpeningDebtPaymentCommand> IncurredDebtPayments { get; set; }
        public List<CuOpeningDebtPaymentCommand> OpeningDebtPayments { get; set; }

        public CuReceiptVoucherCommand Copy()
        {
            var clone = (CuReceiptVoucherCommand)this.MemberwiseClone();
            clone.Id = Guid.NewGuid().ToString();
            if (clone.ReceiptLines != null && clone.ReceiptLines.Count > 0)
            {
                foreach (var line in clone.ReceiptLines)
                {
                    line.Id = Guid.NewGuid().ToString();
                }
            }
            return clone;
        }
    }
}
