using DebtManagement.Domain.Commands.ReceiptVoucherCommand;
using DebtManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DebtManagement.Domain.AggregatesModel.ReceiptVoucherAggregate
{
    [Table("ReceiptVoucherDetailReductions")]
    public class ReceiptVoucherDetailReduction : Entity
    {
        public int? ReceiptVoucherId { get; set; }
        public int ReceiptVoucherDetailId { get; set; }

        public string ReasonId { get; set; }
        public string ReductionReason { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? StopTime { get; set; }
        public string Duration { get; set; }
        public string CId { get; set; }

        public ReceiptVoucherDetailReduction() { }
        public ReceiptVoucherDetailReduction(ReductionDetailCommand command)
        {
            CreatedDate = DateTime.Now;
            ReasonId = command.ReasonId;
            ReductionReason = command.ReductionReason;
            StartTime = command.StartTime;
            StopTime = command.StopTime;
            Duration = command.Duration;
            CId = command.CId;
        }


        public void Binding(ReductionDetailCommand command)
        {
            this.ReasonId = command.ReasonId;
            this.ReductionReason = command.ReductionReason;
            this.StartTime = command.StartTime;
            this.StopTime = command.StopTime;
            this.Duration = command.Duration;
            this.CId = command.CId;

        }
    }
}
