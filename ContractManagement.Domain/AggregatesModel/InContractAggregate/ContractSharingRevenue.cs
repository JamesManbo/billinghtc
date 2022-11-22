using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Commands.InContractCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Seed;

namespace ContractManagement.Domain.AggregatesModel.InContractAggregate
{
    [Table("ContractSharingRevenues")]
    public class ContractSharingRevenue : Entity
    {
        public string Uid { get; set; }
        //public int Year { get; set; }
        public string ChannelTemporaryId { get; set; }
        public string ChannelName { get; set; }
        public string ChannelCid { get; set; }
        public int InContractId { get; set; }
        [StringLength(256)]
        public string InContractCode { get; set; }
        public int OutChannelId { get; set; }
        public int? OutContractId { get; set; }
        [StringLength(256)]
        public string OutContractCode { get; set; }
        public int SharingType { get; set; }
        public decimal TotalAmount { get; set; }

        public decimal TotalAmountAfterTax { get; set; }

        public int CostTerm { get; set; } //Thời hạn chi phí
        public decimal TaxMoney { get; set; } //Tiền thuế
        public int CurrencyUnitId { get; set; } // Đơn vị tiền tệ
        public string CurrencyUnitCode { get; set; }

        public IReadOnlyCollection<ContractSharingRevenueLine> ContractSharingRevenueLines =>
            _contractSharingRevenueLines;

        private readonly List<ContractSharingRevenueLine> _contractSharingRevenueLines;

        public ContractSharingRevenue()
        {
            _contractSharingRevenueLines = new List<ContractSharingRevenueLine>();
        }

        public ContractSharingRevenue(CUContractSharingRevenueCommand sharingRevenueCommand)
        {
            _contractSharingRevenueLines = new List<ContractSharingRevenueLine>();

            if (sharingRevenueCommand.InContractId < 0)
            {
                throw new ContractDomainException("Id, mã hợp đồng đầu vào không hợp lệ");
            }

            InContractId = sharingRevenueCommand.InContractId;
            InContractCode = sharingRevenueCommand.InContractCode;

            CurrencyUnitId = sharingRevenueCommand.CurrencyUnitId;
            CurrencyUnitCode = sharingRevenueCommand.CurrencyUnitCode;

            if (sharingRevenueCommand.OutContractId.HasValue && sharingRevenueCommand.OutContractId <= 0)
            {
                throw new ContractDomainException("Id, mã hợp đồng đầu ra không hợp lệ");
            }

            OutContractId = sharingRevenueCommand.OutContractId;
            OutContractCode = sharingRevenueCommand.OutContractCode;

            SharingType = sharingRevenueCommand.SharingType;

            Uid = sharingRevenueCommand.Uid;
            CreatedBy = sharingRevenueCommand.CreatedBy;
            UpdatedBy = sharingRevenueCommand.UpdatedBy;
            CreatedDate = sharingRevenueCommand.CreatedDate;
            UpdatedDate = sharingRevenueCommand.UpdatedDate;

            ChannelCid = sharingRevenueCommand.ChannelCid;
            ChannelName = sharingRevenueCommand.ChannelName;
            ChannelTemporaryId = sharingRevenueCommand.ChannelTemporaryId;
            OutChannelId = sharingRevenueCommand.OutChannelId;
            TotalAmount = sharingRevenueCommand.TotalAmount;
            CostTerm = sharingRevenueCommand.CostTerm;
        }

        public void RemoveContractSharingRevenueLines(int[] removeIds)
        {
            _contractSharingRevenueLines.RemoveAll(e => removeIds.Contains(e.Id));
        }

        public void ClearContractSharingRevenueLines()
        {
            this._contractSharingRevenueLines.Clear();
        }

        public void AddContractSharingRevenueLine(ContractSharingRevenueLine contractSharingRevenueLine)
        {
            this._contractSharingRevenueLines.Add(contractSharingRevenueLine);
        }

        public ContractSharingRevenue Update(CUContractSharingRevenueCommand contractSharingRevenueCommand)
        {
            UpdatedDate = DateTime.Now;
            UpdatedBy = contractSharingRevenueCommand.UpdatedBy;
            InContractId = contractSharingRevenueCommand.InContractId;
            InContractCode = contractSharingRevenueCommand.InContractCode;
            OutContractId = contractSharingRevenueCommand.OutContractId;
            OutContractCode = contractSharingRevenueCommand.OutContractCode;

            SharingType = contractSharingRevenueCommand.SharingType;

            Uid = contractSharingRevenueCommand.Uid;
            CreatedBy = contractSharingRevenueCommand.CreatedBy;
            UpdatedBy = contractSharingRevenueCommand.UpdatedBy;
            CreatedDate = contractSharingRevenueCommand.CreatedDate;
            UpdatedDate = contractSharingRevenueCommand.UpdatedDate;

            ChannelCid = contractSharingRevenueCommand.ChannelCid;
            ChannelName = contractSharingRevenueCommand.ChannelName;
            ChannelTemporaryId = contractSharingRevenueCommand.ChannelTemporaryId;
            OutChannelId = contractSharingRevenueCommand.OutChannelId;
            TotalAmount = contractSharingRevenueCommand.TotalAmount;
            CostTerm = contractSharingRevenueCommand.CostTerm;
            CurrencyUnitId = contractSharingRevenueCommand.CurrencyUnitId;
            CurrencyUnitCode = contractSharingRevenueCommand.CurrencyUnitCode;

            ClearContractSharingRevenueLines();
            if (contractSharingRevenueCommand.ContractSharingRevenueLines.Any())
            {
                foreach(var csrLine in contractSharingRevenueCommand.ContractSharingRevenueLines)
                {
                    AddContractSharingRevenueLine(new ContractSharingRevenueLine(csrLine));
                }
            }

            return this;
        }
    }
}