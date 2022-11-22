using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Seed;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ContractManagement.Domain.AggregatesModel.InContractAggregate
{
    [Table("SharingRevenueLineDetails")]
    public class SharingRevenueLineDetail : Entity
    {
        public SharingRevenueLineDetail()
        {
        }

        public SharingRevenueLineDetail(CUSharingRevenueLineDetailCommand command)
        {
            this.Id = command.Id;
            this.SharingLineUid = command.SharingLineUid;
            this.SharingLineId = command.SharingLineId;
            this.CurrencyUnitCode = command.CurrencyUnitCode;
            this.SharingType = command.SharingType;
            this.Year = command.Year;
            this.Month = command.Month;
            this.SharingAmount = command.SharingAmount;

            this.CreatedBy = command.CreatedBy;
            this.CreatedDate = command.CreatedDate;
        }

        [StringLength(68)]
        public string SharingLineUid { get; set; }
        public int? SharingLineId { get; set; }
        public string CurrencyUnitCode { get; set; }

        public int SharingType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public decimal SharingAmount { get; set; }
    }
}
